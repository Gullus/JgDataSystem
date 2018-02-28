using System;
using System.Collections.Generic;

namespace JgLibHelper
{
    public class JgMaschinenStatusMeldungen
    {
        public Guid? IdAktivBauteil { get; set; }
        public Guid? IdBediener { get; set; }
        public Guid? IdMeldung { get; set; }
        public List<Guid> IdListeHelfer { get; set; } = new List<Guid>();

        public string Information { get; set; }
    }
}

