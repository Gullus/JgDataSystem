

namespace JgDienstScannerMaschine
{
    public class JgBediener : ServiceRef.JgWcfBediener
    { 
        public string BedienerName { get => $"{Nachname}, {Vorname}"; }
    }
}
