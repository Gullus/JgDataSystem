using JgWcfServiceLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JgScannerMaschineLib
{
    public struct StructOptionenScannerMaschine
    {
        public string PathQueue;
        public Dictionary<Guid, JgMaschineStamm> ListeMaschinen;
        public Dictionary<Guid, JgDbBediener> ListeBediener;
    }

    public struct StructOptionenClientServer
    {
        public string PathQueue;
        public Dictionary<Guid, JgMaschineStamm> ListeMaschinen;
    }
}
