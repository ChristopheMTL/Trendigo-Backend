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
    [System.Xml.Serialization.XmlRoot("TransaxLocationRS")]
    public partial class TransaxLocationRS
    {

        private TransaxLocations[] itemsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Locations", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TransaxLocations[] Items
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
    public partial class TransaxLocations
    {

        private TransaxLocation locationField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Location", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TransaxLocation Location
        {
            get
            {
                return this.locationField;
            }
            set
            {
                this.locationField = value;
            }
        }
    }
}
