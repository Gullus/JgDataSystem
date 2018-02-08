

namespace JgLibHelper
{
    public enum MaschinenArten
    {
        Hand,
        Evg,
        Arsch
    }

    public enum StatusMaschine
    {
        Stillgelegt,
        Frei,
        InArbeit,
        InPause,
        InReparatur,
        InWartung,
        InCoilwechsel
    }

    public enum VorgangProgram
    {
        CRADDLEANMELDUNG,
        FEHLER,
        BAUTEIL,
        ANMELDUNG,
        ABMELDUNG,
        COILSTART,
        REPASTART,
        WARTSTART,
        REPA_ENDE,
        SCHALTER,
        VERBUNDEN,
        TEST
    }
}
