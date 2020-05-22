using IMS.Common.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Entities.IMS
{
    public class IMSProgramRS
    {
        private string codeField;
        private string messageField;
        private Program programField;

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
        [DataMember(Name = "program", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute("Program", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public Program Program
        {
            get
            {
                return this.programField;
            }
            set
            {
                this.programField = value;
            }
        }
    }
}
