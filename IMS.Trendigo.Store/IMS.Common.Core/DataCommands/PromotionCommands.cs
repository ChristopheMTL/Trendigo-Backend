using IMS.Common.Core.Data;
using IMS.Common.Core.Entities.Transax;
using IMS.Common.Core.Enumerations;
using IMS.Common.Core.Utilities;
using IMS.Store.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.DataCommands
{
    public class AddPromotionCommands : BaseDataCommand<Promotion, TransaxPromotion>
    {
        public AddPromotionCommands(Promotion imsEntity, IMSEntities context) : base(imsEntity) 
        {
            this.context = context;
        }

        protected override async Task<TransaxPromotion> ExecuteTransaxOperation()
        {
            return new TransaxPromotion();
       }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.CreationDate = DateTime.Now;
            Entity.ModificationDate = DateTime.Now;
            Entity.IsActive = true;

            context.Promotions.Add(Entity);
            await context.SaveChangesAsync();
        }

        protected override Task RollbackTransaxOperation(TransaxPromotion trxEntity)
        {
            throw new NotImplementedException();
        }
    }

    public class UpdatePromotionCommands : BaseDataCommand<Promotion, TransaxPromotion> 
    {
        public UpdatePromotionCommands(Promotion imsEntity, IMSEntities context) : base(imsEntity) 
        {
            this.context = context;
        }

        protected override async Task<TransaxPromotion> ExecuteTransaxOperation()
        {
            return new TransaxPromotion();
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.ModificationDate = DateTime.Now;

            context.Entry(Entity).State = EntityState.Modified;

            await context.SaveChangesAsync();
        }

        protected override Task RollbackTransaxOperation(TransaxPromotion TransaxEntity)
        {
            throw new NotImplementedException();
        }
    }

    public class DeletePromotionCommands : BaseDataCommand<Promotion, TransaxPromotion>
    {
        public DeletePromotionCommands(Promotion imsEntity, IMSEntities context)
            : base(imsEntity)
        {
            this.context = context;
        }

        protected override async Task<TransaxPromotion> ExecuteTransaxOperation()
        {
            return new TransaxPromotion();
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.IsActive = false;
            Entity.ModificationDate = DateTime.Now;

            context.Entry(Entity).State = EntityState.Modified;

            await context.SaveChangesAsync();
        }

        protected override Task RollbackTransaxOperation(TransaxPromotion TransaxEntity)
        {
            throw new NotImplementedException();
        }
    }

    public class AddUpdatePromotionTranslationCommand : BaseDataCommand<promotion_translations, TransaxPromotion>
    {
        public AddUpdatePromotionTranslationCommand(promotion_translations imsEntity, IMSEntities context)
            : base(imsEntity)
        {
            this.context = context;
        }

        protected override async Task<TransaxPromotion> ExecuteTransaxOperation()
        {
            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.created_at = DateTime.Now;
            Entity.updated_at = DateTime.Now;

            if (Entity.id > 0) 
            {
                context.Entry(Entity).State = EntityState.Modified;
            }
            else 
            {
                context.promotion_translations.Add(Entity);
            }
            
            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxPromotion TransaxEntity)
        {
            //Nothing to do
        }
    }
}
