using IMS.Common.Core.Data;
using IMS.Common.Core.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Services
{
    public class ProcessorManager
    {
        private IMSEntities db = new IMSEntities();
        public async Task<ProcessorDTO> GetProcessor(int processorId, long merchantId)
        {
            ProcessorDTO processor = new ProcessorDTO();

            Processor pro = await db.Processors.FirstOrDefaultAsync(a => a.Id == processorId);
            Merchant merchant = await db.Merchants.FirstOrDefaultAsync(a => a.Id == merchantId);

            if (merchant != null && pro != null)
            {
                processor.merchantProcessorId = 0;
                processor.merchantId = Convert.ToInt64(merchant.TransaxId);
                processor.processorId = pro.TransaxId;
                processor.processorSelectorId = "MERCHANT";
                processor.merchantLogin = "trendigo";
                processor.merchantPassword = "secret";
                processor.processorSelectorIdentity = Convert.ToInt64(merchant.TransaxId);
            }

            return processor;
        }
    }
}
