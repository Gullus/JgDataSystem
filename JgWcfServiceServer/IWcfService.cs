using JgLibHelper;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;

namespace JgWcfServiceLib
{
    [ServiceContract]
    public interface IWcfService
    {
        [OperationContract]
        string WcfTest(String TestString);

        [OperationContract]
        Task<bool> SendeBauteil(JgWcfBauteil Bauteil, JgWcfMaschineStatus Maschine);

        [OperationContract]
        Task<bool> SendeMeldung(JgWcfMeldung Meldung, JgWcfMaschineStatus Maschine);

        [OperationContract]
        Task<List<JgWcfBediener>> GetBediener();

        [OperationContract]
        Task<List<JgWcfMaschine>> GetMaschinen(Guid IdStandort);
    }

    [DataContract]
    public class JgWcfBase : IJgBase
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public DateTime Aenderung { get; set; }
    }

    [DataContract]
    public class JgWcfBauteil : JgWcfBase, IJgBauteil
    {
        [DataMember]
        public int DuchmesserInMm { get; set; }

        [DataMember]
        public double GewichtInKg { get; set; }

        [DataMember]
        public int LaengeInCm { get; set; }

        [DataMember]
        public int AnzahlBiegungen { get; set; }

        [DataMember]
        public string IdBauteilJgData { get; set; }

        [DataMember]
        public Guid IdMaschine { get; set; }

        [DataMember]
        public Guid IdBediener { get; set; }

        [DataMember]
        public int AnzahlHelfer { get; set; }
    }

    [DataContract]
    public class JgWcfMeldung : JgWcfBase, IJgMeldung
    {
        #region Aus Schittsettele

        [DataMember]
        public ScannerMeldung Meldung { get; set; }

        [DataMember]
        public int? Anzahl { get; set; }


        [DataMember]
        public Guid IdBediener { get; set; }

        #endregion

        [DataMember]
        public Guid IdMaschine { get; set; }
    }

    [DataContract]
    public class JgWcfMaschine : JgWcfBase, IJgMaschine
    {
        [DataMember]
        public string MaschineName { get; set; }

        [DataMember]
        public MaschinenArten MaschineArt { get; set; }

        [DataMember]
        public string MaschineIp { get; set; }

        [DataMember]
        public int MaschinePort { get; set; }

        [DataMember]
        public bool SammelScannung { get; set; }

        [DataMember]
        public string NummerScanner { get; set; }

        [DataMember]
        public bool ScannerMitDisplay { get; set; }
    }

    [DataContract]
    public class JgWcfBediener : JgWcfBase, IJgBediener
    {
        [DataMember]
        public string Vorname { get; set; }

        [DataMember]
        public string Nachname { get; set; }

        [DataMember]
        public string NummerAusweis { get; set; }
    }

    [DataContract]
    public class JgWcfMaschineStatus : JgWcfBase, IJgMaschineStatus
    {
        [DataMember]
        public Guid? IdMeldungBediener { get; set; }

        [DataMember]
        public List<Guid> ListeIdMeldungHelfer { get; set; }

        [DataMember]
        public Guid? IdMeldungMeldung { get; set; }

        [DataMember]
        public Guid? IdBauteilAktiv { get; set; }
    }
}
