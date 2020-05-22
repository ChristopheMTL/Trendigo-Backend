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
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class TransaxContractRS
    {

        private TransaxRSContracts[] itemsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Contracts", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TransaxRSContracts[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TransaxRSContracts
    {

        private TransaxRSContractsContract[] contractField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Contract", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TransaxRSContractsContract[] Contract
        {
            get
            {
                return this.contractField;
            }
            set
            {
                this.contractField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TransaxRSContractsContract
    {

        private string cardNonFinancialProgranIdField;

        private string programIdField;

        private string cardNonFinancialIdField;

        private string startDateField;

        private string endDateField;

        private string commentsField;

        private string statusField;

        private string priceField;

        private string financialTransactionField;

        private string pointsBalanceField;

        private string programLevelField;

        private string financialCardAssociatedField;

        private string giftBalanceField;

        private string ewalletBalanceField;

        private string outSidePartnerField;

        private string outSideUserContractIdField;

        private string commisionPerRegistryField;

        private string commisionPerTransactionLoyaltyField;

        private string commisionPerEwalletTransactionField;

        private string commisionPerGiftTransactionField;

        private string commisionPerPointDistributedField;

        private string commisionPerGiftAmountField;

        private string commisionPerPointAcquiredField;

        private string cardIdField;

        private string statusOutSideUserContractField;

        private string cardNonFinancialField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string cardNonFinancialProgranId
        {
            get
            {
                return this.cardNonFinancialProgranIdField;
            }
            set
            {
                this.cardNonFinancialProgranIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string programId
        {
            get
            {
                return this.programIdField;
            }
            set
            {
                this.programIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string cardNonFinancialId
        {
            get
            {
                return this.cardNonFinancialIdField;
            }
            set
            {
                this.cardNonFinancialIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string startDate
        {
            get
            {
                return this.startDateField;
            }
            set
            {
                this.startDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string endDate
        {
            get
            {
                return this.endDateField;
            }
            set
            {
                this.endDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string comments
        {
            get
            {
                return this.commentsField;
            }
            set
            {
                this.commentsField = value;
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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string price
        {
            get
            {
                return this.priceField;
            }
            set
            {
                this.priceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string financialTransaction
        {
            get
            {
                return this.financialTransactionField;
            }
            set
            {
                this.financialTransactionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string pointsBalance
        {
            get
            {
                return this.pointsBalanceField;
            }
            set
            {
                this.pointsBalanceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string programLevel
        {
            get
            {
                return this.programLevelField;
            }
            set
            {
                this.programLevelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string financialCardAssociated
        {
            get
            {
                return this.financialCardAssociatedField;
            }
            set
            {
                this.financialCardAssociatedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string giftBalance
        {
            get
            {
                return this.giftBalanceField;
            }
            set
            {
                this.giftBalanceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ewalletBalance
        {
            get
            {
                return this.ewalletBalanceField;
            }
            set
            {
                this.ewalletBalanceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string outSidePartner
        {
            get
            {
                return this.outSidePartnerField;
            }
            set
            {
                this.outSidePartnerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string outSideUserContractId
        {
            get
            {
                return this.outSideUserContractIdField;
            }
            set
            {
                this.outSideUserContractIdField = value;
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
        public string commisionPerEwalletTransaction
        {
            get
            {
                return this.commisionPerEwalletTransactionField;
            }
            set
            {
                this.commisionPerEwalletTransactionField = value;
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
        public string cardId
        {
            get
            {
                return this.cardIdField;
            }
            set
            {
                this.cardIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string statusOutSideUserContract
        {
            get
            {
                return this.statusOutSideUserContractField;
            }
            set
            {
                this.statusOutSideUserContractField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string cardNonFinancial
        {
            get
            {
                return this.cardNonFinancialField;
            }
            set
            {
                this.cardNonFinancialField = value;
            }
        }
    }
}
