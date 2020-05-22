using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Common.Core.Data;
using IMS.Common.Core.Entities.Transax;
using IMS.Common.Core.Enumerations;
using IMS.Common.Core.Services;
using IMS.Store.Common.Extensions;
using System.Data.Entity;
using IMS.Utilities.PaymentAPI.Model;

namespace IMS.Common.Core.DataCommands
{
    public class AddProgramCommand : BaseDataCommand<Data.Program, TransaxProgram>
    {
        public AddProgramCommand(Data.Program imsEntity, IMSEntities context) : base(imsEntity)
        {
            this.context = context;
        }

        protected async override Task<TransaxProgram> ExecuteTransaxOperation()
        {
            IMS.Utilities.PaymentAPI.Model.Program program = new IMS.Utilities.PaymentAPI.Model.Program();
            program.Name = Entity.ShortDescription;
            program.Status = TransaxStatus.Active.ToString();
            program.LoyaltyValueGainingPoints = Entity.LoyaltyValueGainingPoints;
            program.LoyaltyCostUsingPoints = Entity.LoyaltyCostUsingPoints;
            program.FidelityRewardPercent = (float?)Entity.FidelityRewardPercent;

            EntityId response = await new IMS.Utilities.PaymentAPI.Api.ProgramsApi().CreateProgram(program);

            if (response != null)
            {
                Entity.TransaxId = response.Id.Value.ToString();
            }

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            context.Programs.Add(Entity);

            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxProgram TransaxEntity)
        {
            if (!string.IsNullOrEmpty(Entity.TransaxId))
            {
                await new IMS.Utilities.PaymentAPI.Api.ProgramsApi().DeleteProgram(Convert.ToInt32(Entity.TransaxId));
            }
        }
    }

    public class UpdateProgramCommand : BaseDataCommand<Data.Program, TransaxProgram>
    {
      
        public UpdateProgramCommand(Data.Program imsEntity, IMSEntities context) : base(imsEntity) 
        {
            this.context = context;
        }

        protected override async Task<TransaxProgram> ExecuteTransaxOperation()
        {
            IMS.Utilities.PaymentAPI.Model.Program program = new IMS.Utilities.PaymentAPI.Model.Program();
            program.ProgramId = Convert.ToInt32(Entity.TransaxId);
            program.Name = Entity.ShortDescription;
            program.Status = Entity.IsActive ? TransaxStatus.Active.ToString() : TransaxStatus.Inactive.ToString();
            program.LoyaltyValueGainingPoints = Entity.LoyaltyValueGainingPoints;
            program.LoyaltyCostUsingPoints = Entity.LoyaltyCostUsingPoints;
            program.FidelityRewardPercent = (float?)(Entity.FidelityRewardPercent < 1 ? Entity.FidelityRewardPercent : Entity.FidelityRewardPercent / 100);

            await new IMS.Utilities.PaymentAPI.Api.ProgramsApi().UpdateProgram(program.ProgramId, program);

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.FidelityRewardPercent = Entity.FidelityRewardPercent < 1 ? Entity.FidelityRewardPercent : Entity.FidelityRewardPercent / 100;
            context.Entry(Entity).State = EntityState.Modified;
            
            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxProgram TransaxEntity)
        {
            // TODO: Re-faire un update avec les valeurs originales. Necessiterait d'obtenir et conserver une copie du TransaxProgram avant la mise à jour
            return;
        }
    }

    public class DeleteProgramCommand : BaseDataCommand<Data.Program, TransaxProgram>
    {
        public DeleteProgramCommand(Data.Program imsEntity, IMSEntities context) : base(imsEntity)
        {
            this.context = context;
        }

        protected async override Task<TransaxProgram> ExecuteTransaxOperation()
        {
            await new IMS.Utilities.PaymentAPI.Api.ProgramsApi().DeleteProgram(Convert.ToInt32(Entity.TransaxId));

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.IsActive = false;
            context.Entry(Entity).State = EntityState.Modified;

            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxProgram TransaxEntity)
        {
            //Nothing to do
        }
    }

    public class AddProgramTranslationCommand : BaseDataCommand<program_translations, TransaxProgram>
    {
        public AddProgramTranslationCommand(program_translations imsEntity, IMSEntities context)
            : base(imsEntity)
        {
            this.context = context;
        }

        protected async override Task<TransaxProgram> ExecuteTransaxOperation()
        {
            TransaxEntity = new TransaxProgram();

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.created_at = DateTime.Now;
            Entity.updated_at = DateTime.Now;
            context.program_translations.Add(Entity);

            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxProgram TransaxEntity)
        {
            //Nothing to do here
            return;
        }
    }

    public class UpdateProgramTranslationCommand : BaseDataCommand<program_translations, TransaxProgram>
    {
        public UpdateProgramTranslationCommand(program_translations imsEntity, IMSEntities context)
            : base(imsEntity)
        {
            this.context = context;
        }

        protected async override Task<TransaxProgram> ExecuteTransaxOperation()
        {
            TransaxEntity = new TransaxProgram();

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.updated_at = DateTime.Now;
            context.Entry(Entity).State = EntityState.Modified;

            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxProgram TransaxEntity)
        {
            //Nothing to do here
            return;
        }
    }

}
