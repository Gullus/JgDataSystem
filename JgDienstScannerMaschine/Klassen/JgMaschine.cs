using JgLibHelper;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Generic;

namespace JgDienstScannerMaschine
{
    public abstract class JgMaschineStamm : JgBaseClass, IJgMaschine
    {
        public string MaschineName { get; set; }
        public MaschinenArten MaschineArt { get; set; } = MaschinenArten.Hand;

        public string MaschineIp { get; set; } = "";
        public int MaschinePort { get; set; } = 5100;

        public string NummerScanner { get; set; } = "";
        public bool SammelScannung { get; set; } = false;
        public bool ScannerMitDisplay { get; set; } = true;

        public int VorschubProMeterinSek { get; set; } = 0;
        public int ZeitProBiegunginSek { get; set; } = 0;
        public int ZeitProBauteilinSek { get; set; } = 0;

        public Guid? Bediener { get; set; } = null;
        public List<Guid> ListHelfer { get; set; } = new List<Guid>();
        public List<Guid> ListeBauteile { get; set; } = new List<Guid>();

        public StatusMaschine Status { get; set; } = StatusMaschine.Frei;
        string IJgMaschine.Bemerkung { get; set; }

        public abstract void SendeDatenZurMaschine(string BvBsCode);

        public override string ToString()
        {
            return MaschineName;
        }
    }

    public class JgMaschineEvg : JgMaschineStamm
    {
        public override void SendeDatenZurMaschine(string BvBsCode)
        {
            Logger.Write()

        }
    }

    public class JgMaschineSchnell : JgMaschineStamm
    {
        public override void SendeDatenZurMaschine(string BvBsCode)
        {


        }
    }

    public class JgMaschineHand : JgMaschineStamm
    {
        public override void SendeDatenZurMaschine(string BvBsCode)
        {


        }
    }
}
