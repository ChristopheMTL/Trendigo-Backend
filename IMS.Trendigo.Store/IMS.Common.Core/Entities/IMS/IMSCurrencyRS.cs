using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using IMS.Common.Core.Data;

namespace IMS.Common.Core.Entities.IMS
{
    public class IMSCurrencyRS
    {
        private string codeField;
        private string messageField;
        private Currency currencyField;

        /// <remarks/>
        [DataMember(Name = "code", Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("Code", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [DataMember(Name = "message", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute("Message", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <remarks/>
        [DataMember(Name = "currency", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute("Currency", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public Currency Currency
        {
            get
            {
                return this.currencyField;
            }
            set
            {
                this.currencyField = value;
            }
        }
    }
}
