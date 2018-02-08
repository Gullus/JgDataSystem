using JgLibHelper;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JgDienstScannerMaschine
{
    public class JgScannerMaschinen
    {
        private Task TaskScannerMaschine;

        public JgScannerMaschinen()
        { }

        public void TaskScannerMaschineStarten(JgOptionenCraddle CraddelOptionen)
        {
            var ctScannerMaschine = CraddelOptionen.JgOpt.CraddelTokensSource.Token;

            TaskScannerMaschine = new Task((optCraddel) =>
            {
                var optCrad = (JgOptionenCraddle)optCraddel;
                var auswertScanner = new JgScannerAuswertung(optCrad);

                var msg = "";
                TcpClient client = null;
                NetworkStream netStream = null;

                while (true)
                {
                    Logger.Write($"Verbindungsaufbau {optCrad.Info}", "Service", 0, 0, System.Diagnostics.TraceEventType.Information);

                    if (!Helper.IstPingOk(optCrad.CraddleIpAdresse, out msg))
                    {
                        Logger.Write($"Ping Fehler {optCrad.Info}\nGrund: {msg}", "Service", 0, 0, System.Diagnostics.TraceEventType.Information);
                        Thread.Sleep(20000);
                        continue;
                    }

                    try
                    {
                        client = new TcpClient(optCrad.CraddleIpAdresse, optCrad.CraddlePort);
                    }
                    catch (Exception ex)
                    {
                        Logger.Write($"Fehler Verbindungsaufbau {optCrad.Info}\nGrund: {ex.Message}", "Service", 0, 0, System.Diagnostics.TraceEventType.Information);
                        Thread.Sleep(30000);
                        continue;
                    }

                    Logger.Write($"Verbindung Ok {optCrad.Info}", "Service", 0, 0, System.Diagnostics.TraceEventType.Information);
                    netStream = client.GetStream();

                    while (true)
                    {
                        var ctsScanner = new CancellationTokenSource();
                        var ctPing = ctsScanner.Token;

                        // Wenn dieser Task eher als Scanner beendet wird, liegt ein Verbindungsfehler vor;

                        var taskKontrolle = Task.Factory.StartNew((CraddelIpAdresse) =>
                        {
                            var ipCraddle = (string)CraddelIpAdresse;

                            while (true)
                            {
                                if (ctPing.IsCancellationRequested) break;
                                Thread.Sleep(30000);
                                if (ctPing.IsCancellationRequested) break;

                                if (!Helper.IstPingOk(ipCraddle, out msg))
                                    break;

                                if (ctsScanner.IsCancellationRequested) break;
                            }
                        }, optCrad.CraddleIpAdresse, ctPing);

                        var ctScanner = ctsScanner.Token;

                        var taskScannen = Task.Factory.StartNew<string>((nStream) =>
                        {
                            var nStr = (NetworkStream)netStream;

                            byte[] bufferEmpfang = new byte[4096];
                            int anzZeichen = 0;

                            try
                            {
                                anzZeichen = nStr.Read(bufferEmpfang, 0, bufferEmpfang.Length);
                            }
                            catch (Exception ex)
                            {
                                if (!ctScanner.IsCancellationRequested)
                                {
                                    var letzteZeichen = Encoding.ASCII.GetString(bufferEmpfang, 0, anzZeichen);
                                    Logger.Write($"Fehler Datenempfang {optCrad.Info}!\nLetzter Text: {letzteZeichen}\nGrund: {ex.Message}", "Service", 1, 0, System.Diagnostics.TraceEventType.Warning);
                                    return optCrad.TextBeiFehler;
                                }
                            }

                            return Encoding.ASCII.GetString(bufferEmpfang, 0, anzZeichen);

                        }, netStream, ctScanner);

                        Task.WaitAny(new Task[] { taskKontrolle, taskScannen });
                        ctsScanner.Cancel();

                        if (taskScannen.IsCompleted)
                        {
                            auswertScanner.TextEmpfangen(taskScannen.Result);
                            if (auswertScanner.CraddeFehler)
                            {
                                byte[] senden = auswertScanner.PufferSendeText();
                                netStream.Write(senden, 0, senden.Length);
                                continue;
                            }
                        }
                        try
                        {
                            Logger.Write($"Abbruch {optCrad.Info}!", "Service", 1, 0, System.Diagnostics.TraceEventType.Warning);

                            if (client != null)
                            {
                                if (client.Connected)
                                    client.Close();
                                client.Dispose();
                            }
                            client = null;
                            if (netStream != null)
                            {
                                netStream.Close();
                                netStream.Dispose();
                            }
                            netStream = null;
                        }
                        catch { };

                        break;
                    }
                }

            }, CraddelOptionen, ctScannerMaschine, TaskCreationOptions.LongRunning);

            TaskScannerMaschine.Start();
        }
    }
}
