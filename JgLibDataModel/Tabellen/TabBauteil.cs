using JgLibHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace JgLibDataModel
{
    public class TabBauteil : TabBase, IJgBauteil
    {
        #region Schnittstelle

        public int AnzahlTeile { get; set; }
        public int DuchmesserInMm { get; set; }
        public double GewichtInKg { get; set; }
        public int LaengeInCm { get; set; }
        public int AnzahlBiegungen { get; set; }

  
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

        // Berechnete Felder **************************

        public string ZeitPraktisch
        {
            get
            {
                if (EndeFertigung != null)
                {
                    var zeit = EndeFertigung.Value - StartFertigung;
                    return $"{((int)zeit.TotalMinutes).ToString("D2")}:{zeit.Seconds.ToString("D2")}";
                }

                return "-";
            }
        }

        public string ZeitTheoretisch
        {
            get
            {
                if (EMaschine != null)
                {
                    var erg = AnzahlTeile * (EMaschine.ZeitProBauteilInSek + LaengeInCm / 100 * EMaschine.VorschubProMeterInSek + AnzahlBiegungen * EMaschine.ZeitProBiegungInSek);
                    var zeit = new TimeSpan(0, 0, (int)erg);
                    return $"{((int)zeit.TotalMinutes).ToString("D2")}:{zeit.Seconds.ToString("D2")}";
                }

                return "-";
            }
        }

        public TabBauteil()
        { }
    }
}
