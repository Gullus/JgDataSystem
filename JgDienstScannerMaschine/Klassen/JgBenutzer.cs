using JgLibHelper;

namespace JgDienstScannerMaschine
{
    public class JgBediener : JgBaseClass, IJgBediener
    { 
        public string Vorname { get; set; } = "";
        public string Nachname { get; set; }

        public string NummerAusweis { get; set; }

        public string BedienerName { get => $"{Nachname}, {Vorname}"; }
    }
}
