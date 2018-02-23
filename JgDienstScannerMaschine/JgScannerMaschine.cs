using JgLibHelper;
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
                    JgLog.Set(null, $"Verbindungsaufbau {optCrad.Info}", JgLog.LogArt.Info);

                    if (!Helper.IstPingOk(optCrad.CraddleIpAdresse, out msg))
                    {
                        JgLog.Set(null, $"Ping Fehler {optCrad.Info}\nGrund: {msg}", JgLog.LogArt.Info);
                        Thread.Sleep(20000);
                        continue;
                    }

                    try
                    {
                        client = new TcpClient(optCrad.CraddleIpAdresse, optCrad.CraddlePort);
                    }
                    catch (Exception ex)
                    {
                        JgLog.Set(null, $"Fehler Verbindungsaufbau {optCrad.Info}\nGrund: {ex.Message}", JgLog.LogArt.Info);
                        Thread.Sleep(30000);
                        continue;
                    }

                    JgLog.Set(null, $"Verbindung Ok {optCrad.Info}", JgLog.LogArt.Info);
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

                                JgLog.Set(null, $"Ping {ipCraddle} Ok.", JgLog.LogArt.Unbedeutend);
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
                                    JgLog.Set(null, $"Fehler Datenempfang {optCrad.Info}!\nLetzter Text: {letzteZeichen}\nGrund: {ex.Message}", JgLog.LogArt.Warnung);
                                    return optCrad.TextBeiFehler;
                                }
                            }

                            return Encoding.ASCII.GetString(bufferEmpfang, 0, anzZeichen);

                        }, netStream, ctScanner);

                        Task.WaitAny(new Task[] { taskKontrolle, taskScannen });
                        ctsScanner.Cancel();

                        if (taskScannen.IsCompleted)
                        {
                            var textEmpfangen = taskScannen.Result;

                            if (textEmpfangen == optCrad.TextBeiFehler)
                                JgLog.Set(null, $"{optCrad.Info} -> Fehlertext angesprochen.", JgLog.LogArt.Warnung);
                            else if (textEmpfangen.Length == 1)
                                JgLog.Set(null, $"{optCrad.Info} -> Ein Zeichen Empfangen: {Convert.ToByte(textEmpfangen[0])}", JgLog.LogArt.Warnung);
                            else if (textEmpfangen.Length < 1)
                                JgLog.Set(null, $"{optCrad.Info} -> Leeres Zeichen Empfangen!", JgLog.LogArt.Warnung);                   
                            else
                            {
                                if (textEmpfangen.Contains(optCrad.TextVerbinungOk))
                                    JgLog.Set(null, $"Verbindung Craddle OK ! {textEmpfangen}", JgLog.LogArt.Info);
                                else
                                {
                                    var ergScanner = auswertScanner.TextEmpfangen(taskScannen.Result);
                                    netStream.Write(ergScanner.AusgabeAufCraddle, 0, ergScanner.AusgabeAufCraddle.Length);
                                }
                                continue;
                            }
                        }
                        try
                        {
                            JgLog.Set(null, $"Abbruch {optCrad.Info}!", JgLog.LogArt.Warnung);

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
