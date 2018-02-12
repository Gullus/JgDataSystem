using JgLibHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace JgDienstScannerMaschine
{
    public abstract class JgMaschineStamm : JgBaseClass, IJgMaschine, IJgMaschineStatus
    {
        public string MaschineName { get; set; }
        public MaschinenArten MaschineArt { get; set; } = MaschinenArten.Hand;

        public string MaschineIp { get; set; } = "";
        public int MaschinePort { get; set; } = 5100;

        public string NummerScanner { get; set; } = "";
        public bool SammelScannung { get; set; } = false;
        public bool ScannerMitDisplay { get; set; } = true;

        public int VorschubProMeterInSek { get; set; } = 0;
        public int ZeitProBiegungInSek { get; set; } = 0;
        public int ZeitProBauteilInSek { get; set; } = 0;

        public string Bemerkung { get; set; }
        public Guid? Bediener { get; set; } = null;
        public List<Guid> ListeHelfer { get; set; } = new List<Guid>();

        public List<Guid> ListeBauteile { get; set; } = new List<Guid>();
        public StatusProduktion ProdStatus { get; set; }

        // Ohne Schnittstelle ************

        public JgMaschineBauteil AktivBauteil { get; set; } = null;

        public List<JgBediener> GetHelfer(Dictionary<Guid, JgBediener> DicBediener)
        {
            return DicBediener.Where(w => ListeHelfer.Contains(w.Key)).Select(s => s.Value).ToList();
        }

        public abstract void SendeDatenZurMaschine(string BvBsCode);

        public override string ToString()
        {
            return MaschineName;
        }
    }

    class DatenTaskMaschine
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
            JgLog.Set($"Sende Maschine Progress", JgLog.LogArt.Info);
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
                    JgLog.Set($"Fehler beim schreiben der Progress Produktionsliste Maschine: {MaschineName} \nDatei: {datei}.\nGrund: {ex.Message}", JgLog.LogArt.Warnung);
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
            JgLog.Set($"Sende Maschine Evg", JgLog.LogArt.Info);

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
                    JgLog.Set($"Fehler beim schreiben der EVG Produktionsliste in die Maschine {md.Maschine.MaschineName}!\nDatei: {datProdListe}.\nGrund: {f.Message}", JgLog.LogArt.Warnung);
                }

                // Produktionsauftrag schreiben

                var datProtAuftrag = string.Format(@"\\{0}\{1}", md.Maschine.MaschineIp, md.DateiProduktionsAuftrag);
                try
                {
                    File.WriteAllText(datProtAuftrag, datAuftrag);
                }
                catch (Exception f)
                {
                    JgLog.Set($"Fehler beim schreiben des EVG Produktionsauftrages in die Maschine {md.Maschine.MaschineName}!\nDatei: {datProtAuftrag}.\nGrund: {f.Message}", JgLog.LogArt.Warnung);
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
            JgLog.Set($"Sende Maschine Schnell", JgLog.LogArt.Info);

            if (this.MaschinePort == 0)
                JgLog.Set($"Bei Maschine: {MaschineName} wurde keine Portnummer eingetragen!", JgLog.LogArt.Krittisch);
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
                                JgLog.Set($"Rückantwort von Maschine {md.Maschine.MaschineName}: {empfangen}", JgLog.LogArt.Unbedeutend);

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
                            JgLog.Set($"Verbindung mit: {md.Maschine.MaschineName} abgeschlossen.", JgLog.LogArt.Unbedeutend);
                        }
                    }
                    catch (Exception f)
                    {
                        JgLog.Set($"Fehler beim senden der Bvbs Daten an die Maschine: {md.Maschine.MaschineName}\nDaten: {md.BvbsString}\nGrund: {f.Message}", JgLog.LogArt.Warnung);
                    }
                }, _DatenTask);
            }
        }
    }

    public class JgMaschineHand : JgMaschineStamm
    {
        public override void SendeDatenZurMaschine(string BvBsCode)
        {
            JgLog.Set($"Sende Maschine Hand", JgLog.LogArt.Info);
        }
    }
}
