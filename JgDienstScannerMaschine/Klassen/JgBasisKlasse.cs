using JgLibHelper;
using System;

namespace JgDienstScannerMaschine
{
    public class JgBaseClass : IJgBase
    {
        public Guid Id { get; set; }
        public DateTime Aenderung { get; set; }

        public JgBaseClass()
        { }
    }
}
