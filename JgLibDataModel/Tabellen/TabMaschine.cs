using System;
using System.ComponentModel.DataAnnotations.Schema;
using JgLibHelper;

namespace JgLibDataModel
{
    public class TabMaschine : TabBase
    {
        public string MaschineName { get; set; }

        public MaschinenArten MaschinenArt { get; set; } = MaschinenArten.Hand;

        public string IpAdresse { get; set; }
        public int Port { get; set; }

        public Guid FStandort { get; set; }
        [ForeignKey("FStandort")]
        public TabStandort EStandort { get; set; }
    }
}
