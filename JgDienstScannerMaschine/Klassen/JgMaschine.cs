using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        internal DatenTaskMaschine _DatenTask;

        public JgMaschineStamm()
        {
            _DatenTask = new DatenTaskMaschine() { Maschine = this };
        }

        public void DatenZurMaschine(string BvBsCode)
        {
            JgLog.Set(this, $"Sende Daten an {this.MaschineName}", JgLog.LogArt.Info);
            SendeDatenZurMaschine(BvBsCode);
        }

        internal abstract void SendeDatenZurMaschine(string BvBsCode);

        internal abstract void EinlesenZeitenAusMaschine();

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
        public JgMaschineProgress()
        {
            _DatenTask.PfadProduktionsListe = Properties.Settings.Default.ProgressPfadProduktionsListe;
        }

        internal override void SendeDatenZurMaschine(string BvBsCode)
        {
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
                    JgLog.Set(this, $"Fehler beim schreiben der Progress Produktionsliste!\nDatei: {datei}.\nGrund: {ex.Message}", JgLog.LogArt.Fehler);
                }
            }, BvBsCode);
        }

        private void FehlerAusloesen(string FeldName, int NummerZeile, string Zeile, int FeldIndext, string Wert, Exception Fehler)
        {
            var msg = $"Fehler bei Konvertierung der Importdaten Feld '{FeldName} in Zeile {NummerZeile}."
              + $"\nIndex: {FeldIndext} Wert: {Wert}\nDatensatz: {Zeile}\nFehler: {Fehler.Message}";
            throw new Exception(msg);
        }

        internal override void EinlesenZeitenAusMaschine()
        {
            //int zaehler = 0;
            //string msg = "";
            //_Ergebnisse.Clear();

            //var lZeilen = StringListenLaden(DateiName);

            //if (lZeilen != null)
            //{
            //    foreach (var zeile in lZeilen)
            //    {
            //        zaehler++;
            //        var felder = zeile.Split(new char[] { ';' }, StringSplitOptions.None);
            //        var erg = new ErgebnisAbfrage();

            //        try
            //        {
            //            erg.Start = Convert.ToDateTime(felder[6]);
            //        }
            //        catch (Exception f)
            //        {
            //            FehlerAusloesen("DatumStart", zaehler, zeile, 6, felder[6], f);
            //        }

            //        try
            //        {
            //            erg.Dauer = Convert.ToDateTime(felder[7]) - (DateTime)erg.Start;
            //        }
            //        catch (Exception f)
            //        {
            //            FehlerAusloesen("DatumEnde", zaehler, zeile, 7, felder[7], f);
            //        }

            //        try
            //        {
            //            erg.Schluessel = felder[2];
            //        }
            //        catch (Exception f)
            //        {
            //            FehlerAusloesen("NummerPosition", zaehler, zeile, 2, felder[2], f);
            //        }

            //        _Ergebnisse.Add(erg);
            //    }

            //    ErgebnissInDatenbank(Maschine, _Ergebnisse);

            //    try
            //    {
            //        File.Delete(DateiName);
            //    }
            //    catch (Exception f)
            //    {
            //        msg = $"Datei {DateiName} konnte nicht gelöscht werden !\nGrund: {f.Message}";
            //        throw new Exception(msg);
            //    }
            //}

        }
    }

    public class JgMaschineEvg : JgMaschineStamm
    {
        public JgMaschineEvg()
        {
            _DatenTask.PfadProduktionsListe = Properties.Settings.Default.EvgPfadProduktionsListe;
            _DatenTask.DateiProduktionsAuftrag = Properties.Settings.Default.EvgDateiProduktionsAuftrag;
        }

        internal override void SendeDatenZurMaschine(string BvBsCode)
        {
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

        internal override void EinlesenZeitenAusMaschine()
        {
            var pfadStart = @"\\" + this.MaschineIp + @"\" + "Muss noch eingessellt werden";

            if (! Directory.Exists(pfadStart))
            {
                JgLog.Set(this, "Pad zum Auslesen der Daten nicht gefunden", JgLog.LogArt.Fehler);

            }
            else
            {
                // alle relevante Dateien aus Verzeichnissen laden -> Stammverzeichnis ...\EVG\Eingabe\Monit\(Jahr)\(HahrMonat)\....

                var heute = DateTime.Now.Date;
                var durchlauf = this.ListeBauteile.Min(m => m.Erstellt);
                var dateienAuswertung = new List<string>();

                while (durchlauf <= heute)
                {
                    var dirMonatJahr = string.Format(@"{0}\{1}\{1}{2}", pfadStart, durchlauf.Year, durchlauf.Month.ToString("D2"));
                    dateienAuswertung.AddRange(Directory.EnumerateFiles(dirMonatJahr, "F_*.mon").ToList());
                    durchlauf.AddMonths(1);
                };

                //var merkeLetzteDatum = Maschine.eProtokoll.LetzteDateiDatum;
                //var merkeLetzteZeile = Maschine.eProtokoll.LetzteZeile;
                //var merkeZeileBauteil = 0;

                //foreach (var datei in dateienAuswertung)
                //{
                //    var datum = Helper.DatumAusYyyyMMdd(Path.GetFileName(datei).Substring(2));

                //    if (datum < Maschine.eProtokoll.LetzteDateiDatum)
                //        continue;

                //    var lZeilen = StringListenLaden(datei);

                //    if (datum > Maschine.eProtokoll.LetzteDateiDatum)
                //        Maschine.eProtokoll.LetzteDateiDatum = datum;

                //    ErgebnisAbfrage ergNeu = null;
                //    var zeileStart = 0;

                //    if (datum == merkeLetzteDatum)
                //        zeileStart = merkeLetzteZeile;

                //    for (int zeile = zeileStart; zeile < lZeilen.Length; zeile++)
                //    {
                //        if (lZeilen[zeile][7] == 'A')
                //        {
                //            merkeZeileBauteil = zeile;
                //            ergNeu = new ErgebnisAbfrage() { Start = GetDatum(datum, lZeilen[zeile]) };
                //            _Ergebnisse.Add(ergNeu);

                //            var felder = lZeilen[zeile].Split(new char[] { ';' }, StringSplitOptions.None);
                //            try
                //            {
                //                ergNeu.Schluessel = felder[4];
                //            }
                //            catch (Exception f)
                //            {
                //                var msg = $"Fehler beim konvertieren der Id {felder[4]} in Zeile: {zeile}.\nGrund: {f.Message}";
                //                throw new Exception(msg);
                //            }
                //        }
                //        else if ((ergNeu != null) && (lZeilen[zeile][7] == 'D'))
                //            ergNeu.Dauer = (DateTime)ergNeu.Start - GetDatum(datum, lZeilen[zeile]);
                //    }

                //    if (datum == Maschine.eProtokoll.LetzteDateiDatum)
                //        Maschine.eProtokoll.LetzteZeile = merkeZeileBauteil;
                //}

                //ErgebnissInDatenbank(Maschine, _Ergebnisse);
            }

        }
    }

    public class JgMaschineSchnell : JgMaschineStamm
    {
        public JgMaschineSchnell()
        { }

        internal override void SendeDatenZurMaschine(string BvBsCode)
        {
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

        internal override void EinlesenZeitenAusMaschine()
        { }
    }

    public class JgMaschineHand : JgMaschineStamm
    {
        internal override void SendeDatenZurMaschine(string BvBsCode)
        {
            JgLog.Set(this, $"Daten registriert.", JgLog.LogArt.Info);
        }

        internal override void EinlesenZeitenAusMaschine()
        { }
    }
}
