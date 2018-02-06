using JgWcfServiceLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JgScannerMaschineLib
{
    public abstract class JgMaschineStamm : JgWcfMaschine
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

    public class JgMaschineEvg : JgMaschineStamm
    {
        public override void SendeDatenZurMaschine(string BvBsCode)
        {

        }

    }

    public class JgMaschineArsch : JgMaschineStamm
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
