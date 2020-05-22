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
    [System.Xml.Serialization.XmlRoot("TransaxMerchantRS")]
    public partial class TransaxMerchantRS
    {

        private TransaxMerchants[] itemsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Merchants", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TransaxMerchants[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TransaxMerchants
    {

        private TransaxMerchant[] merchantField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Merchant", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TransaxMerchant[] Merchant
        {
            get
            {
                return this.merchantField;
            }
            set
            {
                this.merchantField = value;
            }
        }
    }
}
