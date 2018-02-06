using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JgLibDataModel.Tabellen
{
    public class tabBediener
    {
        public Guid Id { get; set; }
        public string Vorname { get; set; }
        public string Nachname { get; set; }

        public tabBediener()
        {
            Id = Guid.NewGuid();
        }
    }

}
