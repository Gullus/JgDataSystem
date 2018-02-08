using JgLibDataModel;
using JgLibHelper;
using System.Linq;

namespace JgTestConsole.Temp
{
    public class StandortMaschineEintragen
    {
        public StandortMaschineEintragen()
        {
            using (var db = new JgMaschineDb() { SqlVerbindung = Properties.Settings.Default.SqlVerbindung })
            {
                var standort = db.TabStandortSet.FirstOrDefault();

                db.TabBedienerSet.AddRange(
                    new TabBediener() { Vorname = "Jörg", Nachname = "Gullus" },
                    new TabBediener() { Vorname = "Uta", Nachname = "Lachmann" },
                    new TabBediener() { Vorname = "Bert", Nachname = "Muschick" }
                );

                //db.TabMaschineSet.AddRange(
                //    new TabMaschine()
                //    {
                //        EStandort = standort,
                //        IpAdresse = "192.168.1.84",
                //        Port = 5320,
                //        MaschineName = "Handmaschine",
                //        MaschinenArt = MaschinenArten.Hand
                //    },
                //    new TabMaschine()
                //    {
                //        EStandort = standort,
                //        IpAdresse = "192.168.1.139",
                //        Port = 587,
                //        MaschineName = "Evg Maschine",
                //        MaschinenArt = MaschinenArten.Evg
                //    }, new TabMaschine()
                //    {
                //        EStandort = standort,
                //        IpAdresse = "192.168.1.120",
                //        Port = 5000,
                //        MaschineName = "Arsch Maschine",
                //        MaschinenArt = MaschinenArten.Arsch
                //    });

                db.SaveChanges();

            }
        }
    }
}
