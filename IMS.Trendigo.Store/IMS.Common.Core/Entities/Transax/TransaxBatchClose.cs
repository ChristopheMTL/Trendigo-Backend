using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Entities.Transax
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.18020")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TransaxBatchClose
    {
        private string responseCodeField;

        private string clerkMessageResponseField;

        private string batchNumberSequenceField;

        private string batchTotalField;

        private string traceNumberField;

        private string batchStartTimeField;

        private string batchEndTimeField;

        private string trxcapResponseCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string responseCode
        {
            get
            {
                return this.responseCodeField;
            }
            set
            {
                this.responseCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string clerkMessageResponse
        {
            get
            {
                return this.clerkMessageResponseField;
            }
            set
            {
                this.clerkMessageResponseField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string batchNumberSequence
        {
            get
            {
                return this.batchNumberSequenceField;
            }
            set
            {
                this.batchNumberSequenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string batchTotal
        {
            get
            {
                return this.batchTotalField;
            }
            set
            {
                this.batchTotalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string traceNumber
        {
            get
            {
                return this.traceNumberField;
            }
            set
            {
                this.traceNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string batchStartTime
        {
            get
            {
                return this.batchStartTimeField;
            }
            set
            {
                this.batchStartTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string batchEndTime
        {
            get
            {
                return this.batchEndTimeField;
            }
            set
            {
                this.batchEndTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string trxcapResponseCode
        {
            get
            {
                return this.trxcapResponseCodeField;
            }
            set
            {
                this.trxcapResponseCodeField = value;
            }
        }
    }
}
