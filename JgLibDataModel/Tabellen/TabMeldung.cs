using JgLibHelper;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace JgLibDataModel
{
    public class TabMeldung : TabBase, IJgMaschineMeldung
    {
        public DateTime ZeitMeldung { get; set; }
        public DateTime? ZeitAbmeldung { get; set; } = null;

        public ScannerMeldung Meldung { get; set; }

        public int? Anzahl { get; set; }

        public string Bemerkung { get; set; }

        public Guid IdMaschine { get; set; }
        [ForeignKey("IdMaschine")]
        public TabMaschine EMaschine { get; set; }

        public Guid IdBediener { get; set; }
        [ForeignKey("IdBediener")]
        public TabBediener EBediener { get; set; }
    }
}
