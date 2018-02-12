using JgLibHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace JgLibDataModel
{
    public class TabBauteil : TabBase, IJgMaschineBauteil
    {
        public DateTime StartFertigung { get; set; }
        public DateTime? EndeFertigung { get; set; } = null;

        public int DuchmesserInMm { get; set; } = 0;
        public double GewichtInKg { get; set; } = 0;
        public int LaengeInCm { get; set; } = 0;
        public int AnzahlBiegungen { get; set; } = 0;

        public string IdBauteilJgData { get; set; } = null;

        public Guid IdMaschine { get; set; }
        [ForeignKey("IdMaschine")]
        public TabMaschine EMaschine { get; set; }

        public Guid Bediener { get; set; }


        // Muss nach der Übertragung noch eingetragen werden

        [NotMapped]
        public List<Guid> ListeHelfer { get; set; }

        [InverseProperty("EBauteil")]
        public ICollection<TabBedienerBauteil> SBedienerBauteil { get; set; }

        public TabBauteil()
        {
            StartFertigung = DateTime.Now;
        }
    }
}
