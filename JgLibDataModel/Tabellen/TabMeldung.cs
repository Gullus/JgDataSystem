using JgLibHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JgLibDataModel
{
    public class TabMeldung : TabBase, IJgMeldung
    {
        public DateTime ZeitMeldung { get; set; }
        public VorgangProgram Vorgang { get; set; } = VorgangProgram.FEHLER;

        public int Anzahl { get; set; } = 0;

        public string Bemerkung { get; set; }

        public Guid IdMaschine { get; set; }
        [ForeignKey("IdMaschine")]
        public TabMaschine EMaschine { get; set; }

        public Guid IdBediener { get; set; }
        [ForeignKey("IdBediener")]
        public TabBediener EBediener { get; set; }

        public TabMeldung()
        {
            ZeitMeldung = DateTime.Now;
        }
    }
}
