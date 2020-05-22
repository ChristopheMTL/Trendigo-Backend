using IMS.Common.Core.Data;
using IMS.Common.Core.Entities.IMS;
using IMS.Common.Core.Entities.Transax;
using IMS.Common.Core.Enumerations;
using IMS.Common.Core.Utilities;
using IMS.Store.Common.Extensions;
using IMS.Utilities.PaymentAPI.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.DataCommands
{
    public class AddMerchantCommand :  BaseDataCommand<Data.Merchant, TransaxEntity> 
    {
        private readonly int _enterpriseId;

        public AddMerchantCommand(Data.Merchant imsEntity, string enterpriseId, IMSEntities context) : base(imsEntity) 
        {
            this.context = context;
            _enterpriseId = Convert.ToInt32(enterpriseId);
        }
    
        protected override async Task<TransaxEntity> ExecuteTransaxOperation()
        {
            IMS.Utilities.PaymentAPI.Model.Merchant merchant = new IMS.Utilities.PaymentAPI.Model.Merchant();

            merchant.ProgramId = Entity.ProgramId.HasValue ? Convert.ToInt32(Entity.ProgramId.Value) : -1;
            merchant.Name = Entity.Name;
            merchant.EnterpriseId = _enterpriseId;
            merchant.Locale = Entity.IMSUsers.FirstOrDefault(a => a.AspNetUser.AspNetRoles.Any(b => b.Name == IMSRole.MerchantAdmin.ToString())).Language.ISO639_1.ToUpper();
            merchant.LogoUrl = string.IsNullOrEmpty(Entity.LogoId.ToString()) ? "" : Entity.LogoPath;
            merchant.Status = Entity.Status.ToUpper();
            
            EntityId response = await new IMS.Utilities.PaymentAPI.Api.MerchantsApi().CreateMerchant(merchant);

            if (response != null)
            {
                Entity.TransaxId = response.Id.Value.ToString();
            }

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            DateTime date = DateTime.Now;
            Entity.CreationDate = date;
            Entity.ModificationDate = date;
            Entity.IsActive = true;

            context.Merchants.Add(Entity);
            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxEntity TransaxEntity)
        {
            if (!string.IsNullOrEmpty(Entity.TransaxId))
            {
                await new IMS.Utilities.PaymentAPI.Api.MerchantsApi().DeleteMerchant(Convert.ToInt32(Entity.TransaxId));
            }
        }
    }

    public class UpdateMerchantCommand : BaseDataCommand<Data.Merchant, TransaxEntity>
    {
        private readonly int _enterpriseId;

        public UpdateMerchantCommand(Data.Merchant imsEntity, string enterpriseId, IMSEntities context) : base(imsEntity) 
        {
            this.context = context;
            _enterpriseId = Convert.ToInt32(enterpriseId);
        }

        protected override async Task<TransaxEntity> ExecuteTransaxOperation()
        {
            IMS.Utilities.PaymentAPI.Model.Merchant merchant = new IMS.Utilities.PaymentAPI.Model.Merchant();
            merchant.MerchantId = Convert.ToInt32(Entity.TransaxId);
            merchant.ProgramId = Convert.ToInt32(Entity.Program.TransaxId);
            merchant.Name = Entity.Name;
            merchant.EnterpriseId = _enterpriseId;
            merchant.Locale = Entity.IMSUsers.FirstOrDefault().Language.ISO639_1.ToUpper();
            //TODO Get Logo Path
            merchant.LogoUrl = string.IsNullOrEmpty(Entity.LogoPath) ? "" : Entity.LogoPath;
            merchant.Status = Entity.IsActive ? TransaxStatus.Active.ToString().ToUpper() : TransaxStatus.Inactive.ToString();

            await new IMS.Utilities.PaymentAPI.Api.MerchantsApi().UpdateMerchant(merchant.MerchantId, merchant);

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.ModificationDate = DateTime.Now;
            context.Entry(Entity).State = EntityState.Modified;

            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxEntity trxEntity)
        {
            // Rétablir valeurs originales
        }
    }

    public class DeleteMerchantCommand : BaseDataCommand<Data.Merchant, TransaxEntity>
    {
        private readonly int _merchantId;

        public DeleteMerchantCommand(Data.Merchant imsEntity, string merchantId, IMSEntities context) : base(imsEntity)
        {
            this.context = context;
            _merchantId = Convert.ToInt32(merchantId);
        }

        protected override async Task<TransaxEntity> ExecuteTransaxOperation()
        {
            await new IMS.Utilities.PaymentAPI.Api.MerchantsApi().DeleteMerchant(_merchantId);

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.IsActive = false;
            Entity.Status = MerchantStatus.DELETED.ToString();
            context.Entry(Entity).State = EntityState.Modified;

            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxEntity trxEntity)
        {
            // Rétablir valeurs originales
        }
    }

    public class JoinCommunityCommand : BaseDataCommand<Data.Merchant, TransaxEntity>
    {
        private readonly int _enterpriseId;

        public JoinCommunityCommand(Data.Merchant imsEntity, IMSEntities context) : base(imsEntity)
        {
            this.context = context;
        }

        protected override async Task<TransaxEntity> ExecuteTransaxOperation()
        {
            IMS.Utilities.PaymentAPI.Model.Merchant merchant = new IMS.Utilities.PaymentAPI.Model.Merchant();
            merchant.Name = Entity.Name;
            //merchant.ApplyTaxes = Entity.TaxableProduct;
            //TODO implement acquirer based on enterprise or merchant
            //merchant.ProcessorId = (int)AcquirerEnum.GlobalPayment;
            merchant.Status = TransaxStatus.Active.ToString().ToUpper();
            merchant.EnterpriseId = _enterpriseId;
            //merchant.ApplyTaxes = Entity.TaxableProduct.HasValue ? Entity.TaxableProduct.Value : true;

            EntityId response = await new IMS.Utilities.PaymentAPI.Api.MerchantsApi().CreateMerchant(merchant);

            if (response != null)
            {
                Entity.TransaxId = response.Id.Value.ToString();
            }

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            DateTime date = DateTime.Now;
            Entity.CreationDate = date;
            Entity.ModificationDate = date;
            Entity.IsActive = true;

            context.Merchants.Add(Entity);
            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxEntity TransaxEntity)
        {
            if (!string.IsNullOrEmpty(Entity.TransaxId))
            {
                await new IMS.Utilities.PaymentAPI.Api.MerchantsApi().DeleteMerchant(Convert.ToInt32(Entity.TransaxId));
            }
        }
    }

    public class AddLocationCommand : BaseDataCommand<Data.Location, TransaxEntity>
    {
        public AddLocationCommand(Data.Location imsEntity, IMSEntities context) : base(imsEntity) 
        {
            this.context = context;
        }

        protected override async Task<TransaxEntity> ExecuteTransaxOperation()
        {
            IMS.Utilities.PaymentAPI.Model.Location location = new IMS.Utilities.PaymentAPI.Model.Location();
            location.LocationId = 0;
            location.MerchantId = Convert.ToInt32(Entity.Merchant.TransaxId);
            location.ApplyTaxes = Entity.ApplyTaxes;
            location.ApplyTips = Entity.EnableTips;
            location.PayWithPoints = Entity.PayWithPoints;
            location.Status = TransaxStatus.Active.ToString().ToUpper();
            //TODO implement method to add and get processor
            location.ProcessorId = 3;
            LocationInformation locInfo = new LocationInformation();
            locInfo.Name = Entity.Address.City;
            locInfo.Address = Entity.Address.StreetAddress;
            locInfo.City = Entity.Address.City;
            locInfo.ProvinceState = Entity.Address.State.Alpha2Code;
            locInfo.Country = Entity.Address.Country.Alpha3Code;
            locInfo.PostalCode = Entity.Address.Zip;
            locInfo.Phone = Entity.Telephone;
            locInfo.Timezone = Entity.TimeZone.Value;
            locInfo.Currency = Entity.Address.Country.Currency.Code;

            location.LocationInformation = new LocationInformation();
            location.LocationInformation = locInfo;
            
            EntityId response = await new IMS.Utilities.PaymentAPI.Api.LocationsApi().CreateLocation(location);

            if (response.Id.HasValue)
            {
                Entity.TransaxId = response.Id.Value.ToString();
            }

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.CreationDate = DateTime.Now;
            Entity.IsActive = true;

            Entity.Address.CreationDate = DateTime.Now;
            Entity.Address.IsActive = true;

            //Entity.BankingInfo.CreationDate = DateTime.Now;
            //Entity.BankingInfo.ModificationDate = DateTime.Now;
            //Entity.BankingInfo.IsActive = true;

            context.Locations.Add(Entity);
            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxEntity TransaxEntity)
        {
            if (!string.IsNullOrEmpty(Entity.TransaxId))
            {
                await new IMS.Utilities.PaymentAPI.Api.LocationsApi().DeleteLocation(Convert.ToInt32(Entity.TransaxId));
            }
            
        }
    }

    public class UpdateLocationCommand : BaseDataCommand<Data.Location, TransaxEntity>
    {
        public UpdateLocationCommand(Data.Location imsEntity, IMSEntities context) : base(imsEntity) 
        {
            this.context = context;
        }

        protected override async Task<TransaxEntity> ExecuteTransaxOperation()
        {
            IMS.Utilities.PaymentAPI.Model.Location location = new IMS.Utilities.PaymentAPI.Model.Location();
            location.LocationId = Convert.ToInt32(Entity.TransaxId);
            location.MerchantId = Convert.ToInt32(Entity.Merchant.TransaxId);
            location.ApplyTaxes = Entity.ApplyTaxes;
            location.ApplyTips = Entity.EnableTips;
            location.PayWithPoints = Entity.PayWithPoints;
            location.Status = Entity.IsActive ? TransaxStatus.Active.ToString().ToUpper() : TransaxStatus.Inactive.ToString();
            location.ProcessorId = 3;
            location.LocationInformation = new LocationInformation();
            location.LocationInformation.Name = Entity.Name;
            location.LocationInformation.Address = Entity.Address.StreetAddress;
            location.LocationInformation.City = Entity.Address.City;
            location.LocationInformation.PostalCode = Entity.Address.Zip;
            location.LocationInformation.ProvinceState = Entity.Address.State.Alpha2Code;
            location.LocationInformation.Country = Entity.Address.Country.Alpha3Code;
            location.LocationInformation.Phone = Entity.Telephone;
            location.LocationInformation.Timezone = Entity.TimeZone.Value;

            await new IMS.Utilities.PaymentAPI.Api.LocationsApi().UpdateLocation(Convert.ToInt32(Entity.TransaxId), location);

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            context.Entry(Entity).State = EntityState.Modified;

            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxEntity TransaxEntity)
        {
            // Rétablir valeurs originales
        }
    }

    public class DeleteLocationCommand : BaseDataCommand<Data.Location, TransaxEntity>
    {
        private readonly int _locationId;

        public DeleteLocationCommand(Data.Location imsEntity, string locationId, IMSEntities context) : base(imsEntity)
        {
            this.context = context;
            _locationId = Convert.ToInt32(locationId);
        }

        protected override async Task<TransaxEntity> ExecuteTransaxOperation()
        {
            await new IMS.Utilities.PaymentAPI.Api.LocationsApi().DeleteLocation(_locationId);

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.IsActive = false;
            context.Entry(Entity).State = EntityState.Modified;

            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxEntity TransaxEntity)
        {
            // Rétablir valeurs originales
        }
    }

    public class AddMerchantTranslationCommand : BaseDataCommand<merchant_translations, TransaxEntity>
    {
        public AddMerchantTranslationCommand(merchant_translations imsEntity, IMSEntities context) : base(imsEntity)
        {
            this.context = context;
        }

        protected override async Task<TransaxEntity> ExecuteTransaxOperation()
        {
            TransaxEntity = new TransaxEntity();

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.created_at = DateTime.Now;
            Entity.updated_at = DateTime.Now;

            context.merchant_translations.Add(Entity);
            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxEntity TransaxEntity)
        {
            //Nothing to do
        }
    }

    public class UpdateMerchantTranslationCommand : BaseDataCommand<merchant_translations, TransaxEntity>
    {
        public UpdateMerchantTranslationCommand(merchant_translations imsEntity, IMSEntities context)
            : base(imsEntity)
        {
            this.context = context;
        }

        protected override async Task<TransaxEntity> ExecuteTransaxOperation()
        {
            TransaxEntity = new TransaxEntity();

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.updated_at = DateTime.Now;
            context.Entry(Entity).State = EntityState.Modified;

            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxEntity TransaxEntity)
        {
            // Rétablir valeurs originales
        }
    }
}
