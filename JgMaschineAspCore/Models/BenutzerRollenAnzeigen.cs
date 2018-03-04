
namespace JgMaschineAspCore.Models
{
    public class BenutzerRollenAnzeigen
    {
        public string Id { get; set; }
        public string BenutzerName { get; set; }

        public bool IstAdmin { get; set; }
        public bool IstAuswertung { get; set; }
    }
}
