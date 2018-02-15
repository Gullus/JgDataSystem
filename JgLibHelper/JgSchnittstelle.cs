﻿using System;
using System.Collections.Generic;

namespace JgLibHelper
{
    public interface IJgBase
    {
        Guid Id { get; set; }
        DateTime Aenderung { get; set;}
    }

    public interface IJgBediener : IJgBase
    {
        string Vorname { get; set; }
        string Nachname { get; set; }

        string NummerAusweis { get; set; }
    }

    public interface IJgMaschineBauteil : IJgBase
    {
        DateTime StartFertigung { get; set; }
        DateTime? EndeFertigung { get; set; }

        int DuchmesserInMm { get; set; }
        double GewichtInKg { get; set; }
        int LaengeInCm { get; set; }
        int AnzahlBiegungen { get; set; }

        Guid IdMaschine { get; set; }
        Guid Bediener { get; set; }
        List<Guid> ListeHelfer { get; set; }

        // Id Bauteail aus JgData
        string IdBauteilJgData { get; set; }
    }

    public interface IJgMaschineMeldung : IJgBase
    {
        ScannerMeldung Meldung { get; set; }
        DateTime ZeitMeldung { get; set; }

        int? Anzahl { get; set; }

        Guid IdMaschine { get; set; }
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

    public interface IJgMaschineStatus
    {
        Guid? IdMeldungBediener { get; set; }
        List<Guid> ListeIdMeldungHelfer { get; set; }
        Guid? IdMeldungMeldung { get; set; } 
    }

}
