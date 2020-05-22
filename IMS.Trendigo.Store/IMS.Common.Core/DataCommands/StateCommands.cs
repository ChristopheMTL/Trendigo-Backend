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
    public class AddStateCommand : BaseDataCommand<State, TransaxEntity>
    {
        public AddStateCommand(State imsEntity, IMSEntities context) : base(imsEntity) 
        {
            this.context = context;
        }

        protected override async Task<TransaxEntity> ExecuteTransaxOperation()
        {
            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.CreationDate = DateTime.Now;
            Entity.IsActive = true;

            context.States.Add(Entity);
            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxEntity TransaxEntity)
        {
            //Nothing to do
        }
    }

    public class UpdateStateCommand : BaseDataCommand<State, TransaxEntity>
    {
        public UpdateStateCommand(State imsEntity, IMSEntities context) : base(imsEntity) 
        {
            this.context = context;
        }

        protected override async Task<TransaxEntity> ExecuteTransaxOperation()
        {
            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            context.Entry(Entity).State = EntityState.Modified;

            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxEntity TransaxEntity)
        {
            // Nothing to do
        }
    }
}
