using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Common.Core.Entities.Transax;
using IMS.Common.Core.Entities.IMS.Interface;

namespace IMS.Common.Core.Entities.IMS
{
    public class IMSUser : TransaxUser, IIMSResponse
    {
        private List<TransaxSponsor> _Sponsors;
        private List<TransaxMerchant> _Merchants;
        private string codeField;
        private string messageField;
        private int enterpriseField;

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

        public int EnterpriseId
        {
            get
            {
                return this.enterpriseField;
            }
            set
            {
                this.enterpriseField = value;
            }
        }

        public List<TransaxSponsor> TransaxSponsors
        {
            get
            {
                return this._Sponsors;
            }
            set
            {
                this._Sponsors = value;
            }
        }

        public List<TransaxMerchant> TransaxMerchants
        {
            get
            {
                return this._Merchants;
            }
            set
            {
                this._Merchants = value;
            }
        }
    }
}
