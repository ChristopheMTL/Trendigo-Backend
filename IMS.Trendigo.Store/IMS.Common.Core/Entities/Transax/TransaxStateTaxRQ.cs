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
    public partial class TransaxStateTaxRQ
    {

        private string idField;

        private string entityIdField;

        private string providerIdField;

        private string statusField;

        private string nameField;

        private string percentField;

        private string absoluteField;

        private string minimumField;

        private string defaultField;

        private string priorityField;

        private string regionField;

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
        public string entityId
        {
            get
            {
                return this.entityIdField;
            }
            set
            {
                this.entityIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string providerId
        {
            get
            {
                return this.providerIdField;
            }
            set
            {
                this.providerIdField = value;
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
        public string percent
        {
            get
            {
                return this.percentField;
            }
            set
            {
                this.percentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string absolute
        {
            get
            {
                return this.absoluteField;
            }
            set
            {
                this.absoluteField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string minimum
        {
            get
            {
                return this.minimumField;
            }
            set
            {
                this.minimumField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string @default
        {
            get
            {
                return this.defaultField;
            }
            set
            {
                this.defaultField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string priority
        {
            get
            {
                return this.priorityField;
            }
            set
            {
                this.priorityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string region
        {
            get
            {
                return this.regionField;
            }
            set
            {
                this.regionField = value;
            }
        }
    }
}
