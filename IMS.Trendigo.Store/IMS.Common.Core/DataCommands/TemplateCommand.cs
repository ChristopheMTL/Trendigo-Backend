using IMS.Common.Core.Data;
using IMS.Common.Core.Entities.Transax;
using IMS.Store.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.DataCommands
{
    public class GetTemplateCommands : BaseDataCommand<TrxFinancialTransaction, TransaxTransactionRS>
    {
        private string _templateId;

        public GetTemplateCommands(TrxFinancialTransaction imsEntity, string templateId, string transaxUserId, IMSEntities context) : base(imsEntity, transaxUserId) 
        {
            this.context = context;
            this._templateId = templateId;
        }

        protected override async Task<TransaxTransactionRS> ExecuteTransaxOperation()
        {
            TransaxTransactionRS trxTransactions = new TransaxTransactionRS();
            var trxTransactionRS = await new TransaxHelper().GetTemplate(Token, _templateId);

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
}
