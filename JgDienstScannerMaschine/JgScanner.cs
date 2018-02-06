using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace JgDienstScannerMaschine
{
    public class JgScanner
    {
        public Dictionary<Guid, JgBediener> _ListeBediener = new Dictionary<Guid, JgBediener>();
        public Dictionary<Guid, JgMaschineStamm> _ListeMaschinen = new Dictionary<Guid, JgMaschineStamm>();

        //public JgScanner( Optionen)
        //{
        //    var speicherQueue = new MessageQueue(Optionen.PathQueue, QueueAccessMode.Send);

        //    _ListeBediener = Optionen.ListeBediener;
        //    _ListeMaschinen = Optionen.ListeMaschinen;
        //}
    }
}
