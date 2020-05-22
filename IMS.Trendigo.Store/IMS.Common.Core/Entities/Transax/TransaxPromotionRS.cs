using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IMS.Common.Core.Entities.Transax
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRoot("TransaxRS")]
    public partial class TransaxPromotionRS
    {
        private TransaxRSPromotion[] itemsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Promotion", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TransaxRSPromotion[] Items
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
    public partial class TransaxRSPromotion
    {

        private string idField;

        private string nameField;

        private string descriptionField;

        private string regionField;

        private string entityCategoryField;

        private string itemCategoryField;

        private string statusField;

        private string distributionEntityField;

        private string distibutionCardField;

        private string distributionOSPField;

        private string distributionProgramField;

        private string distributionISRField;

        private string distributionItemField;

        private string distributionCouponsField;

        private string discountPercentField;

        private string discountFixedField;

        private string pointsFixedField;

        private string amountPercentField;

        private string acumulativeField;

        private string maximumAmountOfPointsField;

        private string maxDiscountField;

        private string minimumAmountOfPointsField;

        private string costCustomerUsingPointsField;

        private string valueCustomerGainingPointsField;

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
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string region
        {
            get
            {
                return this.regionField;
            }
            set
            {
                this.regionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string entityCategory
        {
            get
            {
                return this.entityCategoryField;
            }
            set
            {
                this.entityCategoryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string itemCategory
        {
            get
            {
                return this.itemCategoryField;
            }
            set
            {
                this.itemCategoryField = value;
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
        public string distributionEntity
        {
            get
            {
                return this.distributionEntityField;
            }
            set
            {
                this.distributionEntityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string distibutionCard
        {
            get
            {
                return this.distibutionCardField;
            }
            set
            {
                this.distibutionCardField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string distributionOSP
        {
            get
            {
                return this.distributionOSPField;
            }
            set
            {
                this.distributionOSPField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string distributionProgram
        {
            get
            {
                return this.distributionProgramField;
            }
            set
            {
                this.distributionProgramField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string distributionISR
        {
            get
            {
                return this.distributionISRField;
            }
            set
            {
                this.distributionISRField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string distributionItem
        {
            get
            {
                return this.distributionItemField;
            }
            set
            {
                this.distributionItemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string distributionCoupons
        {
            get
            {
                return this.distributionCouponsField;
            }
            set
            {
                this.distributionCouponsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string discountPercent
        {
            get
            {
                return this.discountPercentField;
            }
            set
            {
                this.discountPercentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string discountFixed
        {
            get
            {
                return this.discountFixedField;
            }
            set
            {
                this.discountFixedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string pointsFixed
        {
            get
            {
                return this.pointsFixedField;
            }
            set
            {
                this.pointsFixedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string amountPercent
        {
            get
            {
                return this.amountPercentField;
            }
            set
            {
                this.amountPercentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string acumulative
        {
            get
            {
                return this.acumulativeField;
            }
            set
            {
                this.acumulativeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string maximumAmountOfPoints
        {
            get
            {
                return this.maximumAmountOfPointsField;
            }
            set
            {
                this.maximumAmountOfPointsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string maxDiscount
        {
            get
            {
                return this.maxDiscountField;
            }
            set
            {
                this.maxDiscountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string minimumAmountOfPoints
        {
            get
            {
                return this.minimumAmountOfPointsField;
            }
            set
            {
                this.minimumAmountOfPointsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string costCustomerUsingPoints
        {
            get
            {
                return this.costCustomerUsingPointsField;
            }
            set
            {
                this.costCustomerUsingPointsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string valueCustomerGainingPoints
        {
            get
            {
                return this.valueCustomerGainingPointsField;
            }
            set
            {
                this.valueCustomerGainingPointsField = value;
            }
        }
    }
}
