using System;
using System.ComponentModel.DataAnnotations;

namespace JgLibDataModel
{
    public class TabBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Aenderung { get; set; } = DateTime.Now;

        [TimestampAttribute]
        public byte[] Modifikation { get; set; }
    }
}
