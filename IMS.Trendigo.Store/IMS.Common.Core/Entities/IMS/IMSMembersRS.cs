using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace IMS.Common.Core.Entities.IMS
{
    [DataContract]
    public class IMSMembersRS
    {
        private string codeField;
        private string messageField;
        private List<IMSMember> UserField;

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
        [DataMember(Name = "members", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute("Members", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public List<IMSMember> Members
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
