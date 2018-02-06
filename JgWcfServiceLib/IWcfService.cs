using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml.Serialization;

namespace JgWcfServiceLib
{
    [ServiceContract]
    public interface IWcfService
    {
        [OperationContract]
        string SendeTest(String value);

        [OperationContract]
        bool SendeBauteil(JgWcfBauteil Bauteil);

        [OperationContract]
        bool SendeMeldung(JgWcfMeldung Meldung);

        [OperationContract]
        bool SendeMaschinenanmeldung(JgWcfMaschinenanmeldung Anmeldung);

        [OperationContract]
        JgWcfOptionen GetOptionen(Guid IdStanort);

        [OperationContract]
        List<JgDbBediener> GetBediener();

        [OperationContract]
        List<JgDbMaschine> GetMaschinen();

        [OperationContract]
        bool SendeMaschinenStatus(JgWcfMaschine Maschine);
    }

    #region WCF Klassen ***************************

    [DataContract]
    public class JgWcfOptionen
    {
        [DataMember]
        public Guid IdStandort { get; set; }

        [DataMember]
        public string NameStanort { get; set; }
    }

    [DataContract]
    public class JgWcfStamm
    {
        [DataMember]
        public DateTime Zeit { get; set; }

        [DataMember]
        public int IdMaschine { get; set; }
    }


    [DataContract]
    public class JgWcfBauteil : JgWcfStamm
    {
        [DataMember]
        public int IdBauteil { get; set; }

        [DataMember]
        public int Anzahl { get; set; }

        [DataMember]
        public double Gewicht { get; set; }
    }

    [DataContract]
    public class JgWcfMaschinenanmeldung : JgWcfStamm
    {
        [DataMember]
        public int IdBenutzer { get; set; }

        [DataMember]
        public bool Anmeldung { get; set; }
    }

    [DataContract]
    public class JgWcfMeldung : JgWcfStamm
    {
        public enum EnumMeldungen
        {
            ReparaturStart,
            ReparaturEnde,
            CoilwechselStart,
            CoilwechselEnde
        }

        [DataMember]
        public EnumMeldungen Meldung { get; set; }

        [DataMember]
        public bool Anzahl { get; set; }
    }

    #endregion

    #region Db Klassen ****************************

    [DataContract]
    public class JgDbStamm
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public DateTime Datum { get; set; } = DateTime.Now;
    }

    [DataContract]
    public class JgDbMaschine : JgDbStamm
    {
        public enum EnumArtMaschine
        {
            Hand,
            Elg,
            Arsch
        }

        [DataMember]
        public string MaschineName;

        [DataMember]
        public EnumArtMaschine MaschineArt = EnumArtMaschine.Hand;

        [DataMember]
        public string MaschineIp { get; set; }

        [DataMember]
        public int MaschinePort { get; set; }
    }

    [DataContract]
    public class JgDbBediener : JgDbStamm
    {
        [DataMember]
        public string BedienerName;
    }

    #endregion

    #region MaschinenStatus ***********************

    [DataContract]
    public class JgWcfMaschine : JgDbMaschine
    {
        public enum EnumStatusMaschine
        {
            Unbekannt,
            Frei,
            InArbeit,
            InPause,
            InReparatur,
            InWartung,
            InCoilwechsel
        }

        [DataMember]
        public Guid? IdBediener { get; set; } = null;

        [DataMember]
        public List<Guid> IdisHelfer { get; set; } = new List<Guid>();

        [DataMember]
        public List<Guid> IdisBauteile { get; set; } = new List<Guid>();

        [DataMember]
        public EnumStatusMaschine Status { get; set; } = EnumStatusMaschine.Unbekannt;

        // wird nur für die Anzeige der Maschine verwendet
        [XmlIgnore]
        [DataMember]
        public JgDbBediener Bediener { get; set; } = null;

        // wird nur für die Anzeige der Maschine verwendet
        [XmlIgnore]
        [DataMember]
        public List<JgDbBediener> Helfer { get; set; } = null;

        // wird nur für die Anzeige der Maschine verwendet
        [XmlIgnore]
        [DataMember]
        public List<JgWcfBauteil> Bauteile { get; set; } = new List<JgWcfBauteil>();
    }

    #endregion
}
