using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JgLibHelper;

namespace JgLibDataModel
{
    public class TabMaschine : TabBase, IJgMaschine
    {
        [Required]
        [MaxLength(30, ErrorMessage = "Es dürfen nicht mehr als 30 Zeichen verwendet werden")]
        public string MaschineName { get; set; }

        public MaschinenArten MaschineArt { get; set; } = MaschinenArten.Hand;

        public string MaschineIp { get; set; }
        public int MaschinePort { get; set; }
        public bool SammelScannung { get; set; }

        public Single VorschubProMeterInSek { get; set; }
        public Single ZeitProBiegungInSek { get; set; }
        public Single ZeitProBauteilInSek { get; set; }

        public bool ScannerMitDisplay { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "Es dürfen nicht mehr als 30 Zeichen verwendet werden")]
        public string NummerScanner { get; set; }

        public bool IstAktiv { get; set; }

        public string Bemerkung { get; set; }
        public byte[] StatusMaschine { get; set; } 

        public Guid IdStandort { get; set; }
        [ForeignKey("IdStandort")]
        public TabStandort EStandort { get; set; }

        [InverseProperty("EMaschine")]
        public ICollection<TabMeldung> SMeldungen { get; set; }

        // Anzeige für Validierung in WebPage

        [NotMapped]
        public string VorschubProMeterInSekAnzeige
        {
            get => VorschubProMeterInSek.ToString("N3");
            set
            {
                try
                {
                    VorschubProMeterInSek = Convert.ToSingle(value);
                }
                catch { }
            }
        }

        [NotMapped]
        public string ZeitProBiegungInSekAnzeige
        {
            get => ZeitProBiegungInSek.ToString("N3");
            set
            {
                try
                {
                    ZeitProBiegungInSek = Convert.ToSingle(value);
                }
                catch { }
            }
        }

        [NotMapped]
        public string ZeitProBauteilInSekAnzeige
        {
            get => ZeitProBauteilInSek.ToString("N3");
            set
            {
                try
                {
                    ZeitProBauteilInSek = Convert.ToSingle(value);
                }
                catch { }
            }
        }

        public TabMaschine()
        {
            SMeldungen = new HashSet<TabMeldung>();
        }
    }
}
