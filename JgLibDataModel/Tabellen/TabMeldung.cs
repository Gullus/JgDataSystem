using JgLibHelper;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace JgLibDataModel
{
    public class TabMeldung : TabBase, IJgMeldung
    {
        #region von Schnittstelle

        public DateTime ZeitMeldung { get; set; }
        public int? Anzahl { get; set; }
        public ScannerMeldung Meldung { get; set; }

        #endregion

        public DateTime? ZeitAbmeldung { get; set; } = null;
        public string Bemerkung { get; set; }

        public StatusMeldung Status { get; set; } = StatusMeldung.Offen;

        public Guid IdMaschine { get; set; }
        [ForeignKey("IdMaschine")]
        public TabMaschine EMaschine { get; set; }

        public Guid IdBediener { get; set; }
        [ForeignKey("IdBediener")]
        public TabBediener EBediener { get; set; }

        public TabMeldung()
        { }
    }
}
