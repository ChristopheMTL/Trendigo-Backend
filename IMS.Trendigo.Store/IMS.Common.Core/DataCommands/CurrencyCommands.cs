using IMS.Common.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Common.Core.Entities.IMS;
using IMS.Common.Core.Enumerations;
using IMS.Store.Common.Extensions;
using System.Data.Entity;
using IMS.Utilities.PaymentAPI.Model;

namespace IMS.Common.Core.DataCommands
{
    public class AddCurrencyCommand : BaseDataCommand<Currency, TransaxCurrency>
    {
        public AddCurrencyCommand(Currency imsEntity, IMSEntities context) : base(imsEntity)
        {
            this.context = context;
        }

        protected override async Task<TransaxCurrency> ExecuteTransaxOperation()
        {
            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            context.Currencies.Add(Entity);
            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxCurrency trxEntity)
        {
            // Nothing to do
        }
    }

    public class UpdateCurrencyCommand : BaseDataCommand<Currency, TransaxCurrency>
    {
        public UpdateCurrencyCommand(Currency imsEntity, IMSEntities context) : base(imsEntity)
        {
            this.context = context;
        }

        protected override async Task<TransaxCurrency> ExecuteTransaxOperation()
        {
            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            context.Entry(Entity).State = EntityState.Modified;
            
            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxCurrency trxEntity)
        {
            // Nothing to do
        }
    }

    public class AddCurrencyRateCommand : BaseDataCommand<Data.CurrencyRate, TransaxCurrency>
    {
        public AddCurrencyRateCommand(Data.CurrencyRate imsEntity, IMSEntities context) : base(imsEntity)
        {
            this.context = context;
        }

        protected override async Task<TransaxCurrency> ExecuteTransaxOperation()
        {
            IMS.Utilities.PaymentAPI.Model.CurrencyRate currencyRate = new IMS.Utilities.PaymentAPI.Model.CurrencyRate();
            currencyRate.CurrencyId1 = Convert.ToInt32(Entity.Currency.TransaxId);
            currencyRate.CurrencyId2 = Convert.ToInt32(Entity.Currency1.TransaxId);
            currencyRate.Rate = (float)Entity.Rate;
            currencyRate.StartDate = Entity.StartDate;
            currencyRate.EndDate = Entity.EndDate;

            EntityId response = await new IMS.Utilities.PaymentAPI.Api.CurrenciesApi().CreateCurrencyRate(currencyRate);

            if (response != null)
            {
                Entity.TransaxId = response.Id.Value.ToString();
            }

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            context.CurrencyRates.Add(Entity);
            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxCurrency trxEntity)
        {
            // Nothing to do
        }
    }

    public class UpdateCurrencyRateCommand : BaseDataCommand<Data.CurrencyRate, TransaxCurrency>
    {
        public UpdateCurrencyRateCommand(Data.CurrencyRate imsEntity, IMSEntities context) : base(imsEntity)
        {
            this.context = context;
        }

        protected override async Task<TransaxCurrency> ExecuteTransaxOperation()
        {
            IMS.Utilities.PaymentAPI.Model.CurrencyRate currencyRate = new IMS.Utilities.PaymentAPI.Model.CurrencyRate();
            currencyRate.CurrencyRateId = Convert.ToInt32(Entity.TransaxId);
            currencyRate.CurrencyId1 = Convert.ToInt32(Entity.Currency.TransaxId);
            currencyRate.CurrencyId2 = Convert.ToInt32(Entity.Currency1.TransaxId);
            currencyRate.Rate = (float)Entity.Rate;
            currencyRate.StartDate = Entity.StartDate;
            currencyRate.EndDate = Entity.EndDate;

            await new IMS.Utilities.PaymentAPI.Api.CurrenciesApi().UpdateCurrencyRate(currencyRate.CurrencyRateId, currencyRate);

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            context.Entry(Entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxCurrency trxEntity)
        {
            // Nothing to do
        }
    }


}
