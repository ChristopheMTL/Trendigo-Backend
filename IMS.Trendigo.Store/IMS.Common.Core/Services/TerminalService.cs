using IMS.Common.Core.Data;
using IMS.Common.Core.DataCommands;
using IMS.Common.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Services
{
    public class TerminalService
    {
        private IMSEntities db = new IMSEntities();

        public async Task<Location_Terminals> SetTerminalTimezone(long terminalId, string transaxUserId)
        {
            #region Validation Section

            Terminal terminal = db.Terminals.Where(a => a.Id == terminalId).FirstOrDefault();

            if (terminal == null)
            {
                throw new Exception("Terminal not found");
            }

            if (terminal.Location_Terminals.Count == 0)
            {
                throw new Exception("Terminal location not found");
            }

            Location_Terminals locTerminal = db.Location_Terminals.Where(a => a.TerminalId == terminal.Id && a.IsActive == true).FirstOrDefault();

            if (locTerminal == null)
            {
                throw new Exception("Terminal location not found");
            }

            #endregion

            locTerminal.Terminal.E10011 = new UtilityManager().returnOffsetValue(locTerminal.Location, db);

            var command = DataCommandFactory.UpdateTerminalCommand(locTerminal, transaxUserId, db);

            var result = await command.Execute();

            if (result != DataCommandResult.Success)
            {
                throw new Exception("Unable to set timezone for terminal Id " + terminalId.ToString() + " Exception " + result.ToString());
            }

            return locTerminal;
        }

        public String GetTerminalNameWithAPITerminalId(string terminalId)
        {
            String terminalName = "";

            Location_Terminals locTerminal = db.Location_Terminals.FirstOrDefault(a => a.TransaxId == terminalId);

            if (locTerminal != null)
                terminalName = locTerminal.Name;

            return terminalName;
        }
    }
}
