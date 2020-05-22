using IMS.Common.Core.Data;
using IMS.Common.Core.Enumerations;
using IMS.Utilities.PaymentAPI.Api;
using IMS.Utilities.PaymentAPI.Client;
using IMS.Utilities.PaymentAPI.Model;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace IMS.Common.Core.DataCommands
{
    public class AddMemberCommand : BaseDataCommand<Data.Member, IMS.Utilities.PaymentAPI.Model.Member>
    {
        private string _email;
        private string _pin;

        public AddMemberCommand(Data.Member imsEntity, string email, IMSEntities context, string pin = "") : base(imsEntity)  //, transaxUserId
        {
            base.context = context;
            _email = email;
            _pin = pin;
        }

        protected override async Task<IMS.Utilities.PaymentAPI.Model.Member> ExecuteTransaxOperation()
        {
            IMS.Utilities.PaymentAPI.Model.Member TransaxEntity = new IMS.Utilities.PaymentAPI.Model.Member();
            TransaxEntity.MemberId = 0;
            TransaxEntity.Name = Entity.FirstName + " " + Entity.LastName;
            TransaxEntity.Email = _email;
            TransaxEntity.Locale = Entity.Language.ISO639_1.ToUpper();
            TransaxEntity.Status = TransaxStatus.Active.ToString().ToUpper();

            try
            {
                EntityId response = await new IMS.Utilities.PaymentAPI.Api.MembersApi().CreateMember(TransaxEntity);

                if (response != null)
                {
                    Entity.TransaxId = response.Id.Value.ToString();
                }
            }
            catch(ApiException ex)
            {
                logger.ErrorFormat("PaymentAPI ErrorCode {0} Message {1} Exception {2}", (int)ex.ErrorCode, "Error calling CreateMember: " + ex.ErrorContent, ex.InnerException);
            }

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.CreationDate = DateTime.Now;
            Entity.IsActive = true;
            context.Members.Add(Entity);
            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(IMS.Utilities.PaymentAPI.Model.Member trxEntity)
        {
            if (trxEntity.MemberId != null)
            {
                await new IMS.Utilities.PaymentAPI.Api.MembersApi().DeleteMember(trxEntity.MemberId);
            }
        }
    }

    public class UpdateMemberCommand : BaseDataCommand<Data.Member, IMS.Utilities.PaymentAPI.Model.Member>
    {
        public UpdateMemberCommand(Data.Member imsEntity, IMSEntities context) : base(imsEntity)
        {
            this.context = context;
        }

        protected override async Task<IMS.Utilities.PaymentAPI.Model.Member> ExecuteTransaxOperation()
        {
            IMS.Utilities.PaymentAPI.Model.Member TransaxEntity = new IMS.Utilities.PaymentAPI.Model.Member();
            TransaxEntity.MemberId = Convert.ToInt32(Entity.TransaxId);
            TransaxEntity.Name = Entity.FirstName + " " + Entity.LastName;
            TransaxEntity.Email = Entity.AspNetUser.Email;
            TransaxEntity.Locale = Entity.Language.ISO639_1.ToUpper();
            TransaxEntity.Status = Entity.IsActive ? TransaxStatus.Active.ToString().ToUpper() : TransaxStatus.Inactive.ToString().ToUpper();

            if (TransaxEntity.Notifications != null && Entity.AspNetUser.Notifications != null)
            {
                TransaxEntity.Notifications.First().DeviceId = Entity.AspNetUser.UserNotifications.FirstOrDefault().DeviceId;
                TransaxEntity.Notifications.First().NotificationToken = Entity.AspNetUser.UserNotifications.FirstOrDefault().NotificationToken;
            }

            try
            {
                await new MembersApi().UpdateMember(Convert.ToInt32(Entity.TransaxId), TransaxEntity);
            }
            catch (ApiException ex)
            {
                logger.ErrorFormat("PaymentAPI ErrorCode {0} Message {1} Exception {2}", (int)ex.ErrorCode, "Error calling UpdateMember: " + ex.ErrorContent, ex.InnerException);
            }

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            context.Entry(Entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        protected override Task RollbackTransaxOperation(IMS.Utilities.PaymentAPI.Model.Member trxEntity)
        {
            throw new NotImplementedException();
        }
    }

    public class DeleteMemberCommand : BaseDataCommand<Data.Member, IMS.Utilities.PaymentAPI.Model.Member>
    {
        public DeleteMemberCommand(Data.Member imsEntity, IMSEntities context) : base(imsEntity)
        {
            this.context = context;
        }

        protected override async Task<IMS.Utilities.PaymentAPI.Model.Member> ExecuteTransaxOperation()
        {
            TransaxEntity = new IMS.Utilities.PaymentAPI.Model.Member();

            if (!String.IsNullOrEmpty(Entity.TransaxId)) 
            {
                try
                {
                    await new IMS.Utilities.PaymentAPI.Api.MembersApi().DeleteMember(Convert.ToInt32(Entity.TransaxId));
                }
                catch (ApiException ex)
                {
                    logger.ErrorFormat("PaymentAPI ErrorCode {0} Message {1} Exception {2}", (int)ex.ErrorCode, "Error calling DeleteMember: " + ex.ErrorContent, ex.InnerException);
                }
            }

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.AspNetUser.Email = string.Concat(Entity.AspNetUser.Email, ".DELETED");
            Entity.AspNetUser.UserName = string.Concat(Entity.AspNetUser.UserName, ".DELETED");
            Entity.IsActive = false;
            context.Entry(Entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        protected override Task RollbackTransaxOperation(IMS.Utilities.PaymentAPI.Model.Member trxEntity)
        {
            throw new NotImplementedException();
        }
    }
}
