using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using JgLibHelper;

namespace JgLibDataModel
{
    public class TabMaschine : TabBase, IJgMaschine
    {
        public string MaschineName { get; set; }

        public MaschinenArten MaschineArt { get; set; } = MaschinenArten.Hand;

        public string MaschineIp { get; set; }
        public int MaschinePort { get; set; }
        public bool SammelScannung { get; set; }

        public int VorschubProMeterInSek { get; set; }
        public int ZeitProBiegungInSek { get; set; }
        public int ZeitProBauteilInSek { get; set; }

        public bool ScannerMitDisplay { get; set; }
        public string NummerScanner { get; set; }

        public bool IstAktiv { get; set; }

        public string Bemerkung { get; set; }

        public Guid IdStandort { get; set; }
        [ForeignKey("IdStandort")]
        public TabStandort EStandort { get; set; }

        [InverseProperty("EMaschine")]
        public ICollection<TabMeldung> SMeldungen { get; set; }

        public TabMaschine()
        {
            SMeldungen = new HashSet<TabMeldung>();
        }
    }
}
