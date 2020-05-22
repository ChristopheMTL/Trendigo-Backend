using IMS.Common.Core.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Common.Core.Entities.IMS;
using IMS.Common.Core.Entities.Transax;
using IMS.Common.Core.Enumerations;
using IMS.Store.Common.Extensions;
using System.Data.Entity;

namespace IMS.Common.Core.DataCommands
{
    public class AddContractCommand : BaseDataCommand<Contract, IMSEntityType>
    {
        public AddContractCommand(Contract imsEntity, IMSEntities context) : base(imsEntity) 
        {
            this.context = context;
        }

        protected override async Task<IMSEntityType> ExecuteTransaxOperation()
        {
            return new IMSEntityType(); //Contract is only for IMS, nothing to do in Transax
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.CreationDate = DateTime.Now;
            Entity.ModificationDate = DateTime.Now;
            Entity.IsActive = true;
            context.Contracts.Add(Entity);
            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(IMSEntityType trxEntity)
        {
            // Nothing to do
        }
    }

    public class UpdateContractCommand : BaseDataCommand<Contract, IMSEntityType>
    {
        public UpdateContractCommand(Contract imsEntity, IMSEntities context) : base(imsEntity) 
        {
            this.context = context;
        }

        protected override async Task<IMSEntityType> ExecuteTransaxOperation()
        {

            return new IMSEntityType(); //Contract is only for IMS, nothing to do in Transax
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.ModificationDate = DateTime.Now;
            context.Entry(Entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(IMSEntityType trxEntity)
        {
            // Nothing to do
        }
    }

    public class DeleteContractCommand : BaseDataCommand<Contract, IMSEntityType>
    {
        public DeleteContractCommand(Contract imsEntity, IMSEntities context) : base(imsEntity)
        {
            this.context = context;
        }

        protected override async Task<IMSEntityType> ExecuteTransaxOperation()
        {
            return new IMSEntityType(); //Contract is only for IMS, nothing to do in Transax
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.ModificationDate = DateTime.Now;
            Entity.IsActive = false;
            context.Entry(Entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(IMSEntityType trxEntity)
        {
            // Nothing to do
        }
    }
}
