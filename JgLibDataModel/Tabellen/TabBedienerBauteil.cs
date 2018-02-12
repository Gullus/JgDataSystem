using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace JgLibDataModel
{
    public class TabBedienerBauteil : TabBase
    {
        public Guid IdBediener { get; set; }
        [ForeignKey("IdBediener")]
        public TabBediener EBediener { get; set; }

        public Guid IdBauteil { get; set; }
        [ForeignKey("IdBauteil")]
        public TabBauteil EBauteil { get; set; }
    }
}
