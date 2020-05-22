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
    public class AddEntityTypeCommand : BaseDataCommand<EntityType, TransaxEntityType>
    {
        public AddEntityTypeCommand(EntityType imsEntity, string transaxUserId, IMSEntities context) : base(imsEntity, transaxUserId) 
        {
            this.context = context;
        }

        protected override async Task<TransaxEntityType> ExecuteTransaxOperation()
        {
            long portalId = Convert.ToInt64(ConfigurationManager.AppSettings["IMSPortalID"]);
            TransaxEntity = new TransaxEntityType();
            TransaxEntity.name = Entity.Name;
            TransaxEntity.description = Entity.Name;
            TransaxEntity.providerId = context.IMSEnterpriseParameters.Where(a => a.EnterpriseId == portalId).Select(o => o.ProviderId).FirstOrDefault();
            TransaxEntity.status = ((int)TransaxStatus.Active).ToString();

            var transaxResponse = await new TransaxHelper().AddEntityType(TransaxEntity, Token);

            Entity.TransaxId = transaxResponse.id;

            return transaxResponse;
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.CreationDate = DateTime.Now;
            Entity.IsActive = true;

            context.Entry(Entity).State = EntityState.Added;
            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxEntityType trxEntity)
        {
            trxEntity.status = ((int)TransaxStatus.Inactive).ToString();
            await new TransaxHelper().UpdateEntityType(trxEntity, Token);
        }
    }
    
    public class UpdateEntityTypeCommand : BaseDataCommand<EntityType, TransaxEntityType>
    {
        public UpdateEntityTypeCommand(EntityType imsEntity, string transaxUserId, IMSEntities context) : base(imsEntity, transaxUserId) 
        {
            this.context = context;
        }

        protected override async Task<TransaxEntityType> ExecuteTransaxOperation()
        {
            TransaxEntity = new TransaxEntityType();
            TransaxEntity.id = Entity.TransaxId.ToString();
            TransaxEntity.name = Entity.Name;
            long portalId = Convert.ToInt64(ConfigurationManager.AppSettings["IMSPortalID"]);
            TransaxEntity.providerId = context.IMSEnterpriseParameters.Where(a => a.EnterpriseId == portalId).Select(o => o.ProviderId).FirstOrDefault();
            TransaxEntity.status = ((int)TransaxStatus.Active).ToString();

            return await new TransaxHelper().UpdateEntityType(TransaxEntity, Token);

        }

        protected override async Task ExecuteIMSOperation()
        {
            
            context.Entry(Entity).State = EntityState.Modified;

            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxEntityType trxEntity)
        {
            // Rétablir valeurs originales
        }
    }
}
