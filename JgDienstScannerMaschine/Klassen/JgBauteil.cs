using JgLibHelper;
using System;
using System.Collections.Generic;

namespace JgDienstScannerMaschine
{
    public class JgBauteil : ServiceRef.JgWcfBauteil
    {
        public JgBauteil()
        {
            Id = Guid.NewGuid();
            Aenderung = DateTime.Now;
        }
    }
}
