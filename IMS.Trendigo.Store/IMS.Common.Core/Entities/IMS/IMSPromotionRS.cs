using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using IMS.Common.Core.Data;

namespace IMS.Common.Core.Entities.IMS
{
    [DataContract]
    public class IMSPromotionRS
    {
        private string codeField;
        private string messageField;
        private Promotion UserField;

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
        [DataMember(Name = "promotion", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute("Promotion", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public Promotion Promotion
        {
            get
            {
                return this.UserField;
            }
            set
            {
                this.UserField = value;
            }
        }
    }
}
