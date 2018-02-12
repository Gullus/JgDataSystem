using JgLibHelper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace JgLibDataModel
{
    public class TabBediener : TabBase, IJgBediener
    {
        public string Vorname { get; set; }
        public string Nachname { get; set; }

        [InverseProperty("EBediener")]
        public ICollection<TabMeldung> SMeldungen { get; set; }

        [InverseProperty("EBediener")]
        public ICollection<TabBedienerBauteil> SBauteilBediener { get; set; }

        public TabBediener()
        {
            SMeldungen = new HashSet<TabMeldung>();
        }
    }

}
