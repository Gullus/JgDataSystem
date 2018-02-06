using JgLibHelper;
using System.Collections.Generic;

namespace JgDienstScannerMaschine
{
    public abstract class JgMaschineStamm : JgBasisKlasse
    {
        public string MaschineName { get; set; } = "";
        public MaschinenArten MaschineArt { get; set; } = MaschinenArten.Hand;

        public string MaschineIp { get; set; }
        public int MaschinePort { get; set; }

        public JgBediener Bediener { get; set; } = null;

        public List<JgBediener> Helfer { get; set; } = new List<JgBediener>();

        public override string ToString()
        {
            return MaschineName;
        }
    }

    public abstract class MaschineAbstract : JgMaschineStamm
    {
        public void DatenVonScanner(string DatenScanner)
        {
            var bvbsCode = DatenScanner;




            SendeDatenZurMaschine(bvbsCode);
        }

        public abstract void SendeDatenZurMaschine(string BvBsCode);

        public override string ToString()
        {
            return MaschineName;
        }
    }

    public class JgMaschineEvg : MaschineAbstract
    {
        public override void SendeDatenZurMaschine(string BvBsCode)
        {

        }

    }

    public class JgMaschineArsch : MaschineAbstract
    {
        public override void SendeDatenZurMaschine(string BvBsCode)
        {

        }

    }

    public class JgMaschineHand : MaschineAbstract
    {
        public override void SendeDatenZurMaschine(string BvBsCode)
        {

        }

    }
}
