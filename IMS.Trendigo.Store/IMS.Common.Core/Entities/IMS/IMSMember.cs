    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace IMS.Common.Core.Entities.IMS
{
    public class IMSMember
    {

        private string _memberId;
        private string _transaxId;
        private string _enterpriseId;
        private string _userId;
        private string _statusId;
        private int _roleId;
        private string _gender;
        private int _ageRangeId;
        private string _firstName;
        private string _lastName;
        private string _language;
        private string _email;
        private bool _newsletter;
        private bool _alert;
        private string _streetAddress;
        private int _cityId;
        private int _stateId;
        private int _countryId;
        private string _zip;

        public string memberId
        {
            get
            {
                return this._memberId;
            }
            set
            {
                this._memberId = value;
            }
        }

        public string transaxId
        {
            get
            {
                return this._transaxId;
            }
            set
            {
                this._transaxId = value;
            }
        }

        public string enterpriseId
        {
            get
            {
                return this._enterpriseId;
            }
            set
            {
                this._enterpriseId = value;
            }
        }

        public string userId
        {
            get
            {
                return this._userId;
            }
            set
            {
                this._userId = value;
            }
        }

        public string statusId
        {
            get
            {
                return this._statusId;
            }
            set
            {
                this._statusId = value;
            }
        }

        public int roleId
        {
            get
            {
                return this._roleId;
            }
            set
            {
                this._roleId = value;
            }
        }

        public string gender
        {
            get
            {
                return this._gender;
            }
            set
            {
                this._gender = value;
            }
        }

        public int ageRangeId
        {
            get
            {
                return this._ageRangeId;
            }
            set
            {
                this._ageRangeId = value;
            }
        }

        public string firstName
        {
            get
            {
                return this._firstName;
            }
            set
            {
                this._firstName = value;
            }
        }

        public string lastName
        {
            get
            {
                return this._lastName;
            }
            set
            {
                this._lastName = value;
            }
        }

        public string language
        {
            get
            {
                return this._language;
            }
            set
            {
                this._language = value;
            }
        }

        public string email
        {
            get
            {
                return this._email;
            }
            set
            {
                this._email = value;
            }
        }

        public bool newsLetter
        {
            get
            {
                return this._newsletter;
            }
            set
            {
                this._newsletter = value;
            }
        }

        public bool alert
        {
            get
            {
                return this._alert;
            }
            set
            {
                this._alert = value;
            }
        }

        public string streetAddress
        {
            get
            {
                return this._streetAddress;
            }
            set
            {
                this._streetAddress = value;
            }
        }

        public int countryId
        {
            get
            {
                return this._countryId;
            }
            set
            {
                this._countryId = value;
            }
        }

        public int stateId
        {
            get
            {
                return this._stateId;
            }
            set
            {
                this._stateId = value;
            }
        }

        public int cityId
        {
            get
            {
                return this._cityId;
            }
            set
            {
                this._cityId = value;
            }
        }

        public string zip
        {
            get
            {
                return this._zip;
            }
            set
            {
                this._zip = value;
            }
        }
    }
}
