using System.Messaging;

namespace JgDienstScannerMaschine
{
    public class JgOptionenCraddle
    {
        public JgOptionen JgOpt { get; }
        public MessageQueue MessQueue { get; }

        public string CraddleIpAdresse { get; set; } = "";
        public int CraddlePort { get; set; } = 0;
        public string TextVerbinungOk { get; set; } = "";

        public string TextBeiFehler { get; } = "#FehlerCraddle";

        public string Info { get => $"Craddle Ip: {CraddleIpAdresse} Port: {CraddlePort}"; } 

        public JgOptionenCraddle(JgOptionen MyOptionen)
        {
            JgOpt = MyOptionen;
        }
    }
}
