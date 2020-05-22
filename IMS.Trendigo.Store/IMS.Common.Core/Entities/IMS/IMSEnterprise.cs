using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Common.Core.Entities.Transax;
using IMS.Common.Core.Entities.IMS.Interface;

namespace IMS.Common.Core.Entities.IMS
{
    public class IMSEnterprise : TransaxEnterprise
    {
        private List<TransaxEnterprise> _Enterprises;
        private string codeField;
        private string messageField;

        public string IMSCode
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

        public string IMSMessage
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

        public List<TransaxEnterprise> Enterprises
        {
            get
            {
                return this._Enterprises;
            }
            set
            {
                this._Enterprises = value;
            }
        }
    }
}
