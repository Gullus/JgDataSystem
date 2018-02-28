using JgLibDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JgMaschineAspWeb.Models
{
    public class JgMaschineStatusAnzeige
    {
        public TabMaschine Maschine { get; set; }
        public TabBauteil Bauteil { get; set; } = null;
        public TabMeldung Bediener { get; set; } = null;
        public List<TabMeldung> ListeHelfer { get; set; } = null;
        public TabMeldung Meldung { get; set; }

        public DateTime Aenderung { get; set; }
        public string Information { get; set; }
    }
}