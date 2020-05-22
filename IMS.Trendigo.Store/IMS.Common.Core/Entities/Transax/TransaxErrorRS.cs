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
    [System.Xml.Serialization.XmlRoot("TransaxRS")]
    public partial class TransaxErrorRS
    {

        private string restcodeField;

        private string restmessageField;

        

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string restcode
        {
            get
            {
                return this.restcodeField;
            }
            set
            {
                this.restcodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string restmessage
        {
            get
            {
                return this.restmessageField;
            }
            set
            {
                this.restmessageField = value;
            }
        }
    }
}
