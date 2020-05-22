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
    public class IMSUserRS
    {
        private string codeField;
        private string messageField;
        private Data.IMSUser UserField;

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
        [DataMember(Name = "user", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute("User", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public Data.IMSUser User
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
