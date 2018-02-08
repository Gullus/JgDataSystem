using JgLibHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace JgLibDataModel.Tabellen
{
    public class TabBauteil : TabBase, IJgBauteil
    {
        public DateTime StartFertigung { get; set; }
        public DateTime? EndeFertigung { get; set; } = null;

        public int DuchmesserInMm { get; set; } = 0;
        public double GewichtInKg { get; set; } = 0;
        public int LaengeInCm { get; set; } = 0;
        public int AnzahlBiegungen { get; set; } = 0;

        public int IdBauteilJgData { get; set; } = -1;

        public Guid IdMaschine { get; set; }
        [ForeignKey("IdMaschine")]
        public TabMaschine EMaschine { get; set; }

        public List<Guid> ListeHelfer { get; set; }

        public TabBauteil()
        {
            StartFertigung = DateTime.Now;
        }
    }
}
