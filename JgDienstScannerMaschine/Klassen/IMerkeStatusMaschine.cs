using System.Collections.Generic;

namespace JgDienstScannerMaschine
{
    public interface IJgMaschineStatus
    {
        JgMeldung MeldMeldung { get; set; }

        JgMeldung MeldBediener { get; set; }
        List<JgMeldung> MeldListeHelfer { get; set; }

        JgBauteil AktivBauteil { get; set; }
    }
}
