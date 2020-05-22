using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Common.Core.Entities.Transax;
using IMS.Common.Core.Entities.IMS.Interface;

namespace IMS.Common.Core.Entities.IMS
{
    public class IMSTransaction : TransaxTransaction
    {
        private string codeField;
        private string messageField;

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
    }
}
