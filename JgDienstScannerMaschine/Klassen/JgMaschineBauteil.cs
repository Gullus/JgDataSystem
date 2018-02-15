using JgLibHelper;
using System;
using System.Collections.Generic;

namespace JgDienstScannerMaschine
{
    public class JgMaschineBauteil : ServiceRef.JgWcfBauteil
    {
        public JgMaschineBauteil()
        {
            Id = Guid.NewGuid();
            Aenderung = DateTime.Now;
        }
    }
}
