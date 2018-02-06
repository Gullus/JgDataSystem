using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JgLibDataModel
{ 
    public class TabStandort : TabBase
    {
        [Required]
        public string StandortName { get; set; }

        [InverseProperty("EStandort")]
        public ICollection<TabMaschine> SKfzEinsatz { get; set; }
    }
}
