﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JgDienstScannerMaschine.ServiceRef {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="JgWcfBase", Namespace="http://schemas.datacontract.org/2004/07/JgWcfServiceLib")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(JgDienstScannerMaschine.ServiceRef.JgWcfMeldung))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(JgDienstScannerMaschine.ServiceRef.JgWcfBediener))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(JgDienstScannerMaschine.ServiceRef.JgWcfMaschine))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(JgDienstScannerMaschine.ServiceRef.JgWcfBauteil))]
    public partial class JgWcfBase : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime AenderungField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid IdField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime Aenderung {
            get {
                return this.AenderungField;
            }
            set {
                if ((this.AenderungField.Equals(value) != true)) {
                    this.AenderungField = value;
                    this.RaisePropertyChanged("Aenderung");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="JgWcfMeldung", Namespace="http://schemas.datacontract.org/2004/07/JgWcfServiceLib")]
    [System.SerializableAttribute()]
    public partial class JgWcfMeldung : JgDienstScannerMaschine.ServiceRef.JgWcfBase {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> AnzahlField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string BemerkungField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid IdBedienerField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid IdMaschineField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private JgLibHelper.ScannerProgram ProgramField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime ZeitMeldungField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> Anzahl {
            get {
                return this.AnzahlField;
            }
            set {
                if ((this.AnzahlField.Equals(value) != true)) {
                    this.AnzahlField = value;
                    this.RaisePropertyChanged("Anzahl");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Bemerkung {
            get {
                return this.BemerkungField;
            }
            set {
                if ((object.ReferenceEquals(this.BemerkungField, value) != true)) {
                    this.BemerkungField = value;
                    this.RaisePropertyChanged("Bemerkung");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid IdBediener {
            get {
                return this.IdBedienerField;
            }
            set {
                if ((this.IdBedienerField.Equals(value) != true)) {
                    this.IdBedienerField = value;
                    this.RaisePropertyChanged("IdBediener");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid IdMaschine {
            get {
                return this.IdMaschineField;
            }
            set {
                if ((this.IdMaschineField.Equals(value) != true)) {
                    this.IdMaschineField = value;
                    this.RaisePropertyChanged("IdMaschine");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public JgLibHelper.ScannerProgram Program {
            get {
                return this.ProgramField;
            }
            set {
                if ((this.ProgramField.Equals(value) != true)) {
                    this.ProgramField = value;
                    this.RaisePropertyChanged("Program");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime ZeitMeldung {
            get {
                return this.ZeitMeldungField;
            }
            set {
                if ((this.ZeitMeldungField.Equals(value) != true)) {
                    this.ZeitMeldungField = value;
                    this.RaisePropertyChanged("ZeitMeldung");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="JgWcfBediener", Namespace="http://schemas.datacontract.org/2004/07/JgWcfServiceLib")]
    [System.SerializableAttribute()]
    public partial class JgWcfBediener : JgDienstScannerMaschine.ServiceRef.JgWcfBase {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NachnameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string VornameField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Nachname {
            get {
                return this.NachnameField;
            }
            set {
                if ((object.ReferenceEquals(this.NachnameField, value) != true)) {
                    this.NachnameField = value;
                    this.RaisePropertyChanged("Nachname");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Vorname {
            get {
                return this.VornameField;
            }
            set {
                if ((object.ReferenceEquals(this.VornameField, value) != true)) {
                    this.VornameField = value;
                    this.RaisePropertyChanged("Vorname");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="JgWcfMaschine", Namespace="http://schemas.datacontract.org/2004/07/JgWcfServiceLib")]
    [System.SerializableAttribute()]
    public partial class JgWcfMaschine : JgDienstScannerMaschine.ServiceRef.JgWcfBase {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string BemerkungField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private JgLibHelper.MaschinenArten MaschineArtField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MaschineIpField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MaschineNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int MaschinePortField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NummerScannerField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool SammelScannungField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool ScannerMitDisplayField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int VorschubProMeterInSekField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int ZeitProBauteilInSekField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int ZeitProBiegungInSekField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Bemerkung {
            get {
                return this.BemerkungField;
            }
            set {
                if ((object.ReferenceEquals(this.BemerkungField, value) != true)) {
                    this.BemerkungField = value;
                    this.RaisePropertyChanged("Bemerkung");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public JgLibHelper.MaschinenArten MaschineArt {
            get {
                return this.MaschineArtField;
            }
            set {
                if ((this.MaschineArtField.Equals(value) != true)) {
                    this.MaschineArtField = value;
                    this.RaisePropertyChanged("MaschineArt");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MaschineIp {
            get {
                return this.MaschineIpField;
            }
            set {
                if ((object.ReferenceEquals(this.MaschineIpField, value) != true)) {
                    this.MaschineIpField = value;
                    this.RaisePropertyChanged("MaschineIp");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MaschineName {
            get {
                return this.MaschineNameField;
            }
            set {
                if ((object.ReferenceEquals(this.MaschineNameField, value) != true)) {
                    this.MaschineNameField = value;
                    this.RaisePropertyChanged("MaschineName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int MaschinePort {
            get {
                return this.MaschinePortField;
            }
            set {
                if ((this.MaschinePortField.Equals(value) != true)) {
                    this.MaschinePortField = value;
                    this.RaisePropertyChanged("MaschinePort");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NummerScanner {
            get {
                return this.NummerScannerField;
            }
            set {
                if ((object.ReferenceEquals(this.NummerScannerField, value) != true)) {
                    this.NummerScannerField = value;
                    this.RaisePropertyChanged("NummerScanner");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool SammelScannung {
            get {
                return this.SammelScannungField;
            }
            set {
                if ((this.SammelScannungField.Equals(value) != true)) {
                    this.SammelScannungField = value;
                    this.RaisePropertyChanged("SammelScannung");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool ScannerMitDisplay {
            get {
                return this.ScannerMitDisplayField;
            }
            set {
                if ((this.ScannerMitDisplayField.Equals(value) != true)) {
                    this.ScannerMitDisplayField = value;
                    this.RaisePropertyChanged("ScannerMitDisplay");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int VorschubProMeterInSek {
            get {
                return this.VorschubProMeterInSekField;
            }
            set {
                if ((this.VorschubProMeterInSekField.Equals(value) != true)) {
                    this.VorschubProMeterInSekField = value;
                    this.RaisePropertyChanged("VorschubProMeterInSek");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ZeitProBauteilInSek {
            get {
                return this.ZeitProBauteilInSekField;
            }
            set {
                if ((this.ZeitProBauteilInSekField.Equals(value) != true)) {
                    this.ZeitProBauteilInSekField = value;
                    this.RaisePropertyChanged("ZeitProBauteilInSek");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ZeitProBiegungInSek {
            get {
                return this.ZeitProBiegungInSekField;
            }
            set {
                if ((this.ZeitProBiegungInSekField.Equals(value) != true)) {
                    this.ZeitProBiegungInSekField = value;
                    this.RaisePropertyChanged("ZeitProBiegungInSek");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="JgWcfBauteil", Namespace="http://schemas.datacontract.org/2004/07/JgWcfServiceLib")]
    [System.SerializableAttribute()]
    public partial class JgWcfBauteil : JgDienstScannerMaschine.ServiceRef.JgWcfBase {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int AnzahlBiegungenField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid BedienerField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int DuchmesserInMmField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<System.DateTime> EndeFertigungField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double GewichtInKgField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IdBauteilJgDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid IdMaschineField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int LaengeInCmField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Collections.Generic.List<System.Guid> ListeHelferField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime StartFertigungField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int AnzahlBiegungen {
            get {
                return this.AnzahlBiegungenField;
            }
            set {
                if ((this.AnzahlBiegungenField.Equals(value) != true)) {
                    this.AnzahlBiegungenField = value;
                    this.RaisePropertyChanged("AnzahlBiegungen");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid Bediener {
            get {
                return this.BedienerField;
            }
            set {
                if ((this.BedienerField.Equals(value) != true)) {
                    this.BedienerField = value;
                    this.RaisePropertyChanged("Bediener");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int DuchmesserInMm {
            get {
                return this.DuchmesserInMmField;
            }
            set {
                if ((this.DuchmesserInMmField.Equals(value) != true)) {
                    this.DuchmesserInMmField = value;
                    this.RaisePropertyChanged("DuchmesserInMm");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<System.DateTime> EndeFertigung {
            get {
                return this.EndeFertigungField;
            }
            set {
                if ((this.EndeFertigungField.Equals(value) != true)) {
                    this.EndeFertigungField = value;
                    this.RaisePropertyChanged("EndeFertigung");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double GewichtInKg {
            get {
                return this.GewichtInKgField;
            }
            set {
                if ((this.GewichtInKgField.Equals(value) != true)) {
                    this.GewichtInKgField = value;
                    this.RaisePropertyChanged("GewichtInKg");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string IdBauteilJgData {
            get {
                return this.IdBauteilJgDataField;
            }
            set {
                if ((object.ReferenceEquals(this.IdBauteilJgDataField, value) != true)) {
                    this.IdBauteilJgDataField = value;
                    this.RaisePropertyChanged("IdBauteilJgData");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid IdMaschine {
            get {
                return this.IdMaschineField;
            }
            set {
                if ((this.IdMaschineField.Equals(value) != true)) {
                    this.IdMaschineField = value;
                    this.RaisePropertyChanged("IdMaschine");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int LaengeInCm {
            get {
                return this.LaengeInCmField;
            }
            set {
                if ((this.LaengeInCmField.Equals(value) != true)) {
                    this.LaengeInCmField = value;
                    this.RaisePropertyChanged("LaengeInCm");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.Generic.List<System.Guid> ListeHelfer {
            get {
                return this.ListeHelferField;
            }
            set {
                if ((object.ReferenceEquals(this.ListeHelferField, value) != true)) {
                    this.ListeHelferField = value;
                    this.RaisePropertyChanged("ListeHelfer");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime StartFertigung {
            get {
                return this.StartFertigungField;
            }
            set {
                if ((this.StartFertigungField.Equals(value) != true)) {
                    this.StartFertigungField = value;
                    this.RaisePropertyChanged("StartFertigung");
                }
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceRef.IWcfService")]
    public interface IWcfService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfService/WcfTest", ReplyAction="http://tempuri.org/IWcfService/WcfTestResponse")]
        string WcfTest(string TestString);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfService/WcfTest", ReplyAction="http://tempuri.org/IWcfService/WcfTestResponse")]
        System.Threading.Tasks.Task<string> WcfTestAsync(string TestString);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfService/SendeBauteil", ReplyAction="http://tempuri.org/IWcfService/SendeBauteilResponse")]
        bool SendeBauteil(JgDienstScannerMaschine.ServiceRef.JgWcfBauteil Bauteil);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfService/SendeBauteil", ReplyAction="http://tempuri.org/IWcfService/SendeBauteilResponse")]
        System.Threading.Tasks.Task<bool> SendeBauteilAsync(JgDienstScannerMaschine.ServiceRef.JgWcfBauteil Bauteil);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfService/SendeMeldung", ReplyAction="http://tempuri.org/IWcfService/SendeMeldungResponse")]
        bool SendeMeldung(JgDienstScannerMaschine.ServiceRef.JgWcfMeldung Meldung);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfService/SendeMeldung", ReplyAction="http://tempuri.org/IWcfService/SendeMeldungResponse")]
        System.Threading.Tasks.Task<bool> SendeMeldungAsync(JgDienstScannerMaschine.ServiceRef.JgWcfMeldung Meldung);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfService/GetBediener", ReplyAction="http://tempuri.org/IWcfService/GetBedienerResponse")]
        System.Collections.Generic.List<JgDienstScannerMaschine.ServiceRef.JgWcfBediener> GetBediener();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfService/GetBediener", ReplyAction="http://tempuri.org/IWcfService/GetBedienerResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<JgDienstScannerMaschine.ServiceRef.JgWcfBediener>> GetBedienerAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfService/GetMaschinen", ReplyAction="http://tempuri.org/IWcfService/GetMaschinenResponse")]
        System.Collections.Generic.List<JgDienstScannerMaschine.ServiceRef.JgWcfMaschine> GetMaschinen(System.Guid IdStandort);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfService/GetMaschinen", ReplyAction="http://tempuri.org/IWcfService/GetMaschinenResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<JgDienstScannerMaschine.ServiceRef.JgWcfMaschine>> GetMaschinenAsync(System.Guid IdStandort);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfService/SendeMaschinenStatus", ReplyAction="http://tempuri.org/IWcfService/SendeMaschinenStatusResponse")]
        bool SendeMaschinenStatus(JgDienstScannerMaschine.ServiceRef.JgWcfMaschine Maschine);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfService/SendeMaschinenStatus", ReplyAction="http://tempuri.org/IWcfService/SendeMaschinenStatusResponse")]
        System.Threading.Tasks.Task<bool> SendeMaschinenStatusAsync(JgDienstScannerMaschine.ServiceRef.JgWcfMaschine Maschine);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IWcfServiceChannel : JgDienstScannerMaschine.ServiceRef.IWcfService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WcfServiceClient : System.ServiceModel.ClientBase<JgDienstScannerMaschine.ServiceRef.IWcfService>, JgDienstScannerMaschine.ServiceRef.IWcfService {
        
        public WcfServiceClient() {
        }
        
        public WcfServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WcfServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WcfServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WcfServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string WcfTest(string TestString) {
            return base.Channel.WcfTest(TestString);
        }
        
        public System.Threading.Tasks.Task<string> WcfTestAsync(string TestString) {
            return base.Channel.WcfTestAsync(TestString);
        }
        
        public bool SendeBauteil(JgDienstScannerMaschine.ServiceRef.JgWcfBauteil Bauteil) {
            return base.Channel.SendeBauteil(Bauteil);
        }
        
        public System.Threading.Tasks.Task<bool> SendeBauteilAsync(JgDienstScannerMaschine.ServiceRef.JgWcfBauteil Bauteil) {
            return base.Channel.SendeBauteilAsync(Bauteil);
        }
        
        public bool SendeMeldung(JgDienstScannerMaschine.ServiceRef.JgWcfMeldung Meldung) {
            return base.Channel.SendeMeldung(Meldung);
        }
        
        public System.Threading.Tasks.Task<bool> SendeMeldungAsync(JgDienstScannerMaschine.ServiceRef.JgWcfMeldung Meldung) {
            return base.Channel.SendeMeldungAsync(Meldung);
        }
        
        public System.Collections.Generic.List<JgDienstScannerMaschine.ServiceRef.JgWcfBediener> GetBediener() {
            return base.Channel.GetBediener();
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.List<JgDienstScannerMaschine.ServiceRef.JgWcfBediener>> GetBedienerAsync() {
            return base.Channel.GetBedienerAsync();
        }
        
        public System.Collections.Generic.List<JgDienstScannerMaschine.ServiceRef.JgWcfMaschine> GetMaschinen(System.Guid IdStandort) {
            return base.Channel.GetMaschinen(IdStandort);
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.List<JgDienstScannerMaschine.ServiceRef.JgWcfMaschine>> GetMaschinenAsync(System.Guid IdStandort) {
            return base.Channel.GetMaschinenAsync(IdStandort);
        }
        
        public bool SendeMaschinenStatus(JgDienstScannerMaschine.ServiceRef.JgWcfMaschine Maschine) {
            return base.Channel.SendeMaschinenStatus(Maschine);
        }
        
        public System.Threading.Tasks.Task<bool> SendeMaschinenStatusAsync(JgDienstScannerMaschine.ServiceRef.JgWcfMaschine Maschine) {
            return base.Channel.SendeMaschinenStatusAsync(Maschine);
        }
    }
}