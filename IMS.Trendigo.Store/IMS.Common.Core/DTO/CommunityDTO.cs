using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.DTO
{
    public class CommunityDTO
    {
        public long communityId { get; set; }
        public long communityTypeId { get; set; }
        public string name { get; set; }
    }

    public class CommunityTypeDTO
    {
        public int communityTypeId { get; set; }
        public string description { get; set; }
        public List<CommunityTypeFeeDTO> fees { get; set; }
    }

    public class CommunityTypeFeeDTO
    {
        public string currency { get; set; }
        public decimal amount { get; set; }
    }
}
