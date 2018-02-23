using System;
using System.Collections.Generic;

namespace JgLibHelper
{
    public interface IJgBase
    {
        Guid Id { get; set; }
        DateTime Aenderung { get; set; }
    }

    public interface IJgStandort : IJgBase
    {
        string StandortName { get; set; }
    }       

    public interface IJgBediener : IJgBase
    {
        string Vorname { get; set; }
        string Nachname { get; set; }

        string NummerAusweis { get; set; }
    }

    // Ende der Fertigung durch eine Meldung !!!

    public interface IJgBauteil : IJgBase
    {
        int AnzahlTeile { get; set; }
        int DuchmesserInMm { get; set; }
        double GewichtInKg { get; set; }
        int LaengeInCm { get; set; }
        int AnzahlBiegungen { get; set; }

        // Id Bauteail aus JgData
        string IdBauteilJgData { get; set; }

        Guid IdMaschine { get; set; }
        Guid IdBediener { get; set; }
        int AnzahlHelfer { get; set; }
    }

    public interface IJgMeldung : IJgBase
    {
        ScannerMeldung Meldung { get; set; }
        int? Anzahl { get; set; }
        Guid IdBediener { get; set; }
    }

    public interface IJgMaschine : IJgBase
    {
        string MaschineName { get; set; }
        MaschinenArten MaschineArt { get; set; }

        string MaschineIp { get; set; }
        int MaschinePort { get; set; }

        bool SammelScannung { get; set; }
        string NummerScanner { get; set; }
        bool ScannerMitDisplay { get; set; }
    }
}
