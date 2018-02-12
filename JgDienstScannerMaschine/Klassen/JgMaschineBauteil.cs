using JgLibHelper;
using System;
using System.Collections.Generic;

namespace JgDienstScannerMaschine
{
    public class JgMaschineBauteil : JgBaseClass, IJgMaschineBauteil
    {
        public JgMaschineBauteil()
        {
            Id = Guid.NewGuid();
            Aenderung = DateTime.Now;
        }

        public DateTime StartFertigung { get; set; }
        public DateTime? EndeFertigung { get; set; }

        public int DuchmesserInMm { get; set; }
        public double GewichtInKg { get; set; }
        public int LaengeInCm { get; set; }
        public int AnzahlBiegungen { get; set; }

        public Guid IdMaschine { get; set; }
        public Guid Bediener { get; set; }
        public List<Guid> ListeHelfer { get; set; }

        public string IdBauteilJgData { get; set; }
    }
}
