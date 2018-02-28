using System;

namespace JgDienstScannerMaschine
{
    public class JgProtokollProgress : JgProtokollStamm
    {
        private JgProtokollProgressDaten _Ds;

        public override void DatenEinlesen(string[] TextDaten, DateTime? DatumDaten = null)
        {
            foreach(var ds in TextDaten)
            {
                _Ds = new JgProtokollProgressDaten(ds.Split(new char[] { ';' }));
                ListeDatenZeit.Add(new StructDatenZeit()
                {
                    Kennzeichen = _Ds.Position,
                    StartZeit = _Ds.StartZeit,
                    EndZeit = _Ds.EndZeit
                });
            }
        }
    }

    public class JgProtokollProgressDaten : JgProtokollBase
    {
        public string Kunde { get => _ArDaten[0]; }
        public string Auftrag { get => _ArDaten[1]; }
        public string Position { get => _ArDaten[2]; }
        public string Laenge { get => _ArDaten[3]; }
        public string Anzahl { get => _ArDaten[4]; }
        public string Drahtdurchmesser { get => _ArDaten[5]; }
        public DateTime StartZeit { get => Convert.ToDateTime(_ArDaten[6]); }
        public DateTime EndZeit { get => Convert.ToDateTime(_ArDaten[7]); }
        public string CoilCharge { get => _ArDaten[8]; }
        public string BarId { get => _ArDaten[9]; }

        public JgProtokollProgressDaten(string[] MyDaten)
            : base(MyDaten)
        { }
    }
}
