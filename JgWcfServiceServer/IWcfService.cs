using JgLibHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JgWcfServiceLib
{
    [ServiceContract]
    public interface IWcfService
    {
        [OperationContract]
        string WcfTest(String TestString);

        [OperationContract]
        Task<bool> SendeBauteil(JgWcfBauteil Bauteil);

        [OperationContract]
        Task<bool> SendeMeldung(JgWcfMeldung Meldung);

        [OperationContract]
        Task<bool> SendeMaschinenanmeldung(JgWcfMaschinenanmeldung Anmeldung);

        [OperationContract]
        Task<List<JgDbBediener>> GetBediener();

        [OperationContract]
        Task<List<JgDbMaschine>> GetMaschinen(Guid IdStandort);

        [OperationContract]
        Task<bool> SendeMaschinenStatus(JgWcfMaschine Maschine);
    }

    #region WCF Klassen ***************************

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
        public DateTime Aenderung { get; set; } = DateTime.Now;
    }

    [DataContract]
    public class JgDbMaschine : JgDbStamm
    {
        [DataMember]
        public string MaschineName;

        [DataMember]
        public MaschinenArten MaschineArt = MaschinenArten.Hand;

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
        [DataMember]
        public Guid? IdBediener { get; set; } = null;

        [DataMember]
        public List<Guid> IdisHelfer { get; set; } = new List<Guid>();

        [DataMember]
        public List<Guid> IdisBauteile { get; set; } = new List<Guid>();

        [DataMember]
        public StatusMaschine Status { get; set; } = StatusMaschine.Frei;

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
