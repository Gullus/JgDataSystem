using JgLibHelper;
using System;
using System.ComponentModel.DataAnnotations;

namespace JgLibDataModel
{
    public class TabBase : IJgBase
    {
        public Guid Id { get; set; }
        public DateTime Aenderung { get; set; }

        [TimestampAttribute]
        public byte[] Modifikation { get; set; }
    }
}
