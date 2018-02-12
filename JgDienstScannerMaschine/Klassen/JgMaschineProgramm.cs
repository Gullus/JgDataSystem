using JgLibHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JgDienstScannerMaschine
{
    public class JgMaschineProgramm : JgBaseClass, IJgMaschineProgram
    {
        public JgMaschineProgramm()
        {
            Id = Guid.NewGuid();
            Aenderung = DateTime.Now;
        }

        public JgMaschineProgramm(Guid MyIdMaschine, Guid MyIdBediener, ScannerProgram MyProgram)
            : this()
        {
            IdMaschine = MyIdMaschine;
            IdBediener = MyIdBediener;
            Program = MyProgram;
        }


        public JgMaschineProgramm(Guid MyIdMaschine, ScannerProgram MyProgram, int? MyAnzahl = null)
            : this()
        {
            IdMaschine = MyIdMaschine;
            Program = MyProgram;
            Anzahl = MyAnzahl;
        }


        public ScannerProgram Program { get; set; }
        public DateTime ZeitMeldung { get; set; } = DateTime.Now;

        public int? Anzahl { get; set; } = null;
        public string Bemerkung { get; set; }

        public Guid IdMaschine { get; set; }
        public Guid IdBediener { get; set; }
    }
}
