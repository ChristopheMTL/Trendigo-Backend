using IMS.Common.Core.Data;
using IMS.Common.Core.Entities.Transax;
using IMS.Common.Core.Enumerations;
using IMS.Common.Core.Utilities;
using IMS.Store.Common.Extensions;
using IMS.Utilities.PaymentAPI.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.DataCommands
{
    public class AddPromotionScheduleCommands : BaseDataCommand<Promotion_Schedules, TransaxPromotion>
    {
        readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AddPromotionScheduleCommands(Promotion_Schedules imsEntity, IMSEntities context) : base(imsEntity)
        {
            this.context = context;
        }

        protected override async Task<TransaxPromotion> ExecuteTransaxOperation()
        {
            IMS.Utilities.PaymentAPI.Model.Promotion promotion = new IMS.Utilities.PaymentAPI.Model.Promotion();
            promotion.PromotionId = 0;
            //promotion.PromotionType = Entity.Promotion.PromotionType.Description.ToUpper();
            promotion.PromotionType = IMSPromotionType.BONIFICATION.ToString();
            promotion.Value = (float?)Entity.Value;
            promotion.MaxDiscount = Entity.Promotion.MaxDiscountForPromotion > 0 ? Entity.Promotion.MaxDiscountForPromotion : 0;
            promotion.MaxAmount = Entity.Promotion.MaxAmountForPromotion > 0 ? Entity.Promotion.MaxAmountForPromotion : 0;
            promotion.StartDate = Entity.StartDate.Add(Entity.StartTime).ToUniversalTime();
            promotion.EndDate = Entity.EndDate.Add(Entity.EndTime).ToUniversalTime();
            promotion.Status = TransaxStatus.Active.ToString().ToUpper();
            List<int> locations = Entity.Promotion.Locations.Select(a => Convert.ToInt32(a.TransaxId)).ToList();
            promotion.LocationIds = locations.Cast<int?>().ToList();
            List<int> programs = Entity.Promotion.Programs.Select(a => Convert.ToInt32(a.TransaxId)).ToList();
            promotion.ProgramIds = programs.Cast<int?>().ToList();

            EntityId response = await new IMS.Utilities.PaymentAPI.Api.PromotionsApi().CreatePromotion(promotion);

            if (response != null)
            {
                Entity.TransaxId = response.Id.Value.ToString();
            }

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.CreationDate = DateTime.Now;
            Entity.IsActive = true;

            context.Promotion_Schedules.Add(Entity);
            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxPromotion TransaxEntity)
        {
            if (!string.IsNullOrEmpty(Entity.TransaxId))
            {
                await new IMS.Utilities.PaymentAPI.Api.PromotionsApi().DeletePromotion(Convert.ToInt32(Entity.TransaxId));
            }
        }
    }

    public class UpdatePromotionScheduleCommands : BaseDataCommand<Promotion_Schedules, TransaxPromotion>
    {
        public UpdatePromotionScheduleCommands(Promotion_Schedules imsEntity, IMSEntities context) : base(imsEntity)
        {
            this.context = context;
        }

        protected override async Task<TransaxPromotion> ExecuteTransaxOperation()
        {
            IMS.Utilities.PaymentAPI.Model.Promotion promotion = new IMS.Utilities.PaymentAPI.Model.Promotion();
            promotion.PromotionId = Convert.ToInt32(Entity.TransaxId);
            //promotion.PromotionType = Entity.Promotion.PromotionType.Description.ToUpper();
            promotion.PromotionType = IMSPromotionType.BONIFICATION.ToString();
            promotion.Value = (float?)Entity.Value;
            promotion.MaxDiscount = Entity.Promotion.MaxDiscountForPromotion > 0 ? Entity.Promotion.MaxDiscountForPromotion : 0;
            promotion.MaxAmount = Entity.Promotion.MaxAmountForPromotion > 0 ? Entity.Promotion.MaxAmountForPromotion : 0;
            promotion.StartDate = Entity.StartDate.Add(Entity.StartTime).ToUniversalTime();
            promotion.EndDate = Entity.EndDate.Add(Entity.EndTime).ToUniversalTime();
            promotion.Status = TransaxStatus.Active.ToString().ToUpper();
            List<int> locations = Entity.Promotion.Locations.Select(a => Convert.ToInt32(a.TransaxId)).ToList();
            promotion.LocationIds = locations.Cast<int?>().ToList();
            List<int> programs = Entity.Promotion.Programs.Select(a => Convert.ToInt32(a.TransaxId)).ToList();
            promotion.ProgramIds = programs.Cast<int?>().ToList();

            await new IMS.Utilities.PaymentAPI.Api.PromotionsApi().UpdatePromotion(promotion.PromotionId, promotion);

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            context.Entry(Entity).State = EntityState.Modified;

            await context.SaveChangesAsync();
        }

        protected override Task RollbackTransaxOperation(TransaxPromotion TransaxEntity)
        {
            throw new NotImplementedException();
        }
    }

    public class DeletePromotionScheduleCommands : BaseDataCommand<Promotion_Schedules, TransaxPromotion>
    {
        private readonly int _promotionId;

        public DeletePromotionScheduleCommands(Promotion_Schedules imsEntity, string promotionId, IMSEntities context)
            : base(imsEntity)
        {
            this.context = context;
            _promotionId = Convert.ToInt32(promotionId);
        }

        protected override async Task<TransaxPromotion> ExecuteTransaxOperation()
        {
            await new IMS.Utilities.PaymentAPI.Api.PromotionsApi().DeletePromotion(_promotionId);

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.IsActive = false;
            context.Entry(Entity).State = EntityState.Modified;

            await context.SaveChangesAsync();
        }

        protected override Task RollbackTransaxOperation(TransaxPromotion TransaxEntity)
        {
            //Nothing to do
            return null;
        }
    }
}
