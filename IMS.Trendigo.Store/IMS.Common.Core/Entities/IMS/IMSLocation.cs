using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Common.Core.Entities.Transax;
using IMS.Common.Core.Entities.IMS.Interface;

namespace IMS.Common.Core.Entities.IMS
{
    public class IMSLocation : TransaxLocation, IIMSResponse
    {
        private TransaxLocations[] _Locations;
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

        public TransaxLocations[] Locations
        {
            get
            {
                return this._Locations;
            }
            set
            {
                this._Locations = value;
            }
        }
    }

    public partial class TransaxLocations
    {

        private TransaxLocation[] locationField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Location", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TransaxLocation[] Location
        {
            get
            {
                return this.locationField;
            }
            set
            {
                this.locationField = value;
            }
        }
    }
}
