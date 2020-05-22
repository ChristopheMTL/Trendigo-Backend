using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Entities.Transax
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TransaxInsideRep
    {

        private string idField;

        private string sponsorIdField;

        private string userIdField;

        private string commisionPerRegistryField;

        private string commisionPerTransactionLoyaltyField;

        private string commisionPerIwalletTransactionField;

        private string commisionPerGiftTransactionField;

        private string commisionPerPointDistributedField;

        private string commisionPerPointAcquiredField;

        private string commisionPerGiftAmountField;

        private string statusField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string sponsorId
        {
            get
            {
                return this.sponsorIdField;
            }
            set
            {
                this.sponsorIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string userId
        {
            get
            {
                return this.userIdField;
            }
            set
            {
                this.userIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string commisionPerRegistry
        {
            get
            {
                return this.commisionPerRegistryField;
            }
            set
            {
                this.commisionPerRegistryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string commisionPerTransactionLoyalty
        {
            get
            {
                return this.commisionPerTransactionLoyaltyField;
            }
            set
            {
                this.commisionPerTransactionLoyaltyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string commisionPerIwalletTransaction
        {
            get
            {
                return this.commisionPerIwalletTransactionField;
            }
            set
            {
                this.commisionPerIwalletTransactionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string commisionPerGiftTransaction
        {
            get
            {
                return this.commisionPerGiftTransactionField;
            }
            set
            {
                this.commisionPerGiftTransactionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string commisionPerPointDistributed
        {
            get
            {
                return this.commisionPerPointDistributedField;
            }
            set
            {
                this.commisionPerPointDistributedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string commisionPerPointAcquired
        {
            get
            {
                return this.commisionPerPointAcquiredField;
            }
            set
            {
                this.commisionPerPointAcquiredField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string commisionPerGiftAmount
        {
            get
            {
                return this.commisionPerGiftAmountField;
            }
            set
            {
                this.commisionPerGiftAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
    }
}
