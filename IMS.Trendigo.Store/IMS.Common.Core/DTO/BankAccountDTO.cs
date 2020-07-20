using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.DTO
{
    public class BankAccountDTO
    {
        public long bankAccountId { get; set; }
        public string accountName { get; set; }
        public string transit { get; set; }
        public string branch { get; set; }
        public string account { get; set; }
    }
}
