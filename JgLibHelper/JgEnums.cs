

namespace JgLibHelper
{
    public enum MaschinenArten
    {
        Hand,
        Evg,
        Progress,
        Schnell
    }

    public enum ScannerVorgang
    {
        CRADDLEANMELDUNG,
        FEHLER,
        PROG,
        MITA,       // Mitarbeiter Anmeldung an Maschine  
        BF2D,
        BF3D,
        TEST,
        SCHALTER,
        VERBUNDEN
    }

    public enum ScannerMeldung
    {
        ANMELDUNG,
        ABMELDUNG,
        COILSTART,
        COIL_ENDE,
        REPASTART,
        REPA_ENDE,
        WARTSTART,
        WART_ENDE,
        SCHALTE_R,
        BAUT_ENDE,  // Wird bei Fertigstellung des Bauteils als Meldung an Server geschickt
        TES_____T,
        FEHLE___R
    }

    public enum StatusMeldung
    {
        Offen,
        InBearbeitung,
        Erledigt
    }
}
