using IMS.Common.Core.Data;
using IMS.Common.Core.Entities.IMS;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.DataCommands
{
    public class AddNewsletterCommand : BaseDataCommand<Newsletter, IMSEntityType>
    {
        public AddNewsletterCommand(Newsletter imsEntity, IMSEntities context)
            : base(imsEntity)
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
            context.Newsletters.Add(Entity);
            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(IMSEntityType trxEntity)
        {
            // Nothing to do
        }
    }

    public class UpdateNewsletterCommand : BaseDataCommand<Newsletter, IMSEntityType>
    {
        public UpdateNewsletterCommand(Newsletter imsEntity, IMSEntities context)
            : base(imsEntity)
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

    public class DeleteNewsletterCommand : BaseDataCommand<Newsletter, IMSEntityType>
    {
        public DeleteNewsletterCommand(Newsletter imsEntity, IMSEntities context)
            : base(imsEntity)
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
