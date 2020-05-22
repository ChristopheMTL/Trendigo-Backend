using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Entities.Transax
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRoot("TransaxSponsor")]
    public partial class TransaxSponsor
    {

        private string idField;

        private string nameField;

        private string enterpriseIdField;

        private string adminField;

        private string pricePerTransactionField;

        private string pricePerTransactionPercentField;

        private string pricePerPointDistributedField;

        private string pricePerPointAcquiredField;

        private string imageField;

        private string rootPasswordField;

        private string statusField;

        private string sloganField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string enterpriseId
        {
            get
            {
                return this.enterpriseIdField;
            }
            set
            {
                this.enterpriseIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string admin
        {
            get
            {
                return this.adminField;
            }
            set
            {
                this.adminField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string pricePerTransaction
        {
            get
            {
                return this.pricePerTransactionField;
            }
            set
            {
                this.pricePerTransactionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string pricePerTransactionPercent
        {
            get
            {
                return this.pricePerTransactionPercentField;
            }
            set
            {
                this.pricePerTransactionPercentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string pricePerPointDistributed
        {
            get
            {
                return this.pricePerPointDistributedField;
            }
            set
            {
                this.pricePerPointDistributedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string pricePerPointAcquired
        {
            get
            {
                return this.pricePerPointAcquiredField;
            }
            set
            {
                this.pricePerPointAcquiredField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string image
        {
            get
            {
                return this.imageField;
            }
            set
            {
                this.imageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string rootPassword
        {
            get
            {
                return this.rootPasswordField;
            }
            set
            {
                this.rootPasswordField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string slogan
        {
            get
            {
                return this.sloganField;
            }
            set
            {
                this.sloganField = value;
            }
        }
    }
}
