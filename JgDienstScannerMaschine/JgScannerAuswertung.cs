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
                JgLog.Set($"Sannernummer {TextEmpfangen} zu klein", JgLog.LogArt.Fehler);
            else
            {
                var maschine = _JgOpt.JgOpt.ListeMaschinen.FirstOrDefault(f => f.Value.NummerScanner == scanAusgabe.ScannerKennung).Value;
                if (maschine == null)
                {
                    JgLog.Set($"Maschine mit Scannernummer {scanAusgabe.ScannerKennung} nicht gefunden", JgLog.LogArt.Fehler);
                    scanAusgabe.Set(false, true, "Maschine nicht", "gefunden");
                }
                else if ((scanAusgabe.TextEmpfangen.Length == 16) && (scanAusgabe.TextEmpfangen[13] == 'S'))
                {
                    JgLog.Set($"Schalter getrückt: {scanAusgabe.ScannKoerper}", JgLog.LogArt.Info);
                    scanAusgabe.Set(false, false, $"Schalter {scanAusgabe.ScannKoerper}", "gedrückt");
                }
                else if (scanAusgabe.TextEmpfangen.Length < 17)
                {
                    JgLog.Set($"Scanner {scanAusgabe.TextEmpfangen}. Text zu kurz.", JgLog.LogArt.Fehler);
                    scanAusgabe.Set(false, true, $"Text zu kurz");
                }
                else
                {
                    if (scanAusgabe.VorgangScan != ScannerVorgang.FEHLER)
                    {
                        switch (scanAusgabe.VorgangScan)
                        {
                            case ScannerVorgang.BF2D:
                            case ScannerVorgang.TEST: scanAusgabe.Program = ScannerProgram.SCVORGANG; break;
                            case ScannerVorgang.SCHALTER: scanAusgabe.Program = ScannerProgram.SCHALTER; break;
                            case ScannerVorgang.MITA:
                                if (TextEmpfangen[17] == '0')
                                    scanAusgabe.Program = ScannerProgram.ANMELDUNG;
                                else
                                    scanAusgabe.Program = ScannerProgram.ABMELDUNG;
                                break;
                            case ScannerVorgang.PROG:
                                var scanProgrammText = (scanAusgabe.TextEmpfangen.Length < 26) ? null : scanAusgabe.TextEmpfangen.Substring(17, 9);
                                Enum.TryParse<ScannerProgram>(scanProgrammText, true, out scanAusgabe.Program);
                                break;
                        }

                        if ((scanAusgabe.VorgangScan != ScannerVorgang.MITA) && (maschine.Bediener == null))
                        {
                            JgLog.Set($"Maschine {maschine.MaschineName}. Kein Bediener angemeldet.", JgLog.LogArt.Info);
                            scanAusgabe.Set(false, true, "Kein Bediener", "angemeldet !");
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
                                        MaschineVorgangEintragen(scanAusgabe, maschine);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionPolicy.HandleException(ex, "Policy");
                            }
                        }
                    }
                }
            }

            return scanAusgabe;
        }

        // Programme zum Datenverarbeitung der Sacannerdaten **************************************************************

        private void MaschineBvbsEintragen(JgScannerAusgabe ScanAusgabe, JgMaschineStamm Maschine)
        {
            BvbsDaten btNeu = null;

            try
            {
                btNeu = new BvbsDaten(ScanAusgabe.BvbsString, true);
            }
            catch (Exception f)
            {
                JgLog.Set($"Bvbs Code konnte icht gelesen Werden.\nGrund: {f.Message}", JgLog.LogArt.Info);
                ScanAusgabe.Set(false, true, "Fehler im BVBS", "Code");
                return;
            }

            //todo: Aktives Bausteil von String in eindeute Id aus JgDate umwanden

            if (Maschine.AktivBauteil != null)
            {
                Maschine.AktivBauteil.EndeFertigung = DateTime.Now;

                if (ScanAusgabe.BvbsString == Maschine.AktivBauteil.IdBauteilJgData)
                {
                    JgLog.Set($"Bauteil Maschine: {Maschine.MaschineName}\nBauteilt: {Maschine.AktivBauteil.Id} erledigt", JgLog.LogArt.Unbedeutend);
                    ScanAusgabe.Set(false, false, "Bauteil erledigt");
                    return;
                }
            }

            Maschine.AktivBauteil = new JgMaschineBauteil
            {
                StartFertigung = DateTime.Now,

                IdMaschine = Maschine.Id,
                IdBauteilJgData = ScanAusgabe.BvbsString,

                GewichtInKg = btNeu.Gewicht ?? -1.0,
                DuchmesserInMm = btNeu.Durchmesser ?? -1,
                LaengeInCm = btNeu.Laenge ?? -1,
                AnzahlBiegungen = btNeu.ListeGeometrie.Count(s => s.Geometrie == BvbsGeometrie.Koerper.Bogen)
            };

            _JgOpt.QueueSend("BT", Maschine.AktivBauteil);
        }

        private void MaschineAnmeldungEintragen(JgScannerAusgabe ScanAusgabe, JgMaschineStamm Maschine)
        {
            var bediener = _JgOpt.JgOpt.ListeBediener[Guid.Parse(ScanAusgabe.ScannKoerper)];

            if (bediener == null)
            {
                JgLog.Set($"Bediener ({ScanAusgabe.ScannKoerper}) nicht in Datenbank !", JgLog.LogArt.Info);
                ScanAusgabe.Set(false, true, "Bediener", ScanAusgabe.ScannKoerper, "nicht in Datenbank");
            }
            else
            {
                if ((ScanAusgabe.Program == ScannerProgram.ANMELDUNG) && (Maschine.Bediener != null))
                {
                    if (bediener.Id == Maschine.Bediener)
                    {
                        ScanAusgabe.Set(false, true, "Bediener", bediener.BedienerName, "bereits angemeldet!");
                        JgLog.Set($"Bediener {bediener.BedienerName} bereitd angemeldet!", JgLog.LogArt.Unbedeutend);
                        return;
                    }
                    else if (Maschine.ListeHelfer.Contains(bediener.Id))
                    {
                        ScanAusgabe.Set(false, true, "Helfer", bediener.BedienerName, "bereits angemeldet!");
                        JgLog.Set($"Helfer {bediener.BedienerName} bereits angemeldet!", JgLog.LogArt.Unbedeutend);
                        return;
                    }
                }

                // In anderen Maschinen nach einer Anmeldung suchen

                foreach (var ma in _JgOpt.JgOpt.ListeMaschinen.Values)
                {

                    // Bei der Anmeldung den aktuellen Datensatz ausschließen
                    if ((ScanAusgabe.Program == ScannerProgram.ANMELDUNG) && (ma.Id == Maschine.Id))
                        continue;

                    if (ma.Bediener == bediener.Id)
                    {
                        ma.Bediener = null;
                        var prog = new JgMaschineProgramm(ma.Id, bediener.Id, ScannerProgram.ABMELDUNG);
                        _JgOpt.QueueSend($"Bediener {bediener.BedienerName} von Maschine {ma.MaschineName} abgemeldet", prog);

                        if (ScanAusgabe.Program == ScannerProgram.ABMELDUNG)
                            ScanAusgabe.Set(false, false, "Bediener", bediener.BedienerName, "u. Helfer abgemeldet!");

                        JgLog.Set($"Bediener {bediener.BedienerName} von Maschine {ma.MaschineName}  abgemeldet!", JgLog.LogArt.Unbedeutend);

                        foreach (var guidHelfer in Maschine.ListeHelfer)
                        {
                            prog = new JgMaschineProgramm(ma.Id, guidHelfer, ScannerProgram.ABMELDUNG);
                            JgLog.Set($"Helfer {bediener.BedienerName} von Maschine {ma.MaschineName} abgemeldet!", JgLog.LogArt.Unbedeutend);
                            _JgOpt.QueueSend($"Helder {bediener.BedienerName} abgemeldet", prog);
                        }

                        Maschine.ListeHelfer.Clear();
                    }
                    else
                    {
                        if (ma.ListeHelfer.Contains(bediener.Id))
                        {
                            foreach (var helf in ma.ListeHelfer)
                            {
                                var prog = new JgMaschineProgramm(ma.Id, bediener.Id, ScannerProgram.ABMELDUNG);
                                _JgOpt.QueueSend($"Helder {bediener.BedienerName} von Maschine {ma.MaschineName} abgemeldet", prog);

                                if (ScanAusgabe.Program == ScannerProgram.ABMELDUNG)
                                    ScanAusgabe.Set(false, false, "Helfer", bediener.BedienerName, "abgemeldet!");


                                Maschine.ListeHelfer.Remove(bediener.Id);
                                JgLog.Set($"Helfer {bediener.BedienerName} abgemeldet!", JgLog.LogArt.Unbedeutend);
                            }
                        }
                    }
                }

                // Anmeldung eintragen

                if (ScanAusgabe.Program == ScannerProgram.ANMELDUNG)
                {
                    if ((Maschine.Bediener == null) || (Maschine.Bediener != bediener.Id))
                    {
                        var prog = new JgMaschineProgramm(Maschine.Id, bediener.Id, ScannerProgram.ANMELDUNG);
                        _JgOpt.QueueSend($"Bediener {bediener.BedienerName} angemeldet", prog);
                        ScanAusgabe.Set(false, false, "Bediener", bediener.BedienerName, "angemeldet!");
                        JgLog.Set($"Bediener {bediener.BedienerName} angemeldet!", JgLog.LogArt.Unbedeutend);
                    }
                    else
                    {
                        var prog = new JgMaschineProgramm(Maschine.Id, bediener.Id, ScannerProgram.ANMELDUNG);
                        _JgOpt.QueueSend($"Helfer {bediener.BedienerName} angemeldet", prog);
                        ScanAusgabe.Set(false, false, "Helfer", bediener.BedienerName, "angemeldet!");
                        Maschine.ListeHelfer.Add(bediener.Id);
                        JgLog.Set($"Bediener {bediener.BedienerName} angemeldet!", JgLog.LogArt.Unbedeutend);
                    }
                }
            }
        }

        private void MaschineVorgangEintragen(JgScannerAusgabe ScanAusgabe, JgMaschineStamm Maschine)
        {

            // Bendigung von Coilwechsel, Wartung und Reparatur mit "Rep_Ende"

            void ProgrammBeenden(bool AufScannerAnzeigen = false)
            {
                var progText = "";
                var prog = new JgMaschineProgramm(Maschine.Id, ScannerProgram.SCVORGANG);

                switch (Maschine.ProdStatus)
                {
                    case StatusProduktion.InReparatur:
                        progText = "Reparatur";
                        prog.Program = ScannerProgram.REPA_ENDE;
                        break;
                    case StatusProduktion.InCoilwechsel:
                        progText = "Coilwechsel";
                        prog.Program = ScannerProgram.COIL_ENDE;
                        break;
                    case StatusProduktion.InWartung:
                        progText = "Wartung";
                        prog.Program = ScannerProgram.WART_ENDE;
                        break;
                }

                if (AufScannerAnzeigen)
                    ScanAusgabe.Set(false, false, "", progText, "abgeschlossen");

                Maschine.ProdStatus = StatusProduktion.InArbeit;
                _JgOpt.QueueSend($"Programm {progText} beendet", prog);

                JgLog.Set($"Programm {progText} auf Maschine {Maschine.MaschineName} beendet!", JgLog.LogArt.Info);
            }


            switch (ScanAusgabe.Program)
            {
                case ScannerProgram.COILSTART:
                    if (Maschine.ProdStatus == StatusProduktion.InCoilwechsel)
                        ScanAusgabe.Set(false, true, "Coilwechsle bereits", "begonnen");
                    else
                    {
                        ProgrammBeenden(false);
                        var prog = new JgMaschineProgramm(Maschine.Id, ScannerProgram.COILSTART);

                        try
                        {
                            prog.Anzahl = Convert.ToInt32(ScanAusgabe.ScannKoerper);
                        }
                        catch { }

                        Maschine.ProdStatus = StatusProduktion.InCoilwechsel;
                        ScanAusgabe.Set(false, false, "", "Coilwechsel", "gestartet");
                        _JgOpt.QueueSend($"Coilwechsel gestartet", prog);

                        JgLog.Set($"Coilwechsel Maschine {Maschine.MaschineName}. Anzahl: {prog.Anzahl ?? -1}", JgLog.LogArt.Info);
                    }
                    break;
                case ScannerProgram.REPASTART:
                    if (Maschine.ProdStatus == StatusProduktion.InReparatur)
                        ScanAusgabe.Set(false, true, "Reparatur bereits", "begonnen");
                    else
                    {
                        ProgrammBeenden(false);
                        var prog = new JgMaschineProgramm(Maschine.Id, ScannerProgram.REPASTART);

                        Maschine.ProdStatus = StatusProduktion.InReparatur;
                        ScanAusgabe.Set(false, false, "", "Reparatur", "gestartet");
                        _JgOpt.QueueSend($"Reparatur gestartet", prog);

                        JgLog.Set($"Reparatur Maschine {Maschine.MaschineName}.", JgLog.LogArt.Info);
                    }
                    break;
                case ScannerProgram.WARTSTART:
                    if (Maschine.ProdStatus == StatusProduktion.InWartung)
                        ScanAusgabe.Set(false, true, "Wartung bereits", "begonnen");
                    else
                    {
                        ProgrammBeenden(false);
                        var prog = new JgMaschineProgramm(Maschine.Id, ScannerProgram.WARTSTART);

                        Maschine.ProdStatus = StatusProduktion.InWartung;
                        ScanAusgabe.Set(false, false, "", "Wartung", "gestartet");
                        _JgOpt.QueueSend($"Wartung gestartet", prog);

                        JgLog.Set($"Wartung Maschine {Maschine.MaschineName}.", JgLog.LogArt.Info);
                    }
                    break;
            }
        }
    }
}

