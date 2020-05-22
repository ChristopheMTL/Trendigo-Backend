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
    public class AddStateTaxCommand : BaseDataCommand<StateTax, TransaxStateTaxRQ, TransaxStateTaxRS>
    {
        public AddStateTaxCommand(StateTax imsEntity, IMSEntities context) : base(imsEntity) 
        {
            this.context = context;
        }

        protected override async Task<TransaxStateTaxRS> ExecuteTransaxOperation()
        {
            return new TransaxStateTaxRS();
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.CreationDate = DateTime.Now;
            Entity.ModificationDate = DateTime.Now;
            Entity.IsActive = true;

            context.StateTaxes.Add(Entity);
            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxStateTaxRQ TransaxEntity)
        {
            //Nothing to do
        }
    }

    public class UpdateStateTaxCommand : BaseDataCommand<StateTax, TransaxStateTaxRQ, TransaxStateTaxRS>
    {
        public UpdateStateTaxCommand(StateTax imsEntity, IMSEntities context) : base(imsEntity) 
        {
            this.context = context;
        }

        protected override async Task<TransaxStateTaxRS> ExecuteTransaxOperation()
        {
            return new TransaxStateTaxRS();
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.ModificationDate = DateTime.Now;
            context.Entry(Entity).State = EntityState.Modified;

            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxStateTaxRQ TransaxEntity)
        {
            // Nothing to do
        }
    }

    public class DeleteStateTaxCommand : BaseDataCommand<StateTax, TransaxStateTaxRQ, TransaxStateTaxRS>
    {
        public DeleteStateTaxCommand(StateTax imsEntity, IMSEntities context)
            : base(imsEntity)
        {
            this.context = context;
        }

        protected override async Task<TransaxStateTaxRS> ExecuteTransaxOperation()
        {
            return new TransaxStateTaxRS();
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.IsActive = false;
            Entity.ModificationDate = DateTime.Now;
            context.Entry(Entity).State = EntityState.Modified;

            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxStateTaxRQ TransaxEntity)
        {
            // Nothing to do
        }
    }
}
