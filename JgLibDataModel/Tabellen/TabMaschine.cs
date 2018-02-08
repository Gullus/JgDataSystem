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
        public StatusMaschine Status { get; set; } = StatusMaschine.InArbeit;

        public string MaschineIp { get; set; }
        public int MaschinePort { get; set; }
        public bool SammelScannung { get; set; }

        public int VorschubProMeterinSek { get; set; }
        public int ZeitProBiegunginSek { get; set; }
        public int ZeitProBauteilinSek { get; set; }

        public bool ScannerMitDisplay { get; set; }
        public string NummerScanner { get; set; }

        public Guid? Bediener { get; set; }
        public List<Guid> ListHelfer { get; set; }
        public List<Guid> ListeBauteile { get; set; }

        public string Bemerkung { get; set; }

        public Guid FStandort { get; set; }
        [ForeignKey("FStandort")]
        public TabStandort EStandort { get; set; }

        [InverseProperty("EMaschine")]
        public ICollection<TabMeldung> SMeldungen { get; set; }
    }
}
