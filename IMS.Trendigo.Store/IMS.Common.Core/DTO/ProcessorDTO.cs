using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.DTO
{
    public class ProcessorDTO
    {
        public long merchantProcessorId { get; set; }
        public long merchantId { get; set; }
        public int processorId { get; set; }
        public string processorSelectorId { get; set; }
        public string merchantLogin { get; set; }
        public string merchantPassword { get; set; }
        public long processorSelectorIdentity { get; set; }
    }
}
