using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace JgDienstScannerMaschine
{
    public class JgOptionen
    {
        public Guid IdStandort { get; set; }

        public JgOptionenScanner OptScanner { get; set; } = null;

        public const string PathQueue = ".\\Private$\\JgMaschineVonScanner";

        public Dictionary<Guid, JgMaschineStamm> ListeMaschinen = new Dictionary<Guid, JgMaschineStamm>();
        public Dictionary<Guid, JgBediener> ListeBediener = new Dictionary<Guid, JgBediener>();

        public string PfadExe { get; }

        public JgOptionen()
        {
            FileInfo fi = new FileInfo(Assembly.GetEntryAssembly().Location);
            PfadExe = fi.DirectoryName;
        }
    }
}
