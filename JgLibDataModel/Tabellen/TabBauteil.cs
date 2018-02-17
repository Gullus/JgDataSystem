using JgLibHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace JgLibDataModel
{
    public class TabBauteil : TabBase, IJgBauteil
    {
        #region Schnittstelle

        public int DuchmesserInMm { get; set; } = 0;
        public double GewichtInKg { get; set; } = 0;
        public int LaengeInCm { get; set; } = 0;
        public int AnzahlBiegungen { get; set; } = 0;

        public string IdBauteilJgData { get; set; } = null;

        public Guid IdMaschine { get; set; }
        public Guid IdBediener { get; set; }
        public int AnzahlHelfer { get; set; }
   
        #endregion

        public DateTime StartFertigung { get; set; }
        public DateTime? EndeFertigung { get; set; } = null;

        [ForeignKey("IdMaschine")]
        public TabMaschine EMaschine { get; set; }

        [ForeignKey("IdBediener")]
        public TabBediener EBediener { get; set; }

        public TabBauteil()
        { }
    }
}
