using System;

namespace JgDienstScannerMaschine
{
    public class JgProtokollBase
    {
        internal string[] _ArDaten { get; set; }

        public JgProtokollBase(string[] MyDaten)
        {
            _ArDaten = MyDaten;
        }
    }
}
