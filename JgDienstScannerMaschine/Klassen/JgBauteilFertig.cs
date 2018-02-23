using System;

namespace JgDienstScannerMaschine
{
    public class JgBauteilFertig
    {
        public Guid IdBauteil { get; set; }
        public string IdJgData { get; set; }
        public DateTime Erstellt { get; set; }

        public JgBauteilFertig()
        { }

        public JgBauteilFertig(Guid MyIdBauteil, string MyIdJgData)
        {
            IdBauteil = MyIdBauteil;
            IdJgData = MyIdJgData;
            Erstellt = DateTime.Now;
        }
    }
}