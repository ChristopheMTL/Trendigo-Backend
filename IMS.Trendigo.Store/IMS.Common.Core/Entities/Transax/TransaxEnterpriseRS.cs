using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Common.Core.Data;

namespace IMS.Common.Core.Entities.Transax
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRoot("TransaxEnterpriseRS")]
    public partial class TransaxEnterpriseRS
    {

        private TransaxEnterprises[] itemsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Enterprises", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TransaxEnterprises[] Items
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
    public partial class TransaxEnterprises
    {

        private TransaxEnterprise enterpriseField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Enterprise", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TransaxEnterprise Enterprise
        {
            get
            {
                return this.enterpriseField;
            }
            set
            {
                this.enterpriseField = value;
            }
        }
    }
}
