using JgWcfServiceLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace JgScannerMaschineLib
{
    public class JgScanner
    {
        public Dictionary<Guid, JgDbBediener> _ListeBediener = new Dictionary<Guid, JgDbBediener>();
        public Dictionary<Guid, JgMaschineStamm> _ListeMaschinen = new Dictionary<Guid, JgMaschineStamm>();

        public JgScanner(StructOptionenScannerMaschine Optionen)
        {
            var speicherQueue = new MessageQueue(Optionen.PathQueue, QueueAccessMode.Send);

            _ListeBediener = Optionen.ListeBediener;
            _ListeMaschinen = Optionen.ListeMaschinen;
        }
    }
}
