using JgLibHelper;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace JgLibDataModel
{
    public class TabMeldung : TabBase, IJgMaschineProgram
    {
        public DateTime ZeitMeldung { get; set; }
        public ScannerProgram Program { get; set; } = ScannerProgram.SCVORGANG;

        public int? Anzahl { get; set; } = 0;

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
