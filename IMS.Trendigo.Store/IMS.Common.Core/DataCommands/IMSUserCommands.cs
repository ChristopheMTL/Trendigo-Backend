using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Common.Core.Data;
using IMS.Common.Core.Entities.Transax;
using IMS.Common.Core.Enumerations;
using IMS.Store.Common.Extensions;
using Microsoft.AspNet.Identity.EntityFramework;
using IMS.Common.Core.Utilities;
using IMS.Utilities.PaymentAPI.Model;
using IMS.Utilities.PaymentAPI.Client;

namespace IMS.Common.Core.DataCommands
{
    #region IMSUser

    class AddIMSUserCommand : BaseDataCommand<IMSUser, TransaxUser>
    {
        private String _rolename;

        public AddIMSUserCommand(IMSUser imsEntity, String rolename, IMSEntities context) : base(imsEntity)
        {
            this.context = context;
            _rolename = rolename;
        }

        protected override async Task<TransaxUser> ExecuteTransaxOperation()
        {
            IMS.Utilities.PaymentAPI.Model.User user = new IMS.Utilities.PaymentAPI.Model.User();

            string merchantId = Entity.Merchants.Count > 0 ? Entity.Merchants.FirstOrDefault().TransaxId : "-1";
            user.MerchantId = _rolename == "ADMIN" ? -1 : Convert.ToInt32(merchantId);
            user.UserId = 0;
            user.Role = _rolename;
            user.Locale = context.Languages.Where(a => a.Id == Entity.LanguageId).Select(b => b.ISO639_1).First().ToUpper();
            user.Name = Entity.FirstName + ' ' + Entity.LastName;
            user.Email = Entity.AspNetUser.Email;
            user.Status = TransaxStatus.Active.ToString().ToUpper();
            user.Notifications = new List<IMS.Utilities.PaymentAPI.Model.Notification>();

            EntityId response = await new IMS.Utilities.PaymentAPI.Api.UsersApi().CreateUser(user);

            if (response != null)
            {
                Entity.TransaxId = response.Id.Value.ToString();
            }

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.EnterpriseId = Entity.EnterpriseId == 0 ? null : Entity.EnterpriseId;
            Entity.CreationDate = DateTime.Now;
            Entity.ModificationDate = DateTime.Now;
            Entity.IsActive = true;
            Entity.AspNetUser = null;
            context.IMSUsers.Add(Entity);
            try 
            {
                await context.SaveChangesAsync();
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.ToString());
            }
        }

        protected override async Task RollbackTransaxOperation(TransaxUser trxEntity)
        {
            if (!string.IsNullOrEmpty(Entity.TransaxId)) 
            {
                await new IMS.Utilities.PaymentAPI.Api.UsersApi().DeleteUser(Convert.ToInt32(Entity.TransaxId));
            }
        }
    }

    class UpdateUserCommand : BaseDataCommand<IMSUser, TransaxUser>
    {
        private String _rolename;

        public UpdateUserCommand(IMSUser imsEntity, String rolename, IMSEntities context) : base(imsEntity)
        {
            this.context = context;
            _rolename = rolename;
        }
        protected override async Task<TransaxUser> ExecuteTransaxOperation()
        {
            if (_rolename == IMSRole.MerchantUser.ToString())
            {
                User user = new User();
                user.UserId = Convert.ToInt32(Entity.TransaxId);
                string merchantId = Entity.Merchants.Count > 0 ? Entity.Merchants.FirstOrDefault().TransaxId : "-1";
                user.MerchantId = Convert.ToInt32(merchantId);
                user.Name = Entity.FirstName + ' ' + Entity.LastName;
                user.Email = Entity.AspNetUser.Email;
                user.Status = TransaxStatus.Active.ToString().ToUpper();

                await new IMS.Utilities.PaymentAPI.Api.UsersApi().UpdateUser(user.UserId, user);
            }

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            context.Entry(Entity).State = System.Data.Entity.EntityState.Modified;
            await context.SaveChangesAsync();
        }

        protected override Task RollbackTransaxOperation(TransaxUser trxEntity)
        {
            throw new NotImplementedException();
        }
    }

    class DeleteIMSUserCommand : BaseDataCommand<IMSUser, TransaxUser>
    {
        private String _rolename;

        public DeleteIMSUserCommand(IMSUser imsEntity, String rolename, IMSEntities context)
            : base(imsEntity)
        {
            this.context = context;
            _rolename = rolename;
        }

        protected override async Task<TransaxUser> ExecuteTransaxOperation()
        {
            if (_rolename == IMSRole.MerchantAdmin.ToString())
            {
                await new IMS.Utilities.PaymentAPI.Api.UsersApi().DeleteUser(Convert.ToInt32(Entity.TransaxId));
            }

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.CreationDate = DateTime.Now;
            Entity.ModificationDate = DateTime.Now;
            Entity.IsActive = false;
            context.Entry(Entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxUser trxEntity)
        {
            //nothing to do here
        }
    }

    public class AddUserNotificationCommand : BaseDataCommand<Data.UserNotification, IMS.Utilities.PaymentAPI.Model.Notification>
    {
        private int _transaxId;

        public AddUserNotificationCommand(Data.UserNotification imsEntity, int transaxId, IMSEntities context) : base(imsEntity)  //, transaxUserId
        {
            base.context = context;
            _transaxId = transaxId;
        }

        protected override async Task<IMS.Utilities.PaymentAPI.Model.Notification> ExecuteTransaxOperation()
        {
            IMS.Utilities.PaymentAPI.Model.Notification TransaxEntity = new IMS.Utilities.PaymentAPI.Model.Notification();
            TransaxEntity.DeviceId = Entity.DeviceId;
            TransaxEntity.NotificationToken = Entity.NotificationToken;

            try
            {
                EntityId response = await new IMS.Utilities.PaymentAPI.Api.MembersApi().AssociateNotificationToMember(_transaxId, TransaxEntity);
            }
            catch (ApiException ex)
            {
                logger.ErrorFormat("PaymentAPI ErrorCode {0} Message {1} Exception {2}", (int)ex.ErrorCode, "Error calling CreateMember: " + ex.ErrorContent, ex.InnerException);
            }

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.IsActive = true;
            Entity.CreationDate = DateTime.Now;
            Entity.ModificationDate = DateTime.Now;
            context.UserNotifications.Add(Entity);
            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(IMS.Utilities.PaymentAPI.Model.Notification trxEntity)
        {
            //nothing to do here
        }
    }

    //public class UpdateUserNotificationCommand : BaseDataCommand<Data.UserNotification, IMS.Utilities.PaymentAPI.Model.Notification>
    //{
    //    private int _transaxId;

    //    public UpdateUserNotificationCommand(Data.UserNotification imsEntity, int transaxId, IMSEntities context) : base(imsEntity)  //, transaxUserId
    //    {
    //        base.context = context;
    //        _transaxId = transaxId;
    //    }

    //    protected override async Task<IMS.Utilities.PaymentAPI.Model.Notification> ExecuteTransaxOperation()
    //    {
    //        IMS.Utilities.PaymentAPI.Model.Notification TransaxEntity = new IMS.Utilities.PaymentAPI.Model.Notification();
    //        TransaxEntity.DeviceId = Entity.DeviceId;
    //        TransaxEntity.NotificationToken = Entity.NotificationToken;

    //        try
    //        {
    //            EntityId response = await new IMS.Utilities.PaymentAPI.Api.MembersApi().AssociateNotificationToMember(_transaxId, TransaxEntity);
    //        }
    //        catch (ApiException ex)
    //        {
    //            logger.ErrorFormat("PaymentAPI ErrorCode {0} Message {1} Exception {2}", (int)ex.ErrorCode, "Error calling CreateMember: " + ex.ErrorContent, ex.InnerException);
    //        }

    //        return TransaxEntity;
    //    }

    //    protected override async Task ExecuteIMSOperation()
    //    {
    //        Entity.ModificationDate = DateTime.Now;

    //        context.Entry(Entity).State = EntityState.Modified;
    //        await context.SaveChangesAsync();
    //    }

    //    protected override async Task RollbackTransaxOperation(IMS.Utilities.PaymentAPI.Model.Notification trxEntity)
    //    {
    //        //nothing to do here
    //    }
    //}

    //public class DeleteUserNotificationCommand : BaseDataCommand<UserNotification, IMS.Utilities.PaymentAPI.Model.Member>
    //{
    //    private String _rolename;

    //    public DeleteUserNotificationCommand(UserNotification imsEntity, IMSEntities context)
    //        : base(imsEntity)
    //    {
    //        this.context = context;
    //    }

    //    protected override Task<IMS.Utilities.PaymentAPI.Model.Member> ExecuteTransaxOperation()
    //    {
    //        return null;
    //    }

    //    protected override async Task ExecuteIMSOperation()
    //    {
    //        Entity.ModificationDate = DateTime.Now;
    //        Entity.IsActive = false;
    //        context.Entry(Entity).State = EntityState.Modified;
    //        await context.SaveChangesAsync();
    //    }

    //    protected override Task RollbackTransaxOperation(IMS.Utilities.PaymentAPI.Model.Member trxEntity)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    #endregion

    #region Outside Channel

    public class AddOutsideChannelCommand : BaseDataCommand<OutsideChannel, TransaxOutsidePartner>
    {
        public AddOutsideChannelCommand(OutsideChannel imsEntity, IMSEntities context)
            : base(imsEntity)
        {
            this.context = context;
        }

        protected override async Task<TransaxOutsidePartner> ExecuteTransaxOperation()
        {
            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.CreationDate = DateTime.Now;
            Entity.ModificationDate = DateTime.Now;
            Entity.IsActive = true;

            context.OutsideChannels.Add(Entity);
            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxOutsidePartner TransaxEntity)
        {
            //Nothing to do
        }
    }

    public class UpdateOutsideChannelCommand : BaseDataCommand<OutsideChannel, TransaxOutsidePartner>
    {
        public UpdateOutsideChannelCommand(OutsideChannel imsEntity, IMSEntities context)
            : base(imsEntity)
        {
            this.context = context;
        }

        protected override async Task<TransaxOutsidePartner> ExecuteTransaxOperation()
        {
            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            context.Entry(Entity).State = EntityState.Modified;

            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxOutsidePartner TransaxEntity)
        {
            // Rétablir valeurs originales
        }
    }

    public class DeleteOutsideChannelCommand : BaseDataCommand<OutsideChannel, TransaxOutsidePartner>
    {
        public DeleteOutsideChannelCommand(OutsideChannel imsEntity, IMSEntities context) : base(imsEntity)
        {
            this.context = context;
        }

        protected override async Task<TransaxOutsidePartner> ExecuteTransaxOperation()
        {
            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.ModificationDate = DateTime.Now;
            Entity.IsActive = false;
            context.Entry(Entity).State = EntityState.Modified;

            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxOutsidePartner TransaxEntity)
        {
            //Nothing to do
        }
    }

    #endregion

    #region SalesRep

    public class AddSalesRepCommand : BaseDataCommand<SalesRep, TransaxInsideRep>
    {
        public AddSalesRepCommand(SalesRep imsEntity, IMSEntities context)
            : base(imsEntity)
        {
            this.context = context;
        }

        protected override async Task<TransaxInsideRep> ExecuteTransaxOperation()
        {
            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.CreationDate = DateTime.Now;
            Entity.ModificationDate = DateTime.Now;
            Entity.IsActive = true;

            context.SalesReps.Add(Entity);
            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxInsideRep TransaxEntity)
        {
            //Nothing to do
        }
    }

    public class UpdateSalesRepCommand : BaseDataCommand<SalesRep, TransaxInsideRep>
    {
        public UpdateSalesRepCommand(SalesRep imsEntity, IMSEntities context)
            : base(imsEntity)
        {
            this.context = context;
        }

        protected override async Task<TransaxInsideRep> ExecuteTransaxOperation()
        {
            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            context.Entry(Entity).State = EntityState.Modified;

            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxInsideRep TransaxEntity)
        {
            // Rétablir valeurs originales
        }
    }

    public class DeleteSalesRepCommand : BaseDataCommand<SalesRep, TransaxInsideRep>
    {
        public DeleteSalesRepCommand(SalesRep imsEntity, IMSEntities context)
            : base(imsEntity)
        {
            this.context = context;
        }

        protected override async Task<TransaxInsideRep> ExecuteTransaxOperation()
        {
            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.IsActive = false;
            Entity.ModificationDate = DateTime.Now;
            context.Entry(Entity).State = EntityState.Modified;
            
            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxInsideRep TransaxEntity)
        {
            //Nothing to do
        }
    }

    #endregion

    
}
