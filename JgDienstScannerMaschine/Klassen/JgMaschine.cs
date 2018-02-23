using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace JgDienstScannerMaschine
{
    public abstract class JgMaschineStamm : ServiceRef.JgWcfMaschine
    {
        // Schnittstelle IMerkeStatusMaschine  ************

        public JgMeldung MeldMeldung { get; set; }

        public JgMeldung MeldBediener { get; set; }

        public JgBauteil AktivBauteil { get; set; } = null;

        public List<JgMeldung> MeldListeHelfer { get; set; } = new List<JgMeldung>();

        public List<JgBauteilFertig> ListeBauteile { get; set; } = new List<JgBauteilFertig>();

        public string Information { get; set; }

        // Programme *********************

        public abstract void SendeDatenZurMaschine(string BvBsCode);

        public override string ToString()
        {
            return MaschineName;
        }
    }

    public class DatenTaskMaschine
    {
        public JgMaschineStamm Maschine;
        public string BvbsString;
        public string PfadProduktionsListe;
        public string DateiProduktionsAuftrag;
    }

    public class JgMaschineProgress : JgMaschineStamm
    {
        private DatenTaskMaschine _DatenTask;

        public JgMaschineProgress()
        {
            _DatenTask = new DatenTaskMaschine()
            {

                PfadProduktionsListe = Properties.Settings.Default.ProgressPfadProduktionsListe,
            };
        }

        public override void SendeDatenZurMaschine(string BvBsCode)
        {
            JgLog.Set(this, $"Sende Maschine Progress", JgLog.LogArt.Info);
            _DatenTask.BvbsString = BvBsCode;

            Task.Factory.StartNew((SendDaten) =>
            {
                var md = (DatenTaskMaschine)SendDaten;
                var datei = string.Format(@"\\{0}\{1}\{2}", md.Maschine.MaschineIp, md.PfadProduktionsListe, "Auftrag.txt");

                try
                {
                    File.WriteAllText(datei, BvBsCode, Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    JgLog.Set(this, $"Fehler beim schreiben der Progress Produktionsliste!\nDatei: {datei}.\nGrund: {ex.Message}", JgLog.LogArt.Warnung);
                }
            }, _DatenTask);
        }
    }

    public class JgMaschineEvg : JgMaschineStamm
    {
        private DatenTaskMaschine _DatenTask;

        public JgMaschineEvg()
        {
            _DatenTask = new DatenTaskMaschine()
            {
                Maschine = this,
                PfadProduktionsListe = Properties.Settings.Default.EvgPfadProduktionsListe,
                DateiProduktionsAuftrag = Properties.Settings.Default.EvgDateiProduktionsAuftrag
            };
        }

        public override void SendeDatenZurMaschine(string BvBsCode)
        {

            JgLog.Set(this, $"Sende Daten.", JgLog.LogArt.Info);

            Task.Factory.StartNew((SendDaten) =>
            {
                var md = (DatenTaskMaschine)SendDaten;

                var datAuftrag = "Auftrag1.txt";
                var datProdListe = string.Format(@"\\{0}\{1}\{2}", md.Maschine.MaschineIp, md.PfadProduktionsListe, datAuftrag);

                try
                {
                    File.WriteAllText(datProdListe, md.BvbsString);
                }
                catch (Exception f)
                {
                    JgLog.Set(this, $"Fehler beim schreiben der EVG Produktionsliste!\nDatei: {datProdListe}.\nGrund: {f.Message}", JgLog.LogArt.Warnung);
                }

                // Produktionsauftrag schreiben

                var datProtAuftrag = string.Format(@"\\{0}\{1}", md.Maschine.MaschineIp, md.DateiProduktionsAuftrag);
                try
                {
                    File.WriteAllText(datProtAuftrag, datAuftrag);
                }
                catch (Exception f)
                {
                    JgLog.Set(this, $"Fehler beim schreiben des EVG Produktionsauftrages!\nDatei: {datProtAuftrag}.\nGrund: {f.Message}", JgLog.LogArt.Warnung);
                }

            }, _DatenTask);
        }
    }

    public class JgMaschineSchnell : JgMaschineStamm
    {
        private DatenTaskMaschine _DatenTask;

        public JgMaschineSchnell()
        {
            _DatenTask = new DatenTaskMaschine()
            {
                Maschine = this
            };
        }

        public override void SendeDatenZurMaschine(string BvBsCode)
        {
            JgLog.Set(this, $"Sende Daten.", JgLog.LogArt.Info);

            if (this.MaschinePort == 0)
                JgLog.Set(this, $"Bei Maschine wurde keine Portnummer eingetragen!", JgLog.LogArt.Krittisch);
            else
            {

                Task.Factory.StartNew((SendDaten) =>
                {
                    var md = (DatenTaskMaschine)SendDaten;

                    try
                    {
                        using (var client = new TcpClient(md.Maschine.MaschineIp, md.Maschine.MaschinePort))
                        {
                            client.NoDelay = true;
                            client.SendTimeout = 1000;
                            client.ReceiveTimeout = 1000;

                            var nwStr = client.GetStream();
                            var buffer = Encoding.ASCII.GetBytes(md.BvbsString + Convert.ToChar(13) + Convert.ToChar(10));
                            nwStr.Write(buffer, 0, buffer.Length);

                            // Auf Antwort von Maschine warten 

                            try
                            {
                                buffer = new byte[client.ReceiveBufferSize];
                                int anzEmpfang = nwStr.Read(buffer, 0, (int)client.ReceiveBufferSize);
                                var empfangen = Encoding.ASCII.GetString(buffer, 0, anzEmpfang);
                                JgLog.Set(this, $"Rückantwort von Maschine {md.Maschine.MaschineName}: {empfangen}", JgLog.LogArt.Unbedeutend);

                                #region Antwort der Maschine auf Fehler prüfen

                                //if ((empfangen.Length >= 3) && (empfangen[0] == Convert.ToChar(15)))
                                //{
                                //    var dat = JgMaschineLib.Helper.StartVerzeichnis() + @"FehlerCode\JgMaschineFehlerSchnell.txt";
                                //    if (!File.Exists(dat))
                                //    {
                                //        msg = $"Fehlerdatei: {dat} für Maschine 'Schnell' existiert nicht.";
                                //        Logger.Write(msg, "Service", 1, 0, System.Diagnostics.TraceEventType.Warning);
                                //    }
                                //    else
                                //    {
                                //        try
                                //        {
                                //            string nummer = empfangen.Substring(1, 2);

                                //            var zeilen = File.ReadAllLines(dat);
                                //            foreach (var zeile in zeilen)
                                //            {
                                //                if (zeile.Substring(0, 2) == nummer)
                                //                {
                                //                    msg = $"Fehlermeldung von Maschine {md.Maschine.MaschinenName} ist {zeile}!";
                                //                    Logger.Write(msg, "Service", 1, 0, System.Diagnostics.TraceEventType.Verbose);
                                //                    break;
                                //                }
                                //            }
                                //        }
                                //        catch (Exception f)
                                //        {
                                //            msg = $"Fehler beim auslesen der Fehlerdatei Firma Schnell.\nGrund: {f.Message}";
                                //            Logger.Write(msg, "Service", 1, 0, System.Diagnostics.TraceEventType.Information);
                                //        }
                                //    }
                                //}

                                #endregion

                            }
                            catch (Exception f)
                            {
                                throw new Exception($"Fehler beim warten auf Antwort von Maschine ! {f.Message}", f);
                            }

                            client.Close();
                            JgLog.Set(this, $"Verbindung abgeschlossen.", JgLog.LogArt.Unbedeutend);
                        }
                    }
                    catch (Exception f)
                    {
                        JgLog.Set(this, $"Fehler beim senden der Bvbs Daten an die Maschine.\nDaten: {md.BvbsString}\nGrund: {f.Message}", JgLog.LogArt.Warnung);
                    }
                }, _DatenTask);
            }
        }
    }

    public class JgMaschineHand : JgMaschineStamm
    {
        public override void SendeDatenZurMaschine(string BvBsCode)
        {
            JgLog.Set(this, $"Sende Daten.", JgLog.LogArt.Info);
        }
    }
}
