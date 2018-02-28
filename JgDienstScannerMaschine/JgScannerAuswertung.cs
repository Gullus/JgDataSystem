using JgLibHelper;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.Linq;

namespace JgDienstScannerMaschine
{
    public class JgScannerAuswertung
    {
        private JgOptionenCraddle _JgOpt;

        public JgScannerAuswertung(JgOptionenCraddle CraddelOptionen)
        {
            _JgOpt = CraddelOptionen;
        }

        public JgScannerAusgabe TextEmpfangen(string TextEmpfangen)
        {
            var scanAusgabe = new JgScannerAusgabe(TextEmpfangen);

            if (scanAusgabe.ScannerKennung == null)
                JgLog.Set(null, $"Sannernummer {TextEmpfangen} zu klein", JgLog.LogArt.Fehler);
            else
            {
                var maschine = _JgOpt.JgOpt.ListeMaschinen.FirstOrDefault(f => f.Value.NummerScanner == scanAusgabe.ScannerKennung).Value;
                if (maschine == null)
                {
                    JgLog.Set(null, $"Maschine mit Scannernummer {scanAusgabe.ScannerKennung} nicht gefunden", JgLog.LogArt.Fehler);
                    scanAusgabe.Set(false, true, "Maschine nicht", "gefunden");
                }
                else if ((scanAusgabe.TextEmpfangen.Length == 16) && (scanAusgabe.TextEmpfangen[13] == 'S'))
                {
                    JgLog.Set(maschine, $"Schalter getrückt: {scanAusgabe.ScannKoerper}", JgLog.LogArt.Info);
                    scanAusgabe.Set(false, false, $"Schalter {scanAusgabe.ScannKoerper}", "gedrückt");
                }
                else if (scanAusgabe.TextEmpfangen.Length < 17)
                {
                    JgLog.Set(maschine, $"Scanner {scanAusgabe.TextEmpfangen}. Text zu kurz.", JgLog.LogArt.Fehler);
                    scanAusgabe.Set(false, true, $"Text zu kurz");
                }
                else
                {
                    if (scanAusgabe.VorgangScan != ScannerVorgang.FEHLER)
                    {
                        switch (scanAusgabe.VorgangScan)
                        {
                            case ScannerVorgang.BF2D:
                            case ScannerVorgang.TEST: scanAusgabe.Meldung = ScannerMeldung.TES_____T; break;
                            case ScannerVorgang.SCHALTER: scanAusgabe.Meldung = ScannerMeldung.SCHALTE_R; break;
                            case ScannerVorgang.MITA:
                                if (TextEmpfangen[17] == '0')
                                    scanAusgabe.Meldung = ScannerMeldung.ANMELDUNG;
                                else
                                    scanAusgabe.Meldung = ScannerMeldung.ABMELDUNG;
                                break;
                            case ScannerVorgang.PROG:
                                var scanProgrammText = (scanAusgabe.TextEmpfangen.Length < 26) ? null : scanAusgabe.TextEmpfangen.Substring(17, 9);
                                Enum.TryParse<ScannerMeldung>(scanProgrammText, true, out scanAusgabe.Meldung);
                                break;
                        }

                        if ((scanAusgabe.VorgangScan != ScannerVorgang.MITA) && (maschine.MeldBediener == null))
                        {
                            JgLog.Set(maschine, "Kein Bediener angemeldet.", JgLog.LogArt.Info);
                            scanAusgabe.Set(false, true, "", "Kein Bediener", "angemeldet !");
                        }
                        else
                        {
                            try
                            {
                                scanAusgabe.ScannerMitDisplay = maschine.ScannerMitDisplay;

                                switch (scanAusgabe.VorgangScan)
                                {
                                    case ScannerVorgang.BF3D:
                                    case ScannerVorgang.BF2D:
                                        scanAusgabe.BvbsString = scanAusgabe.VorgangScan.ToString() + scanAusgabe.ScannKoerper;
                                        MaschineBvbsEintragen(scanAusgabe, maschine);
                                        if (scanAusgabe.MeldungAnMaschine)
                                            maschine.SendeDatenZurMaschine(scanAusgabe.BvbsString);
                                        break;
                                    case ScannerVorgang.MITA:
                                        MaschineAnmeldungEintragen(scanAusgabe, maschine);
                                        break;
                                    case ScannerVorgang.PROG:
                                        MaschineMeldungEintragen(scanAusgabe, maschine);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionPolicy.HandleException(ex, "Policy");
                                throw new Exception("Felhler in ScannerAusgabe", ex);
                            }
                        }
                    }
                }
            }

            return scanAusgabe;
        }

        // Programme zum Datenverarbeitung der Sacannerdaten **************************************************************

        private void BauteilBeendet(JgScannerAusgabe ScanAusgabe, JgMaschineStamm Maschine)
        {
            var meldung = new JgMeldung()
            {
                Id = Maschine.AktivBauteil.Id,
                IdBediener = Maschine.AktivBauteil.IdBediener,
                Meldung = ScannerMeldung.BAUT_ENDE,
                Aenderung = DateTime.Now
            };

            Maschine.ListeBauteile.Add(new JgBauteilFertig(meldung.Id , Maschine.AktivBauteil.IdBauteilJgData));
            Maschine.AktivBauteil = null;

            JgLog.Set(Maschine,  $"Bauteilt: {meldung.Id} erledigt", JgLog.LogArt.Unbedeutend);
            _JgOpt.QueueSend($"Bauteil {ScanAusgabe.ScannKoerper} fertig", Maschine, meldung);
            ScanAusgabe.Set(false, false, "", "Bauteil", "fertig!");
        }

        private void MaschineBvbsEintragen(JgScannerAusgabe ScanAusgabe, JgMaschineStamm Maschine)
        {
            if (Maschine.AktivBauteil != null)
            {
                var bauteilFertig = Maschine.AktivBauteil.IdBauteilJgData == ScanAusgabe.ScannKoerper;
                BauteilBeendet(ScanAusgabe, Maschine);
                if (bauteilFertig)
                    return;
            }

            BvbsDaten btNeu = null;

            try
            {
                btNeu = new BvbsDaten(ScanAusgabe.BvbsString, true);
            }
            catch (Exception f)
            {
                JgLog.Set(Maschine, $"Bvbs Code konnte icht gelesen Werden.\nGrund: {f.Message}", JgLog.LogArt.Info);
                ScanAusgabe.Set(false, true, "Fehler im BVBS", "Code");
                return;
            }

            //todo: Aktives Bausteil von String in eindeutige Id aus JgData umwandeln

            var btId = ScanAusgabe.ScannKoerper.Replace("\n", string.Empty).Replace("\r", string.Empty);

            if (Maschine.ListeBauteile.Any(a => a.IdJgData == btId))
            {
                ScanAusgabe.Set(true, true, "Bauteil wurde", "bereits", "erstellt !");
            }
            else
            {
                Maschine.AktivBauteil = new JgBauteil
                {

                    IdMaschine = Maschine.Id,
                    IdBediener = Maschine.MeldBediener.IdBediener,
                    AnzahlHelfer = Maschine.MeldListeHelfer.Count,

                    IdBauteilJgData = btId,

                    AnzahlTeile = btNeu.Anzahl ?? 0,
                    GewichtInKg = btNeu.Gewicht ?? 0,
                    DuchmesserInMm = btNeu.Durchmesser ?? 0,
                    LaengeInCm = btNeu.Laenge ?? 0,
                    AnzahlBiegungen = btNeu.ListeGeometrie.Count() - 1
                };

                JgLog.Set(Maschine, $"Bauteil {Maschine.AktivBauteil.Id} registriert.", JgLog.LogArt.Unbedeutend);
                _JgOpt.QueueSend($"BT {Maschine.AktivBauteil.Id}", Maschine.AktivBauteil);
                ScanAusgabe.Set(true, false, "Bauteil in", "Maschine", "registriert");
            }
        }

        private void MaschineAnmeldungEintragen(JgScannerAusgabe ScanAusgabe, JgMaschineStamm Maschine)
        {
            var bediener = _JgOpt.JgOpt.ListeBediener.FirstOrDefault(f => (f.Value.NummerAusweis == ScanAusgabe.ScannKoerper)).Value;

            if (bediener == null)
            {
                JgLog.Set(Maschine, $"Bediener ({ScanAusgabe.ScannKoerper}) nicht in Datenbank !", JgLog.LogArt.Info);
                ScanAusgabe.Set(false, true, "Bediener", ScanAusgabe.ScannKoerper, "nicht regisitriert");
            }
            else
            {
                if ((ScanAusgabe.Meldung == ScannerMeldung.ANMELDUNG) && (Maschine.MeldBediener != null))
                {
                    if (bediener.Id == Maschine.MeldBediener.IdBediener)
                    {
                        ScanAusgabe.Set(false, true, "Bediener", bediener.BedienerName, "bereits angemeldet!");
                        return;
                    }
                    else if (Maschine.MeldListeHelfer.Any(f => f.IdBediener == bediener.Id))
                    {
                        ScanAusgabe.Set(false, true, "Helfer", bediener.BedienerName, "bereits angemeldet!");
                        return;
                    }
                }

                // In allen Maschinen nach einer Anmeldung suchen und abmelden

                var abmeldungBediener = false;
                var abmeldungHelfer = false;

                foreach (var maAbmeldung in _JgOpt.JgOpt.ListeMaschinen.Values)
                {
                    // Wenn Bediener auf einer Maschine angemeldet ist

                    if (maAbmeldung?.MeldBediener?.IdBediener == bediener.Id)
                    {
                        abmeldungBediener = true;

                        var msg = $"Bediener {bediener.BedienerName} abgemeldet";
                        var meldung = maAbmeldung.MeldBediener.Abmeldung();
                        maAbmeldung.MeldBediener = null;

                        _JgOpt.QueueSend(msg, maAbmeldung, meldung);
                        JgLog.Set(Maschine, msg, JgLog.LogArt.Unbedeutend);

                        // Bei Bedienerabmeldung auch alle Helfer abmelden

                        foreach (var meldHelfer in maAbmeldung.MeldListeHelfer)
                        {
                            // Wenn Helfer in Liste Bediener vorhanden
                            if (_JgOpt.JgOpt.ListeBediener.ContainsKey(meldHelfer.IdBediener))
                            {
                                meldHelfer.Abmeldung();
                                msg = $"Helfer {_JgOpt.JgOpt.ListeBediener[meldHelfer.IdBediener].BedienerName} abgemeldet!";
                                _JgOpt.QueueSend(msg, maAbmeldung, meldHelfer);
                                JgLog.Set(Maschine, msg, JgLog.LogArt.Unbedeutend);
                            }
                        }

                        Maschine.MeldListeHelfer.Clear();
                    }
                    else
                    {
                        // Ob in einer Helferliste eingetragen

                        var lHelfer = maAbmeldung.MeldListeHelfer.Where(w => w.IdBediener == bediener.Id).ToList();

                        foreach (var meldHelfer in lHelfer)
                        {
                            abmeldungHelfer = true;

                            if (_JgOpt.JgOpt.ListeBediener.ContainsKey(meldHelfer.IdBediener))
                            {
                                var meldung = meldHelfer.Abmeldung();
                                maAbmeldung.MeldListeHelfer.Remove(meldHelfer);
                                _JgOpt.QueueSend($"Helfer {_JgOpt.JgOpt.ListeBediener[meldHelfer.IdBediener]} abgemeldet", maAbmeldung, meldung);
                                JgLog.Set(Maschine, $"Helfer {_JgOpt.JgOpt.ListeBediener[meldHelfer.IdBediener].BedienerName} abgemeldet!", JgLog.LogArt.Unbedeutend);
                            }
                        }
                    }
                }

                if (ScanAusgabe.Meldung == ScannerMeldung.ABMELDUNG)
                {
                    if (abmeldungBediener)
                        ScanAusgabe.Set(false, false, "", "Bediener", bediener.BedienerName, "abgemeldet!");
                    else if (abmeldungHelfer)
                        ScanAusgabe.Set(false, false, "", "Helfer", bediener.BedienerName, "abgemeldet!");
                    else
                        ScanAusgabe.Set(false, true, "Keine Anmeldung", bediener.BedienerName, "registriert!");
                }
                else   // Anmeldung eintragen
                {
                    if (Maschine.MeldBediener == null)
                    {
                        Maschine.MeldBediener = new JgMeldung(bediener.Id, ScannerMeldung.ANMELDUNG);

                        _JgOpt.QueueSend($"Bediener {bediener.BedienerName} angemeldet", Maschine, Maschine.MeldBediener);
                        ScanAusgabe.Set(false, false, "", "Bediener", bediener.BedienerName, "angemeldet!");
                        JgLog.Set(Maschine, $"Bediener {bediener.BedienerName} angemeldet!", JgLog.LogArt.Unbedeutend);
                    }
                    else // Wenn ein Bediener angemeldet, wird ein Helfer angemeldet
                    {
                        var meld = new JgMeldung(bediener.Id, ScannerMeldung.ANMELDUNG);
                        Maschine.MeldListeHelfer.Add(meld);

                        _JgOpt.QueueSend($"Helfer {bediener.BedienerName} angemeldet", Maschine, meld);
                        ScanAusgabe.Set(false, false, "", "Helfer", bediener.BedienerName, "angemeldet!");
                        JgLog.Set(Maschine, $"Helfer {bediener.BedienerName} angemeldet!", JgLog.LogArt.Unbedeutend);
                    }
                }
            }
        }

        private string[] MeldungBeenden(JgMaschineStamm Maschine)
        {
            if (Maschine.MeldMeldung == null)
                return null;

            var progText = "Reparatur";
            switch (Maschine.MeldMeldung.Meldung)
            {
                case ScannerMeldung.COILSTART:
                    progText = "Coilwechsel";
                    break;
                case ScannerMeldung.WARTSTART:
                    progText = "Wartung";
                    break;
            }

            var meldung = Maschine.MeldMeldung.Abmeldung();
            Maschine.MeldMeldung = null;
            JgLog.Set(Maschine, $"Meldung {progText} beendet!", JgLog.LogArt.Info);
            _JgOpt.QueueSend($"Meldung {progText} beendet", Maschine, meldung);

            return new string[] { "", progText, "beendet" };
        }

        private void MaschineMeldungEintragen(JgScannerAusgabe ScanAusgabe, JgMaschineStamm Maschine)
        {
            // *** Unterprogramm Bendigung von Coilwechsel, Wartung und Reparatur mit "Rep_Ende"

            switch (ScanAusgabe.Meldung)
            {
                case ScannerMeldung.COILSTART:
                    if (Maschine?.MeldMeldung?.Meldung == ScannerMeldung.COILSTART)
                        ScanAusgabe.Set(false, true, "Coilwechsle bereits", "begonnen");
                    else
                    {
                        MeldungBeenden(Maschine);
                        Maschine.MeldMeldung = new JgMeldung(Maschine.MeldBediener.IdBediener, ScannerMeldung.COILSTART);

                        try
                        {
                            Maschine.MeldMeldung.Anzahl = Convert.ToInt32(ScanAusgabe.ScannKoerper);
                        }
                        catch { }

                        JgLog.Set(Maschine, $"Start Coilwechsel. Anzahl: {Maschine.MeldMeldung.Anzahl ?? 0}", JgLog.LogArt.Info);
                        ScanAusgabe.Set(false, false, "", "Coilwechsel", "gestartet");
                        _JgOpt.QueueSend($"Start Coilwechsel", Maschine, Maschine.MeldMeldung);
                    }
                    break;
                case ScannerMeldung.REPASTART:
                    if (Maschine?.MeldMeldung?.Meldung == ScannerMeldung.REPASTART)
                        ScanAusgabe.Set(false, true, "Reparatur bereits", "begonnen");
                    else
                    {
                        MeldungBeenden(Maschine);
                        Maschine.MeldMeldung = new JgMeldung(Maschine.MeldBediener.IdBediener, ScannerMeldung.REPASTART);

                        JgLog.Set(Maschine, "Start Reparatur.", JgLog.LogArt.Info);
                        ScanAusgabe.Set(false, false, "", "Reparatur", "gestartet");
                        _JgOpt.QueueSend($"Start Reparatur", Maschine, Maschine.MeldMeldung);
                    }
                    break;
                case ScannerMeldung.WARTSTART:
                    if (Maschine?.MeldMeldung?.Meldung == ScannerMeldung.WARTSTART)
                        ScanAusgabe.Set(false, true, "Wartung bereits", "begonnen");
                    else
                    {
                        MeldungBeenden(Maschine);
                        Maschine.MeldMeldung = new JgMeldung(Maschine.MeldBediener.IdBediener, ScannerMeldung.WARTSTART);

                        JgLog.Set(Maschine, $"Start Wartung.", JgLog.LogArt.Info);
                        ScanAusgabe.Set(false, false, "", "Wartung", "gestartet");
                        _JgOpt.QueueSend($"Start Wartung", Maschine, Maschine.MeldMeldung);
                    }
                    break;
                case ScannerMeldung.REPA_ENDE:
                    if (Maschine.MeldMeldung == null)
                        ScanAusgabe.Set(false, true, "", "Keine Meldung", "registriert");
                    else
                        ScanAusgabe.Set(false, false, MeldungBeenden(Maschine));
                    break;
            }
        }
    }
}

