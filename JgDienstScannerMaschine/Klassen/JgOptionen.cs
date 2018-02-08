using System;
using System.Collections.Generic;
using System.IO;
using System.Messaging;
using System.Reflection;
using System.Threading;

namespace JgDienstScannerMaschine
{
    public class JgOptionen
    {
        public Guid IdStandort { get; set; }

        public JgOptionenCraddle OptScanner { get; set; } = null;

        public string PathQueue { get; }  = ".\\Private$\\JgMaschineVonScanner";

        public Dictionary<Guid, JgMaschineStamm> ListeMaschinen = new Dictionary<Guid, JgMaschineStamm>();
        public Dictionary<Guid, JgBediener> ListeBediener = new Dictionary<Guid, JgBediener>();

        public CancellationTokenSource CraddelTokensSource { get; set; } = new CancellationTokenSource();

        public string PfadProgramm { get; }
        public string PfadDaten { get; }

        public JgOptionen()
        {
            FileInfo fi = new FileInfo(Assembly.GetEntryAssembly().Location);
            PfadProgramm = fi.DirectoryName;

            if (!MessageQueue.Exists(PathQueue))
                MessageQueue.Create(PathQueue, true);

            PfadDaten = PfadProgramm + @"\Daten\";

            if (!Directory.Exists(PfadDaten))
                Directory.CreateDirectory(PfadDaten);
        }
    }
}
