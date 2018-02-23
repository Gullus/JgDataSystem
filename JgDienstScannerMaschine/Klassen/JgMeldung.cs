using JgLibHelper;
using System;

namespace JgDienstScannerMaschine
{
    public class JgMeldung : IJgMeldung
    {
        #region Schnittstelle

        public Guid Id { get; set; }
        public ScannerMeldung Meldung { get; set; }
        public int? Anzahl { get; set; }
        public Guid IdBediener { get; set; }
        public DateTime Aenderung { get; set; }

        #endregion

        public JgMeldung()
        { }

        public JgMeldung(Guid MyIdBediener, ScannerMeldung MyMeldung, int? MyAnzahl = null)
        {
            Id = Guid.NewGuid();
            Aenderung = DateTime.Now;
            Meldung = MyMeldung;
            Anzahl = MyAnzahl;
            IdBediener = MyIdBediener;
        }

        public JgMeldung Abmeldung()
        {
            Aenderung = DateTime.Now;

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

            return this;
        }
    }
}
