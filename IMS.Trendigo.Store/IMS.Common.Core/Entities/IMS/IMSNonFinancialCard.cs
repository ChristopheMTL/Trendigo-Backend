using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Common.Core.Entities.Transax;
using IMS.Common.Core.Entities.IMS.Interface;

namespace IMS.Common.Core.Entities.IMS
{
    public class IMSNonFinancialCard : TransaxNonFinancialCard, IIMSResponse
    {
        private TransaxNonFinancialCard _card;
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

        public TransaxNonFinancialCard card
        {
            get
            {
                return this._card;
            }
            set
            {
                this._card = value;
            }
        }
    }
}
