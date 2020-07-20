using IMS.Common.Core.Data;
using IMS.Common.Core.Enumerations;
using IMS.Utilities.PaymentAPI.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.DataCommands
{
    public class AddMembershipCommand : BaseDataCommand<IMSMembership, IMS.Utilities.PaymentAPI.Model.Membership>
    {
        private int _pointBalance;
        private int _memberId;

        public AddMembershipCommand(IMSMembership imsEntity, string memberId, int pointBalance, IMSEntities context) : base(imsEntity)
        {
            base.context = context;
            _memberId = Convert.ToInt32(memberId);
            _pointBalance = pointBalance;
        }

        protected override async Task<IMS.Utilities.PaymentAPI.Model.Membership> ExecuteTransaxOperation()
        {
            IMS.Utilities.PaymentAPI.Model.Membership TransaxEntity = new IMS.Utilities.PaymentAPI.Model.Membership();
            TransaxEntity.MembershipId = 0;
            TransaxEntity.MemberId = Convert.ToInt32(Entity.Member.TransaxId);
            TransaxEntity.ProgramId = Convert.ToInt32(Entity.Program.TransaxId);
            TransaxEntity.Status = TransaxStatus.Active.ToString().ToUpper();
            //TransaxEntity.ExpiryDate = DateTime.Now.AddYears(60).ToUniversalTime();
            TransaxEntity.PointBalance = _pointBalance;

            IMS.Utilities.PaymentAPI.Model.EntityId entityId = await new IMS.Utilities.PaymentAPI.Api.MembershipsApi().CreateMembership(TransaxEntity);

            if (entityId.Id != null)
            {
                Entity.TransaxId = entityId.Id.ToString();
            }

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.CreationDate = DateTime.Now;
            Entity.IsActive = true;
            context.IMSMemberships.Add(Entity);
            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(IMS.Utilities.PaymentAPI.Model.Membership trxEntity)
        {
            if (trxEntity.MemberId != null)
            {
                await new IMS.Utilities.PaymentAPI.Api.MembershipsApi().DeleteMembership(trxEntity.MembershipId);
            }
        }
    }

    public class UpdateMembershipCommand : BaseDataCommand<IMSMembership, IMS.Utilities.PaymentAPI.Model.Membership>
    {
        private int _pointBalance;

        public UpdateMembershipCommand(IMSMembership imsEntity, int pointBalance, IMSEntities context) : base(imsEntity)
        {
            base.context = context;
            _pointBalance = pointBalance;
        }

        protected override async Task<IMS.Utilities.PaymentAPI.Model.Membership> ExecuteTransaxOperation()
        {
            IMS.Utilities.PaymentAPI.Model.Membership TransaxEntity = new IMS.Utilities.PaymentAPI.Model.Membership();
            TransaxEntity.MembershipId = 0;
            TransaxEntity.MemberId = Convert.ToInt32(Entity.Member.TransaxId);
            TransaxEntity.ProgramId = Convert.ToInt32(Entity.Program.TransaxId);
            TransaxEntity.Status = TransaxStatus.Active.ToString().ToUpper();
            //TransaxEntity.ExpiryDate = Entity.ExpiryDate.Value.ToUniversalTime();
            TransaxEntity.PointBalance = _pointBalance;

            await new IMS.Utilities.PaymentAPI.Api.MembershipsApi().UpdateMembership(Convert.ToInt32(Entity.TransaxId), TransaxEntity);

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            context.Entry(Entity).State = EntityState.Modified;

            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(IMS.Utilities.PaymentAPI.Model.Membership trxEntity)
        {
            //Nothing to do
        }
    }

    public class DeleteMembershipCommand : BaseDataCommand<IMSMembership, IMS.Utilities.PaymentAPI.Model.Membership>
    {
        private int _membershipId;

        public DeleteMembershipCommand(IMSMembership imsEntity, IMSEntities context) : base(imsEntity)
        {
            base.context = context;
            _membershipId = Convert.ToInt32(imsEntity.TransaxId);
        }

        protected override async Task<IMS.Utilities.PaymentAPI.Model.Membership> ExecuteTransaxOperation()
        {
            await new IMS.Utilities.PaymentAPI.Api.MembershipsApi().DeleteMembership(_membershipId);

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.CreationDate = DateTime.Now;
            Entity.IsActive = false;
            context.IMSMemberships.Add(Entity);
            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(IMS.Utilities.PaymentAPI.Model.Membership trxEntity)
        {
            //Nothing to do
        }
    }

    public class AddMembershipPointsCommand : BaseDataCommand<CardPointHistory, EntityId>
    {
        private readonly Int32 _points;
        private readonly Int32 _membershipId;

        public AddMembershipPointsCommand(CardPointHistory imsEntity, string membershipId, Int32 points, IMSEntities context) : base(imsEntity)
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
                Entity.Points = _points;
                Entity.CreatedDate = DateTime.Now;
                context.Entry(Entity).State = EntityState.Modified;
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

    public class RemoveMembershipPointsCommand : BaseDataCommand<CardPointHistory, EntityId>
    {
        private readonly Int32 _points;
        private readonly Int32 _membershipId;

        public RemoveMembershipPointsCommand(CardPointHistory imsEntity, string membershipId, Int32 points, IMSEntities context) : base(imsEntity)
        {
            this.context = context;
            _points = points;
            _membershipId = Convert.ToInt32(membershipId);
        }

        protected override async Task<EntityId> ExecuteTransaxOperation()
        {
            Point point = new Point();
            point.Value = _points;
            EntityId response = await new IMS.Utilities.PaymentAPI.Api.MembershipsApi().SubtractMemberPoints(_membershipId, point);

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
                Entity.Points = (_points * -1);
                Entity.CreatedDate = DateTime.Now;
                context.Entry(Entity).State = EntityState.Modified;
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
                await new IMS.Utilities.PaymentAPI.Api.MembershipsApi().AddMemberPoints(_membershipId, point);
            }
        }
    }

    public class TransferMembershipPointsCommand : BaseDataCommand<List<CardPointHistory>, List<EntityId>>
    {
        private readonly Int32 _points;
        private readonly Int32 _fromMembershipId;
        private readonly Int32 _toMembershipId;

        public TransferMembershipPointsCommand(List<CardPointHistory> imsEntity, String fromMembershipId, String toMembershipId, Int32 points, IMSEntities context) : base(imsEntity)
        {
            this.context = context;
            _points = points;
            _fromMembershipId = Convert.ToInt32(fromMembershipId);
            _toMembershipId = Convert.ToInt32(toMembershipId);
        }

        protected override async Task<List<EntityId>> ExecuteTransaxOperation()
        {
            Point point = new Point();
            point.Value = _points;
            List<EntityId> TrxEntity = await new IMS.Utilities.PaymentAPI.Api.MembershipsApi().TransferMemberPoints(_fromMembershipId, _toMembershipId, point);

            if (TrxEntity != null)
            {
                if (TrxEntity[0].Id.HasValue)
                {
                    Entity[0].TransaxId = TrxEntity[0].Id.Value.ToString();
                }

                if (TrxEntity[1].Id.HasValue)
                {
                    Entity[1].TransaxId = TrxEntity[1].Id.Value.ToString();
                }
            }

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            if (!string.IsNullOrEmpty(Entity[0].TransaxId) && !string.IsNullOrEmpty(Entity[1].TransaxId))
            {
                Entity[0].Points = (_points * -1);
                Entity[0].CreatedDate = DateTime.Now;
                context.CardPointHistories.Add(Entity[0]);

                Entity[1].Points = _points;
                Entity[1].CreatedDate = DateTime.Now;
                context.CardPointHistories.Add(Entity[1]);

                await context.SaveChangesAsync();
            }
        }

        protected override async Task RollbackTransaxOperation(List<EntityId> trxEntity)
        {
            //If TransaxId[0] is not null and TransaxId[1] is null, this mean that the point where removed but not attributed and we need to reverse the transaction
            if (!string.IsNullOrEmpty(Entity[0].TransaxId) && string.IsNullOrEmpty(Entity[1].TransaxId))
            {
                Point point = new Point();
                point.Value = _points;
                EntityId response = await new IMS.Utilities.PaymentAPI.Api.MembershipsApi().AddMemberPoints(_fromMembershipId, point);

                if (response.Id.HasValue)
                {
                    Entity[0].Points = _points;
                    Entity[0].CreatedDate = DateTime.Now;
                    context.CardPointHistories.Add(Entity[0]);

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
