using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JgLibHelper
{
    public class JgMaschineStatus : IJgMaschineStatus
    {
        public Guid? Bediener { get; set; }
        public List<Guid> ListeHelfer { get; set; }
        public List<Guid> ListeBauteile { get; set; }
        public StatusProduktion ProdStatus { get; set; }
    }
}
