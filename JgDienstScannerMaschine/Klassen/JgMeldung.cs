using JgLibHelper;
using System;

namespace JgDienstScannerMaschine
{
    public class JgMeldung : ServiceRef.JgWcfMeldung
    {
        public JgMeldung()
        { }

        public JgMeldung(Guid MyIdMaschine,  Guid MyIdBediener, ScannerMeldung MyMeldung, int? MyAnzahl = null)
        {
            Id = Guid.NewGuid();
            ZeitMeldung = DateTime.Now;
            Aenderung = DateTime.Now;

            IdMaschine = MyIdMaschine;
            IdBediener = MyIdBediener;
            Meldung = MyMeldung;
            Anzahl = MyAnzahl;
        }

        public void Abmeldung()
        {
            ZeitMeldung = DateTime.Now;

            switch (Meldung)
            {
                case ScannerMeldung.ANMELDUNG:
                    Meldung = ScannerMeldung.ABMELDUNG;
                    break;
                case ScannerMeldung.COILSTART:
                    Meldung = ScannerMeldung.COIL_ENDE;
                    break;
                case ScannerMeldung.REPASTART:
                    Meldung = ScannerMeldung.REPA_ENDE;
                    break;
                case ScannerMeldung.WARTSTART:
                    Meldung = ScannerMeldung.WART_ENDE;
                    break;
            }
        }
    }
}
