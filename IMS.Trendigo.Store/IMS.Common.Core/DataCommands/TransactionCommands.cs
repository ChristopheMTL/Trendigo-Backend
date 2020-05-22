using IMS.Common.Core.Data;
using IMS.Common.Core.Entities.IMS;
using IMS.Common.Core.Entities.Transax;
using IMS.Common.Core.Enumerations;
using IMS.Store.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.DataCommands
{
    public class AddFinancialTransactionCommands : BaseDataCommand<TrxFinancialTransaction, TransaxTransactionRS>
    {
        public AddFinancialTransactionCommands(TrxFinancialTransaction imsEntity, IMSEntities context) : base(imsEntity) 
        {
            this.context = context;
        }

        protected override async Task<TransaxTransactionRS> ExecuteTransaxOperation()
        {
            //Nothing to do here
            return new TransaxTransactionRS();
        }

        protected override async Task ExecuteIMSOperation()
        {
            context.TrxFinancialTransactions.Add(Entity);
            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxTransactionRS TransaxEntity)
        {
            //Nothing to do here
        }
    }

    public class GetFinancialTransactionCommands : BaseDataCommand<TrxFinancialTransaction, TransaxTransactionRS>
    {
        private string _enterpriseId;

        public GetFinancialTransactionCommands(TrxFinancialTransaction imsEntity, string enterpriseId, IMSEntities context) : base(imsEntity) 
        {
            this.context = context;
            this._enterpriseId = enterpriseId;
        }

        protected override async Task<TransaxTransactionRS> ExecuteTransaxOperation()
        {
            TransaxTransactionRS trxTransactions = new TransaxTransactionRS();
            DateTime dt = new EPOCHHelper().ConvertToDateTime(Convert.ToDouble(Entity.systemDateTime));
            string nextTransaction = new EPOCHHelper().ConvertToTimestamp(dt.AddMilliseconds(1)).ToString();
            var trxTransactionRS = await new TransaxHelper().GetAllFinancialTransactions(Token, nextTransaction, _enterpriseId);

            return trxTransactionRS;
        }

        protected override async Task ExecuteIMSOperation()
        {
            //Entity.CreationDate = DateTime.Now;
            //Entity.IsActive = true;

            //context.OutsideChannels.Add(Entity);
            //await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxTransactionRS TransaxEntity)
        {
            //Nothing to do here
        }
    }

    public class AddNonFinancialTransactionCommands : BaseDataCommand<TrxNonFinancialTransaction, TransaxNonFinancialTransactionRS>
    {
        public AddNonFinancialTransactionCommands(TrxNonFinancialTransaction imsEntity, IMSEntities context) : base(imsEntity)
        {
            this.context = context;
        }

        protected override async Task<TransaxNonFinancialTransactionRS> ExecuteTransaxOperation()
        {
            //Nothing to do here
            return new TransaxNonFinancialTransactionRS();
        }

        protected override async Task ExecuteIMSOperation()
        {
            context.TrxNonFinancialTransactions.Add(Entity);
            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxNonFinancialTransactionRS TransaxEntity)
        {
            //Nothing to do here
        }
    }

    public class GetNonFinancialTransactionCommands : BaseDataCommand<TrxNonFinancialTransaction, TransaxNonFinancialTransactionRS>
    {
        private string _enterpriseId;

        public GetNonFinancialTransactionCommands(TrxNonFinancialTransaction imsEntity, string enterpriseId, IMSEntities context)
            : base(imsEntity)
        {
            this.context = context;
            this._enterpriseId = enterpriseId;
        }

        protected override async Task<TransaxNonFinancialTransactionRS> ExecuteTransaxOperation()
        {
            TransaxNonFinancialTransactionRS trxTransactions = new TransaxNonFinancialTransactionRS();
            DateTime dt = new EPOCHHelper().ConvertToDateTime(Convert.ToDouble(Entity.systemDateTime));
            string nextNonFinancialTransaction = new EPOCHHelper().ConvertToTimestamp(dt.AddMilliseconds(1)).ToString();
            var trxNonFinancialTransactionRS = await new TransaxHelper().GetAllNonFinancialTransactions(Token, nextNonFinancialTransaction, _enterpriseId);

            return trxNonFinancialTransactionRS;
        }

        protected override async Task ExecuteIMSOperation()
        {
            //Entity.CreationDate = DateTime.Now;
            //Entity.IsActive = true;

            //context.OutsideChannels.Add(Entity);
            //await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxNonFinancialTransactionRS TransaxEntity)
        {
            //Nothing to do here
        }
    }
}
