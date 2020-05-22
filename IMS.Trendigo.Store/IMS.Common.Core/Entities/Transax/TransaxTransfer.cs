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
    public partial class TransaxTransfer
    {

        private string cardNoFinantialProgramIdSourceField;

        private string cardNoFinantialProgramIdDestinationField;

        private string typeField;

        private string amountField;

        private string entityIdField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string cardNoFinantialProgramIdSource
        {
            get
            {
                return this.cardNoFinantialProgramIdSourceField;
            }
            set
            {
                this.cardNoFinantialProgramIdSourceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string cardNoFinantialProgramIdDestination
        {
            get
            {
                return this.cardNoFinantialProgramIdDestinationField;
            }
            set
            {
                this.cardNoFinantialProgramIdDestinationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string amount
        {
            get
            {
                return this.amountField;
            }
            set
            {
                this.amountField = value;
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
    }
}


