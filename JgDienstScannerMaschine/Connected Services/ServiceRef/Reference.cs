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
    [System.Runtime.Serialization.DataContractAttribute(Name="JgWcfStamm", Namespace="http://schemas.datacontract.org/2004/07/JgWcfServiceLib")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(JgDienstScannerMaschine.ServiceRef.JgWcfMeldung))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(JgDienstScannerMaschine.ServiceRef.JgWcfMaschinenanmeldung))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(JgDienstScannerMaschine.ServiceRef.JgWcfBauteil))]
    public partial class JgWcfStamm : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IdMaschineField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime ZeitField;
        
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
        public int IdMaschine {
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
        public System.DateTime Zeit {
            get {
                return this.ZeitField;
            }
            set {
                if ((this.ZeitField.Equals(value) != true)) {
                    this.ZeitField = value;
                    this.RaisePropertyChanged("Zeit");
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
    public partial class JgWcfMeldung : JgDienstScannerMaschine.ServiceRef.JgWcfStamm {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool AnzahlField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private JgDienstScannerMaschine.ServiceRef.JgWcfMeldung.EnumMeldungen MeldungField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Anzahl {
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
        public JgDienstScannerMaschine.ServiceRef.JgWcfMeldung.EnumMeldungen Meldung {
            get {
                return this.MeldungField;
            }
            set {
                if ((this.MeldungField.Equals(value) != true)) {
                    this.MeldungField = value;
                    this.RaisePropertyChanged("Meldung");
                }
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
        [System.Runtime.Serialization.DataContractAttribute(Name="JgWcfMeldung.EnumMeldungen", Namespace="http://schemas.datacontract.org/2004/07/JgWcfServiceLib")]
        public enum EnumMeldungen : int {
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            ReparaturStart = 0,
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            ReparaturEnde = 1,
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            CoilwechselStart = 2,
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            CoilwechselEnde = 3,
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="JgWcfMaschinenanmeldung", Namespace="http://schemas.datacontract.org/2004/07/JgWcfServiceLib")]
    [System.SerializableAttribute()]
    public partial class JgWcfMaschinenanmeldung : JgDienstScannerMaschine.ServiceRef.JgWcfStamm {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool AnmeldungField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IdBenutzerField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Anmeldung {
            get {
                return this.AnmeldungField;
            }
            set {
                if ((this.AnmeldungField.Equals(value) != true)) {
                    this.AnmeldungField = value;
                    this.RaisePropertyChanged("Anmeldung");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int IdBenutzer {
            get {
                return this.IdBenutzerField;
            }
            set {
                if ((this.IdBenutzerField.Equals(value) != true)) {
                    this.IdBenutzerField = value;
                    this.RaisePropertyChanged("IdBenutzer");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="JgWcfBauteil", Namespace="http://schemas.datacontract.org/2004/07/JgWcfServiceLib")]
    [System.SerializableAttribute()]
    public partial class JgWcfBauteil : JgDienstScannerMaschine.ServiceRef.JgWcfStamm {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int AnzahlField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double GewichtField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IdBauteilField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Anzahl {
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
        public double Gewicht {
            get {
                return this.GewichtField;
            }
            set {
                if ((this.GewichtField.Equals(value) != true)) {
                    this.GewichtField = value;
                    this.RaisePropertyChanged("Gewicht");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int IdBauteil {
            get {
                return this.IdBauteilField;
            }
            set {
                if ((this.IdBauteilField.Equals(value) != true)) {
                    this.IdBauteilField = value;
                    this.RaisePropertyChanged("IdBauteil");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="JgDbBediener", Namespace="http://schemas.datacontract.org/2004/07/JgWcfServiceLib")]
    [System.SerializableAttribute()]
    public partial class JgDbBediener : JgDienstScannerMaschine.ServiceRef.JgDbStamm {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string BedienerNameField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string BedienerName {
            get {
                return this.BedienerNameField;
            }
            set {
                if ((object.ReferenceEquals(this.BedienerNameField, value) != true)) {
                    this.BedienerNameField = value;
                    this.RaisePropertyChanged("BedienerName");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="JgDbStamm", Namespace="http://schemas.datacontract.org/2004/07/JgWcfServiceLib")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(JgDienstScannerMaschine.ServiceRef.JgDbMaschine))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(JgDienstScannerMaschine.ServiceRef.JgWcfMaschine))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(JgDienstScannerMaschine.ServiceRef.JgDbBediener))]
    public partial class JgDbStamm : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
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
    [System.Runtime.Serialization.DataContractAttribute(Name="JgDbMaschine", Namespace="http://schemas.datacontract.org/2004/07/JgWcfServiceLib")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(JgDienstScannerMaschine.ServiceRef.JgWcfMaschine))]
    public partial class JgDbMaschine : JgDienstScannerMaschine.ServiceRef.JgDbStamm {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private JgLibHelper.MaschinenArten MaschineArtField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MaschineIpField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MaschineNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int MaschinePortField;
        
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
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="JgWcfMaschine", Namespace="http://schemas.datacontract.org/2004/07/JgWcfServiceLib")]
    [System.SerializableAttribute()]
    public partial class JgWcfMaschine : JgDienstScannerMaschine.ServiceRef.JgDbMaschine {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Collections.Generic.List<JgDienstScannerMaschine.ServiceRef.JgWcfBauteil> BauteileField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private JgDienstScannerMaschine.ServiceRef.JgDbBediener BedienerField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Collections.Generic.List<JgDienstScannerMaschine.ServiceRef.JgDbBediener> HelferField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<System.Guid> IdBedienerField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Collections.Generic.List<System.Guid> IdisBauteileField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Collections.Generic.List<System.Guid> IdisHelferField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private JgLibHelper.StatusMaschine StatusField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.Generic.List<JgDienstScannerMaschine.ServiceRef.JgWcfBauteil> Bauteile {
            get {
                return this.BauteileField;
            }
            set {
                if ((object.ReferenceEquals(this.BauteileField, value) != true)) {
                    this.BauteileField = value;
                    this.RaisePropertyChanged("Bauteile");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public JgDienstScannerMaschine.ServiceRef.JgDbBediener Bediener {
            get {
                return this.BedienerField;
            }
            set {
                if ((object.ReferenceEquals(this.BedienerField, value) != true)) {
                    this.BedienerField = value;
                    this.RaisePropertyChanged("Bediener");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.Generic.List<JgDienstScannerMaschine.ServiceRef.JgDbBediener> Helfer {
            get {
                return this.HelferField;
            }
            set {
                if ((object.ReferenceEquals(this.HelferField, value) != true)) {
                    this.HelferField = value;
                    this.RaisePropertyChanged("Helfer");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<System.Guid> IdBediener {
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
        public System.Collections.Generic.List<System.Guid> IdisBauteile {
            get {
                return this.IdisBauteileField;
            }
            set {
                if ((object.ReferenceEquals(this.IdisBauteileField, value) != true)) {
                    this.IdisBauteileField = value;
                    this.RaisePropertyChanged("IdisBauteile");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.Generic.List<System.Guid> IdisHelfer {
            get {
                return this.IdisHelferField;
            }
            set {
                if ((object.ReferenceEquals(this.IdisHelferField, value) != true)) {
                    this.IdisHelferField = value;
                    this.RaisePropertyChanged("IdisHelfer");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public JgLibHelper.StatusMaschine Status {
            get {
                return this.StatusField;
            }
            set {
                if ((this.StatusField.Equals(value) != true)) {
                    this.StatusField = value;
                    this.RaisePropertyChanged("Status");
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
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfService/SendeMaschinenanmeldung", ReplyAction="http://tempuri.org/IWcfService/SendeMaschinenanmeldungResponse")]
        bool SendeMaschinenanmeldung(JgDienstScannerMaschine.ServiceRef.JgWcfMaschinenanmeldung Anmeldung);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfService/SendeMaschinenanmeldung", ReplyAction="http://tempuri.org/IWcfService/SendeMaschinenanmeldungResponse")]
        System.Threading.Tasks.Task<bool> SendeMaschinenanmeldungAsync(JgDienstScannerMaschine.ServiceRef.JgWcfMaschinenanmeldung Anmeldung);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfService/GetBediener", ReplyAction="http://tempuri.org/IWcfService/GetBedienerResponse")]
        System.Collections.Generic.List<JgDienstScannerMaschine.ServiceRef.JgDbBediener> GetBediener();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfService/GetBediener", ReplyAction="http://tempuri.org/IWcfService/GetBedienerResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<JgDienstScannerMaschine.ServiceRef.JgDbBediener>> GetBedienerAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfService/GetMaschinen", ReplyAction="http://tempuri.org/IWcfService/GetMaschinenResponse")]
        System.Collections.Generic.List<JgDienstScannerMaschine.ServiceRef.JgDbMaschine> GetMaschinen(System.Guid IdStandort);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfService/GetMaschinen", ReplyAction="http://tempuri.org/IWcfService/GetMaschinenResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<JgDienstScannerMaschine.ServiceRef.JgDbMaschine>> GetMaschinenAsync(System.Guid IdStandort);
        
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
        
        public bool SendeMaschinenanmeldung(JgDienstScannerMaschine.ServiceRef.JgWcfMaschinenanmeldung Anmeldung) {
            return base.Channel.SendeMaschinenanmeldung(Anmeldung);
        }
        
        public System.Threading.Tasks.Task<bool> SendeMaschinenanmeldungAsync(JgDienstScannerMaschine.ServiceRef.JgWcfMaschinenanmeldung Anmeldung) {
            return base.Channel.SendeMaschinenanmeldungAsync(Anmeldung);
        }
        
        public System.Collections.Generic.List<JgDienstScannerMaschine.ServiceRef.JgDbBediener> GetBediener() {
            return base.Channel.GetBediener();
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.List<JgDienstScannerMaschine.ServiceRef.JgDbBediener>> GetBedienerAsync() {
            return base.Channel.GetBedienerAsync();
        }
        
        public System.Collections.Generic.List<JgDienstScannerMaschine.ServiceRef.JgDbMaschine> GetMaschinen(System.Guid IdStandort) {
            return base.Channel.GetMaschinen(IdStandort);
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.List<JgDienstScannerMaschine.ServiceRef.JgDbMaschine>> GetMaschinenAsync(System.Guid IdStandort) {
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
