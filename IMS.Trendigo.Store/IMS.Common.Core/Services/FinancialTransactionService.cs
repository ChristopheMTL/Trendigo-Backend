using IMS.Common.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Configuration;

namespace IMS.Common.Core.Services
{
    public class FinancialTransactionService
    {
        IMSEntities context = new IMSEntities();
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public async Task<List<TrxFinancialTransaction>> GetFinancialTransaction(long enterpriseId) 
        {
            List<TrxFinancialTransaction> financialTransactions = await context.TrxFinancialTransactions.Where(a => a.EnterpriseId == enterpriseId).ToListAsync();

            return financialTransactions;
        }

        public async Task<DateTime> GetLastFinancialTransactionDateTime(long enterpriseId) 
        {
            List<TrxFinancialTransaction> financialTransactions = await GetFinancialTransaction(enterpriseId);

            if (financialTransactions == null)
            {
                return Convert.ToDateTime(ConfigurationManager.AppSettings["IMS.Service.TransactionPuller.Date.StartFrom"]);
            }

            if (financialTransactions.OrderByDescending(a => a.Id).FirstOrDefault().systemDateTime.HasValue)
                return Convert.ToDateTime(financialTransactions.OrderByDescending(a => a.Id).FirstOrDefault().systemDateTime.Value);
            else
                return Convert.ToDateTime(ConfigurationManager.AppSettings["IMS.Service.TransactionPuller.Date.StartFrom"]);
        }
    }
}
