using JgLibHelper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JgLibDataModel
{
    public class TabBediener : TabBase, IJgBediener
    {
        #region Schnittstelle

        [Required]
        [MaxLength(30, ErrorMessage = "Es dürfen nicht mehr als 30 Zeichen verwendet werden")]
        public string Vorname { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "Es dürfen nicht mehr als 30 Zeichen verwendet werden")]
        public string Nachname { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "Es dürfen nicht mehr als 30 Zeichen verwendet werden")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Die Zeichen müssen 3 Zeichen lang sein.")]
        public string NummerAusweis { get; set; }

        #endregion

        [InverseProperty("EBediener")]
        public ICollection<TabMeldung> SMeldungen { get; set; }

        [InverseProperty("EBediener")]
        public ICollection<TabBauteil> SBauteile { get; set; }

        // ******************************************************************

        public string AnzeigeName { get => $"{Nachname}, {Vorname}"; }

        public TabBediener()
        {
            SMeldungen = new HashSet<TabMeldung>();
            SBauteile = new HashSet<TabBauteil>();
        }
    }

}
