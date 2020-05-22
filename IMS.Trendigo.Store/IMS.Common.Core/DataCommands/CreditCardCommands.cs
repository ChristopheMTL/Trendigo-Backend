using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using IMS.Common.Core;
using IMS.Common.Core.Entities.Transax;
using IMS.Store.Common.Extensions;
using System.Data.Entity;
using System.Globalization;
using IMS.Common.Core.Enumerations;
using IMS.Common.Core.Exceptions;
using System.Web;
using IMS.Utilities.PaymentAPI.Model;
using IMS.Common.Core.DTO;

namespace IMS.Common.Core.DataCommands
{
    public class AddCreditCardCommand : BaseDataCommand<Data.CreditCard, IMS.Utilities.PaymentAPI.Model.Creditcard>
    {
        private readonly int _memberId;
        private readonly CreditCardDTO _cc;

        public AddCreditCardCommand(Data.CreditCard imsEntity, CreditCardDTO cc, string memberId, Data.IMSEntities context)  : base(imsEntity)
        {
            this.context = context;
            _memberId = Convert.ToInt32(memberId);
            _cc = cc;
        }

        protected override async Task<Creditcard> ExecuteTransaxOperation()
        {
            Creditcard ccard = new Creditcard();
            ccard.CreditCardId = 0;
            ccard.MemberId = _memberId;
            ccard.PanMask = _cc.CardNumber;
            ccard.NameOnCard = _cc.CardHolder;
            ccard.Token = _cc.Token;
            ccard.ExpirationDate = _cc.ExpiryDate;
            ccard.CardTypeId = _cc.CreditCardTypeId;
            ccard.Status = TransaxStatus.Active.ToString().ToUpper();

            EntityId response = await new IMS.Utilities.PaymentAPI.Api.MembersApi().CreateCreditCard(_memberId, ccard);

            if (response != null)
            {
                Entity.TransaxId = response.Id.Value.ToString();
            }

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            Data.CreditCard cc = new Data.CreditCard();

            Entity.CreationDate = DateTime.Now;
            Entity.IsActive = true;

            context.CreditCards.Add(Entity);

            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(Creditcard trxEntity)
        {
            //TransaxId > 0 ==> This means IMSFailed to insert credit card
            //We need to reverse the insertion of the card and delete it
            if (!string.IsNullOrEmpty(Entity.TransaxId))
            {
                await new IMS.Utilities.PaymentAPI.Api.MembersApi().DeleteCreditCard(_memberId, Convert.ToInt32(Entity.TransaxId));
            }
            return;
        }
    }

    public class UpdateCreditCardCommand : BaseDataCommand<Data.CreditCard, Creditcard>
    {
        private readonly CreditCardDTO _cc;

        public UpdateCreditCardCommand(Data.CreditCard imsEntity, CreditCardDTO cc, Data.IMSEntities context) : base(imsEntity)
        {
            _cc = cc;
            this.context = context;
        }

        protected override async Task<Creditcard> ExecuteTransaxOperation()
        {
            TransaxEntity = new Creditcard();
            TransaxEntity.CreditCardId = Convert.ToInt32(Entity.TransaxId);
            TransaxEntity.MemberId = Convert.ToInt32(Entity.Member.TransaxId);
            TransaxEntity.NameOnCard = _cc.CardHolder;
            //TransaxEntity.Token = _cc.Token;
            //TransaxEntity.PanMask = _cc.CardNumber;
            TransaxEntity.ExpirationDate = _cc.ExpiryDate;
            TransaxEntity.CardTypeId = _cc.CreditCardTypeId;
            TransaxEntity.CreationDate = _cc.CreationDate.ToUniversalTime();
            TransaxEntity.Status = Entity.IsActive ? TransaxStatus.Active.ToString().ToUpper() : TransaxStatus.Inactive.ToString().ToUpper();

            await new IMS.Utilities.PaymentAPI.Api.MembersApi().UpdateCreditCard(Convert.ToInt32(Entity.Member.TransaxId), Convert.ToInt32(Entity.TransaxId), TransaxEntity);

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            context.Entry(Entity).State = EntityState.Modified;

            await context.SaveChangesAsync();
        }

        protected override Task RollbackTransaxOperation(Creditcard trxEntity)
        {
            throw new NotImplementedException();
        }
    }

    public class DeleteCreditCardCommand : BaseDataCommand<Data.CreditCard, Creditcard>
    {
        public DeleteCreditCardCommand(Data.CreditCard imsEntity, Data.IMSEntities context) : base(imsEntity)
        {
            this.context = context;
        }

        protected override async Task<Creditcard> ExecuteTransaxOperation()
        {
            await new IMS.Utilities.PaymentAPI.Api.MembersApi().DeleteCreditCard(Convert.ToInt32(Entity.Member.TransaxId), Convert.ToInt32(Entity.TransaxId));

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.IsActive = false;
            context.Entry(Entity).State = EntityState.Modified;

            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(Creditcard trxEntity)
        {
            //Nothing to do
            return;
        }
    }

    public class SetDefaultCreditCardCommand : BaseDataCommand<Data.CreditCard, Creditcard>
    {
        private readonly int _memberId;
        private readonly int _creditCardId;

        public SetDefaultCreditCardCommand(Data.CreditCard imsEntity, string memberId, string creditCardId, Data.IMSEntities context) : base(imsEntity)
        {
            _memberId = Convert.ToInt32(memberId);
            _creditCardId = Convert.ToInt32(creditCardId);
            this.context = context;
        }

        protected override async Task<Creditcard> ExecuteTransaxOperation()
        {
            await new IMS.Utilities.PaymentAPI.Api.MembersApi().SetDefaultCreditCard(_memberId, _creditCardId);

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            //Nothing to do
        }

        protected override async Task RollbackTransaxOperation(Creditcard trxEntity)
        {
            //Nothing to do
        }
    }
}
