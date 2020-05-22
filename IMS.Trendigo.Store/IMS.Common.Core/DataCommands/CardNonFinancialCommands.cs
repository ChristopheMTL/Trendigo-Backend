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
using IMS.Utilities.PaymentAPI.Model;
using IMS.Utilities.PaymentAPI.Client;

namespace IMS.Common.Core.DataCommands
{
    //public class AddCardNonFinancialCommand : BaseDataCommand<IMSCard, Card>
    //{
    //    private readonly int _cardStatusId;
    //    private readonly int _memberId;

    //    public AddCardNonFinancialCommand(IMSCard imsEntity, string memberId, int cardStatusId, IMSEntities context) : base(imsEntity) 
    //    {
    //        this.context = context;
    //        _cardStatusId = cardStatusId;
    //        _memberId = Convert.ToInt32(memberId);
    //    }

    //    protected override async Task<Card> ExecuteTransaxOperation()
    //    {
    //        Card _card = new Card();
    //        _card.CardId = 0;
    //        _card.MemberId = Convert.ToInt32(Entity.Member.TransaxId);
    //        _card.CardNumber = Entity.CardNumber;
    //        _card.ProgramId = Convert.ToInt32(Entity.Program.TransaxId);
    //        _card.Status = _cardStatusId == (int)IMSCardStatus.ACTIVATED ? TransaxStatus.Active.ToString().ToUpper() : TransaxStatus.Inactive.ToString().ToUpper();
    //        _card.CardType = "MEMBERSHIP";

    //        try
    //        {
    //            EntityId response = await new IMS.Utilities.PaymentAPI.Api.MembersApi().CreateCard(_memberId, _card);

    //            if (response != null)
    //            {
    //                Entity.TransaxId = response.Id.Value.ToString();
    //            }
    //        }
    //        catch(ApiException ex)
    //        {
    //            logger.Debug("AddCardNonFinancialCommand");
    //            logger.DebugFormat("Card {0}", new ApiClient().Serialize(_card));
    //            throw new Exception(ex.ToString());
    //        }

    //        return TransaxEntity;
    //    }

    //    protected override async Task ExecuteIMSOperation()
    //    {
    //        Entity.CardStatusId = _cardStatusId;
    //        Entity.ModificationDate = DateTime.Now;
    //        Entity.AssignDate = DateTime.Now;
    //        context.Entry(Entity).State = EntityState.Modified;

    //        await context.SaveChangesAsync();
    //    }

    //    protected override Task RollbackTransaxOperation(Card trxEntity)
    //    {
    //        if (trxEntity.CardId.HasValue)
    //        {
    //            //TODO implement reversal for assign card
    //        }

    //        throw new NotImplementedException();
    //    }
    //}

    //public class UpdateCardNonFinancialCommand : BaseDataCommand<IMSCard, Card>
    //{
    //    private readonly int _cardStatusId;
    //    private readonly int _memberId;

    //    public UpdateCardNonFinancialCommand(IMSCard imsEntity, string memberId, int cardStatusId, IMSEntities context) : base(imsEntity)
    //    {
    //        this.context = context;
    //        _cardStatusId = cardStatusId;
    //        _memberId = Convert.ToInt32(memberId);
    //    }

    //    protected override async Task<Card> ExecuteTransaxOperation()
    //    {
    //        Card _card = new Card();
    //        _card.CardId = Convert.ToInt32(Entity.TransaxId);
    //        _card.CardNumber = Entity.CardNumber;
    //        _card.MemberId = Convert.ToInt32(Entity.Member.TransaxId);
    //        _card.ProgramId = Convert.ToInt32(Entity.Program.TransaxId);
    //        _card.Status = _cardStatusId == (int)IMSCardStatus.ACTIVATED ? TransaxStatus.Active.ToString().ToUpper() : TransaxStatus.Inactive.ToString().ToUpper();
    //        _card.CardType = "MEMBERSHIP";

    //        try
    //        {
    //            await new IMS.Utilities.PaymentAPI.Api.MembersApi().UpdateCard(_memberId, Convert.ToInt32(Entity.TransaxId), _card);
    //        }
    //        catch(ApiException ex)
    //        {
    //            logger.Debug("UpdateCardNonFinancialCommand");
    //            logger.DebugFormat("Card {0}", new ApiClient().Serialize(_card));
    //            throw new Exception(ex.ToString());
    //        }

    //        return TransaxEntity;
    //    }

    //    protected override async Task ExecuteIMSOperation()
    //    {
    //        Entity.ModificationDate = DateTime.Now;
    //        Entity.CardStatusId = _cardStatusId;
    //        context.Entry(Entity).State = EntityState.Modified;
    //        await context.SaveChangesAsync();
    //    }

    //    protected override Task RollbackTransaxOperation(Card trxEntity)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public class DeactivateCardNonFinancialCommand : BaseDataCommand<IMSCard, Card>
    //{
    //    private readonly int _cardId;
    //    private readonly int _memberId;
    //    private readonly int _cardStatusId;

    //    public DeactivateCardNonFinancialCommand(IMSCard imsEntity, int cardStatusId, IMSEntities context) : base(imsEntity)
    //    {
    //        this.context = context;
    //        _cardId = Convert.ToInt32(imsEntity.TransaxId);
    //        _memberId = Convert.ToInt32(imsEntity.Member.TransaxId);
    //        _cardStatusId = cardStatusId;
    //    }

    //    protected override async Task<Card> ExecuteTransaxOperation()
    //    {
    //        IMS.Utilities.PaymentAPI.Model.Card _card = await new IMS.Utilities.PaymentAPI.Api.MembersApi().FindCard(_memberId, _cardId);

    //        _card.Status = TransaxStatus.Inactive.ToString().ToUpper();

    //        await new IMS.Utilities.PaymentAPI.Api.MembersApi().UpdateCard(_memberId, _cardId, _card);

    //        if (_card.Status.ToUpper() == TransaxStatus.Inactive.ToString().ToUpper())
    //        {
    //            Entity.CardStatusId = _cardStatusId;
    //        }

    //        return TransaxEntity;
    //    }

    //    protected override async Task ExecuteIMSOperation()
    //    {
    //        Entity.ModificationDate = DateTime.Now;
    //        context.Entry(Entity).State = EntityState.Modified;

    //        await context.SaveChangesAsync();

    //        //Look for Device Request to Deactivate
    //        DeviceRequest dr = await context.DeviceRequests.FirstOrDefaultAsync(a => a.IMSCardId == Entity.Id);

    //        if (dr != null)
    //        {
    //            dr.DeviceRequestStatutId = (int)DeviceRequestStatus.DEACTIVATED;
    //            dr.ModificationDate = DateTime.Now;
    //            context.Entry(dr).State = EntityState.Modified;
    //        }

    //        await context.SaveChangesAsync();
    //    }

    //    protected override async Task RollbackTransaxOperation(Card trxEntity)
    //    {
    //        Card _card = await new IMS.Utilities.PaymentAPI.Api.MembersApi().FindCard(_memberId, _cardId);

    //        if (_card.Status == TransaxStatus.Inactive.ToString())
    //        {
    //            _card.Status = TransaxStatus.Active.ToString();

    //            await new IMS.Utilities.PaymentAPI.Api.MembersApi().UpdateCard(_memberId, _cardId, _card);
    //        }
    //    }
    //}

    //public class ReactivateCardNonFinancialCommand : BaseDataCommand<IMSCard, TransaxNonFinancialCard>
    //{
    //    private readonly int _cardId;
    //    private readonly int _memberId;

    //    public ReactivateCardNonFinancialCommand(IMSCard imsEntity, int cardStatusId, IMSEntities context) : base(imsEntity)
    //    {
    //        this.context = context;
    //        _cardId = Convert.ToInt32(imsEntity.TransaxId);
    //        _memberId = Convert.ToInt32(imsEntity.Member.TransaxId);
    //    }

    //    protected override async Task<TransaxNonFinancialCard> ExecuteTransaxOperation()
    //    {
    //        Card _card = await new IMS.Utilities.PaymentAPI.Api.MembersApi().FindCard(_memberId, _cardId);
    //        _card.Status = TransaxStatus.Active.ToString();

    //        await new IMS.Utilities.PaymentAPI.Api.MembersApi().UpdateCard(_memberId, _cardId, _card);

    //        if (_card.Status == TransaxStatus.Active.ToString())
    //        {
    //            Entity.CardStatusId = (int)IMSCardStatus.ACTIVATED;
    //        }

    //        return TransaxEntity;
    //    }

    //    protected override async Task ExecuteIMSOperation()
    //    {
    //        Entity.ModificationDate = DateTime.Now;
    //        Entity.AssignDate = DateTime.Now;
    //        context.Entry(Entity).State = EntityState.Modified;

    //        await context.SaveChangesAsync();
    //    }

    //    protected override async Task RollbackTransaxOperation(TransaxNonFinancialCard trxEntity)
    //    {
    //        Card _card = await new IMS.Utilities.PaymentAPI.Api.MembersApi().FindCard(_memberId, _cardId);

    //        if (_card.Status == TransaxStatus.Active.ToString())
    //        {
    //            _card.Status = TransaxStatus.Inactive.ToString();

    //            await new IMS.Utilities.PaymentAPI.Api.MembersApi().UpdateCard(_memberId, _cardId, _card);
    //        }
    //    }
    //}

    #region PromoCode Section

    public class ApplyPromoCodeCommand : BaseDataCommand<CardPointHistory, EntityId>
    {
        private readonly Int32 _points;
        private readonly Int32 _membershipId;

        public ApplyPromoCodeCommand(CardPointHistory imsEntity, string membershipId, Int32 points, IMSEntities context) : base(imsEntity)
        {
            this.context = context;
            _points = points;
            _membershipId = Convert.ToInt32(membershipId);
        }

        protected override async Task<EntityId> ExecuteTransaxOperation()
        {
            Point point = new Point();
            point.Value = _points;
            EntityId response = await new IMS.Utilities.PaymentAPI.Api.MembershipsApi().AddMemberPoints(_membershipId, point);

            if (response != null)
            {
                Entity.TransaxId = response.Id.Value.ToString();
            }

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            if (!String.IsNullOrEmpty(Entity.TransaxId))
            {
                Entity.CreatedDate = DateTime.Now;
                context.CardPointHistories.Add(Entity);
                await context.SaveChangesAsync();
            }
        }

        protected override async Task RollbackTransaxOperation(EntityId trxEntity)
        {
            //If TransaxId is not null this mean that the point where attributed and we need to reverse the transaction
            if (!string.IsNullOrEmpty(Entity.TransaxId))
            {
                Point point = new Point();
                point.Value = _points;
                await new IMS.Utilities.PaymentAPI.Api.MembershipsApi().SubtractMemberPoints(_membershipId, point);
            }
        }
    }

    #endregion
}
