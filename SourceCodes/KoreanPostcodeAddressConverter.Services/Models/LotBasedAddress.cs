﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.5466
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=2.0.50727.3038.
// 
namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Models {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://aliencube.org/schemas/2013/05/lot-based-address")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://aliencube.org/schemas/2013/05/lot-based-address", IsNullable=false)]
    public partial class LotBasedAddresses {
        
        private LotBasedAddress[] lotBasedAddressField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("LotBasedAddress")]
        public LotBasedAddress[] LotBasedAddress {
            get {
                return this.lotBasedAddressField;
            }
            set {
                this.lotBasedAddressField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://aliencube.org/schemas/2013/05/lot-based-address")]
    public partial class LotBasedAddress {
        
        private string postcodeField;
        
        private int sequenceNumberField;
        
        private string provinceField;
        
        private string countyField;
        
        private string suburbField;
        
        private string villageField;
        
        private string islandField;
        
        private bool sanField;
        
        private System.Nullable<int> lotNumberMajorFromField;
        
        private System.Nullable<int> lotNumberMinorFromField;
        
        private System.Nullable<int> lotNumberMajorToField;
        
        private System.Nullable<int> lotNumberMinorToField;
        
        private string buildingNameField;
        
        private System.Nullable<int> buildingNumberFromField;
        
        private System.Nullable<int> buildingNumberToField;
        
        private System.DateTime dateUpdatedField;
        
        private string addressField;
        
        /// <remarks/>
        public string Postcode {
            get {
                return this.postcodeField;
            }
            set {
                this.postcodeField = value;
            }
        }
        
        /// <remarks/>
        public int SequenceNumber {
            get {
                return this.sequenceNumberField;
            }
            set {
                this.sequenceNumberField = value;
            }
        }
        
        /// <remarks/>
        public string Province {
            get {
                return this.provinceField;
            }
            set {
                this.provinceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string County {
            get {
                return this.countyField;
            }
            set {
                this.countyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Suburb {
            get {
                return this.suburbField;
            }
            set {
                this.suburbField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Village {
            get {
                return this.villageField;
            }
            set {
                this.villageField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Island {
            get {
                return this.islandField;
            }
            set {
                this.islandField = value;
            }
        }
        
        /// <remarks/>
        public bool San {
            get {
                return this.sanField;
            }
            set {
                this.sanField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<int> LotNumberMajorFrom {
            get {
                return this.lotNumberMajorFromField;
            }
            set {
                this.lotNumberMajorFromField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<int> LotNumberMinorFrom {
            get {
                return this.lotNumberMinorFromField;
            }
            set {
                this.lotNumberMinorFromField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<int> LotNumberMajorTo {
            get {
                return this.lotNumberMajorToField;
            }
            set {
                this.lotNumberMajorToField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<int> LotNumberMinorTo {
            get {
                return this.lotNumberMinorToField;
            }
            set {
                this.lotNumberMinorToField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string BuildingName {
            get {
                return this.buildingNameField;
            }
            set {
                this.buildingNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<int> BuildingNumberFrom {
            get {
                return this.buildingNumberFromField;
            }
            set {
                this.buildingNumberFromField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<int> BuildingNumberTo {
            get {
                return this.buildingNumberToField;
            }
            set {
                this.buildingNumberToField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date")]
        public System.DateTime DateUpdated {
            get {
                return this.dateUpdatedField;
            }
            set {
                this.dateUpdatedField = value;
            }
        }
        
        /// <remarks/>
        public string Address {
            get {
                return this.addressField;
            }
            set {
                this.addressField = value;
            }
        }
    }
}
