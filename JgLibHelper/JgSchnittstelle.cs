using System;
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

    public interface IJgMaschineProgram : IJgBase
    {
        ScannerProgram Program { get; set; }
        DateTime ZeitMeldung { get; set; }

        int? Anzahl { get; set; }
        string Bemerkung { get; set; }

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

        int VorschubProMeterInSek { get; set; }
        int ZeitProBiegungInSek { get; set; }
        int ZeitProBauteilInSek { get; set; }

        string NummerScanner { get; set; }
        bool ScannerMitDisplay { get; set; }
    }

    public interface IJgMaschineStatus
    {
        Guid? Bediener { get; set; }
        List<Guid> ListeHelfer { get; set; }
        List<Guid> ListeBauteile { get; set; }
        StatusProduktion ProdStatus { get; set; }
    }

}
