using JgLibHelper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JgLibDataModel
{ 
    public class TabStandort : TabBase, IJgStandort
    {
        [Required]
        [MaxLength(30, ErrorMessage = "Es dürfen nicht mehr als 30 Zeichen verwendet werden")]
        public string StandortName { get; set; }

        [InverseProperty("EStandort")]
        public ICollection<TabMaschine> SMaschinen { get; set; }

        public TabStandort()
        {
            SMaschinen = new HashSet<TabMaschine>();
        }
    }
}
