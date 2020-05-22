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

namespace IMS.Common.Core.DataCommands
{
    public class AddTerminalCommand : BaseDataCommand<Location_Terminals, TransaxTerminal>
    {
        private readonly string _transaxUserId;

        public AddTerminalCommand(Location_Terminals location_terminal, String transaxUserId, IMSEntities context)
            : base(location_terminal)
        {
            this.context = context;
            _transaxUserId = transaxUserId;
        }

        protected override async Task<TransaxTerminal> ExecuteTransaxOperation()
        {
            IMS.Utilities.PaymentAPI.Model.Terminal terminal = new IMS.Utilities.PaymentAPI.Model.Terminal();
            terminal.TerminalId = 0;
            terminal.LocationId = Convert.ToInt32(Entity.Location.TransaxId);
            terminal.PinpadSerialNumber = Entity.Terminal.SerialNumber;
            terminal.Status = TransaxStatus.Active.ToString().ToUpper();
            terminal.TerminalNumber = _transaxUserId.Length == 0 ? "" : _transaxUserId;

            EntityId response = await new IMS.Utilities.PaymentAPI.Api.TerminalsApi().CreateTerminal(terminal);

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
            context.Location_Terminals.Add(Entity);
            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxTerminal transaxTerminal)
        {
            if (!string.IsNullOrEmpty(Entity.TransaxId))
            {
                await new IMS.Utilities.PaymentAPI.Api.TerminalsApi().DeleteTerminal(Convert.ToInt32(Entity.TransaxId));
            }
        }
    }

    public class UpdateTerminalCommand : BaseDataCommand<Location_Terminals, TransaxTerminal>
    {
        private readonly string _transaxUserId;

        public UpdateTerminalCommand(Location_Terminals location_terminal, String transaxUserId, IMSEntities context)
            : base(location_terminal)
        {
            this.context = context;
            _transaxUserId = transaxUserId;
        }

        protected override async Task<TransaxTerminal> ExecuteTransaxOperation()
        {
            IMS.Utilities.PaymentAPI.Model.Terminal terminal = new IMS.Utilities.PaymentAPI.Model.Terminal();
            terminal.TerminalId = Convert.ToInt32(Entity.TransaxId);
            terminal.LocationId = Convert.ToInt32(Entity.Location.TransaxId);
            terminal.PinpadSerialNumber = Entity.Terminal.SerialNumber;
            terminal.Status = Entity.IsActive ? TransaxStatus.Active.ToString().ToUpper() : TransaxStatus.Inactive.ToString().ToUpper();
            terminal.TerminalNumber = _transaxUserId;

            logger.DebugFormat("UpdateTerminalCommand {0}", terminal.ToString());

            await new IMS.Utilities.PaymentAPI.Api.TerminalsApi().UpdateTerminal(Convert.ToInt32(Entity.TransaxId), terminal);

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.ModificationDate = DateTime.Now;
            context.Entry(Entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxTerminal transaxTerminal)
        {
            // Nothing to do
        }
    }

    public class DeleteTerminalCommand : BaseDataCommand<Location_Terminals, TransaxTerminal>
    {
        public DeleteTerminalCommand(Location_Terminals location_terminal, IMSEntities context)
            : base(location_terminal)
        {
            this.context = context;
        }

        protected override async Task<TransaxTerminal> ExecuteTransaxOperation()
        {
            await new IMS.Utilities.PaymentAPI.Api.TerminalsApi().DeleteTerminal(Convert.ToInt32(Entity.TransaxId));

            return TransaxEntity;
        }

        protected override async Task ExecuteIMSOperation()
        {
            Entity.ModificationDate = DateTime.Now;
            Entity.IsActive = false;
            context.Entry(Entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        protected override async Task RollbackTransaxOperation(TransaxTerminal transaxTerminal)
        {
            // Nothing to do
        }
    }
}
