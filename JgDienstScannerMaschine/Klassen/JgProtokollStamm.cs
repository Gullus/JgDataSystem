using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JgDienstScannerMaschine
{
    public abstract class JgProtokollStamm
    {
        public class StructDatenZeit
        {
            public DateTime StartZeit;
            public DateTime EndZeit;
            public string Kennzeichen;
        }

        public List<StructDatenZeit> ListeDatenZeit = new List<StructDatenZeit>();

        public abstract void DatenEinlesen(string[] TextDaten, DateTime? DatumDaten = null);
    }
}
