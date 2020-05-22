using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.DTO
{
    public class StateDTO
    {
        public int stateId { get; set; }
        public int transaxId { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public List<StateLocaleDTO> locale { get; set; }
    }

    public class StateLocaleDTO
    {
        public int stateLocaleId { get; set; }
        public string language { get; set; }
        public string name { get; set; }
    }
}
