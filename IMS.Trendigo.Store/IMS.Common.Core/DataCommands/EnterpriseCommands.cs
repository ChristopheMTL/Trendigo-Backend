using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Common.Core.Data;
using IMS.Common.Core.Entities.IMS;
using IMS.Common.Core.Entities.Transax;
using IMS.Common.Core.Enumerations;
using IMS.Store.Common.Extensions;

namespace IMS.Common.Core.DataCommands
{
    public class AddEnterpriseCommand : BaseDataCommand<Enterprise, TransaxSponsor>
    {
        public AddEnterpriseCommand(Enterprise imsEntity, IMSEntities context) : base(imsEntity)
        {
            this.context = context;
        }

        protected override async Task<TransaxSponsor> ExecuteTransaxOperation()
        {
            return TransaxEntity;

        }

        protected override async Task ExecuteIMSOperation()
        {
            DateTime dateNow = DateTime.Now;

            Entity.ModificationDate = dateNow;
            Entity.CreationDate = dateNow;
            Entity.IsActive = true;

            Entity.Address.CreationDate = dateNow;
            Entity.Address.IsActive = true;

            Entity.BankingInfo.CreationDate = dateNow;
            Entity.BankingInfo.ModificationDate = dateNow;

            context.Enterprises.Add(Entity);
            await context.SaveChangesAsync();

        }

        protected override async Task RollbackTransaxOperation(TransaxSponsor trxEntity)
        {
            //Nothing to do
        }
    }

    public class UpdateEnterpriseCommand : BaseDataCommand<Enterprise, TransaxSponsor>
    {
        public UpdateEnterpriseCommand(Enterprise imsEntity, IMSEntities context) : base(imsEntity)
        {
            this.context = context;
        }

        protected override async Task<TransaxSponsor> ExecuteTransaxOperation()
        {
            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.ModificationDate = DateTime.Now;
            context.Entry(Entity).State = EntityState.Modified;

            await context.SaveChangesAsync();

        }

        protected override async Task RollbackTransaxOperation(TransaxSponsor trxEntity)
        {
            //Nothing to do
        }
    }

    public class AddEnrollLocationCommand : BaseDataCommand<ContractLocation, TransaxEnrollEntity>
    {
        string _salesRepId;
        string _locationId;

        public AddEnrollLocationCommand(ContractLocation imsEntity, string salesRepId, string locationId, IMSEntities context) : base(imsEntity)
        {
            this.context = context;
            this._salesRepId = salesRepId;
            this._locationId = locationId;
        }

        protected override async Task<TransaxEnrollEntity> ExecuteTransaxOperation()
        {
            return new TransaxEnrollEntity();
        }

        protected override async Task ExecuteIMSOperation()
        {
            //Create only if the contract was created correctly in Eko-Pay
            if (!string.IsNullOrEmpty(Entity.TransaxEnterpriseContractId) && !string.IsNullOrEmpty(Entity.TransaxSalesRepContractId))
            {
                Entity.CreationDate = DateTime.Now;
                Entity.IsActive = true;

                context.ContractLocations.Add(Entity);
                await context.SaveChangesAsync();
            }
        }

        protected override async Task RollbackTransaxOperation(TransaxEnrollEntity trxEntity)
        {
            // Nothing to do
        }
    }

    public class UpdateEnrollLocationCommand : BaseDataCommand<ContractLocation, TransaxEnrollEntity>
    {
        string _salesRepId;

        public UpdateEnrollLocationCommand(ContractLocation imsEntity, string salesRepId, IMSEntities context) : base(imsEntity)
        {
            this.context = context;
            this._salesRepId = salesRepId;
        }

        protected override async Task<TransaxEnrollEntity> ExecuteTransaxOperation()
        {
            return new TransaxEnrollEntity();
        }

        protected override async Task ExecuteIMSOperation()
        {
            if (!string.IsNullOrEmpty(Entity.TransaxEnterpriseContractId) && !string.IsNullOrEmpty(Entity.TransaxSalesRepContractId)) 
            {
                context.Entry(Entity).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }

        protected override async Task RollbackTransaxOperation(TransaxEnrollEntity trxEntity)
        {
            // Nothing to do
        }
    }

    public class DeleteEnrollLocationCommand : BaseDataCommand<ContractLocation, TransaxEnrollEntity>
    {
        //string _salesRepId;

        public DeleteEnrollLocationCommand(ContractLocation imsEntity, IMSEntities context) : base(imsEntity)
        {
            this.context = context;
            //this._salesRepId = salesRepId;
        }

        protected override async Task<TransaxEnrollEntity> ExecuteTransaxOperation()
        {
            return new TransaxEnrollEntity();
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.IsActive = false;
            context.Entry(Entity).State = EntityState.Modified;

            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxEnrollEntity trxEntity)
        {
            //Nothing to do here
        }
    }

    public class DeleteMerchantEnterpriseCommand : BaseDataCommand<Enterprise, TransaxEnrollEntity>
    {
        string _merchantId;

        public DeleteMerchantEnterpriseCommand(Enterprise imsEntity, string merchantId, IMSEntities context) : base(imsEntity)
        {
            this.context = context;
            this._merchantId = merchantId;
        }

        protected override async Task<TransaxEnrollEntity> ExecuteTransaxOperation()
        {
            return new TransaxEnrollEntity();
        }

        protected override async Task ExecuteIMSOperation()
        {
            IList<Merchant> MerchantToRemove = Entity.Merchants.Where(m => m.Id.ToString() == _merchantId).ToList();

            foreach (var merchant in Entity.Merchants.Where(m => m.Id.ToString() == _merchantId).ToList())
            {

                Entity.Merchants.Remove(merchant);
                await context.SaveChangesAsync();                    
                
            }
        }

        protected override async Task RollbackTransaxOperation(TransaxEnrollEntity trxEntity)
        {
            //Nothing to do here
        }
    }
}
