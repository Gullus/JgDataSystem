using System.ComponentModel.DataAnnotations;

namespace JgLibDataModel
{
    public class TabReport : TabBase
    {
        [Required]
        [MinLength(5, ErrorMessage ="Text muss mindestens 5 Zeichen enthalten")]
        [MaxLength(30,ErrorMessage ="Der Text darf nicht mehr als 30 Zeichen enthalten")]
        public string ReportName { get; set; }

        public string Beschreibung { get; set; }

        public byte[] ReportDaten { get; set; }
    }
}
