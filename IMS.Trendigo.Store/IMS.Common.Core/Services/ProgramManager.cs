using IMS.Common.Core.Data;
using IMS.Common.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IMS.Common.Core.Services
{
    public class ProgramManager
    {
        IMSEntities context = new IMSEntities();

        public List<Program> GetProgramListWithRole(IMSUser user, Boolean ActiveOnly = false)
        {
            List<Program> programs = new List<Program>();

            if (HttpContext.Current.User.IsInRole(IMSRole.IMSAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.IMSSupport.ToString())
                || HttpContext.Current.User.IsInRole(IMSRole.IMSAccounting.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.IMSUser.ToString()))
            {
                programs = context.Programs.ToList();
                if (ActiveOnly)
                    programs = programs.Where(a => a.IsActive == true).ToList();
                return programs;
            }

            if (HttpContext.Current.User.IsInRole(IMSRole.SponsorAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.SponsorUser.ToString()))
            {
                programs = context.Programs.Where(a => a.EnterpriseId == user.EnterpriseId).Distinct().ToList();
            }

            if (ActiveOnly)
                programs = programs.Where(a => a.IsActive == true).ToList();

            return programs;
        }

        //public Program GetProgramWithRole(IMSUser user, String programId)
        //{
        //    List<Program> programs = GetProgramListWithRole(user);
        //    var listOfProgramId = programs.Select(m => m.Id);

        //    return context.Programs.Where(a => a.Id.ToString() == programId).Where(a => listOfProgramId.Contains(a.Id)).FirstOrDefault();
        //}

        //public Program GetProgram(long enterpriseId, int cardType, decimal fidelityRewardPercentage) 
        //{
        //    Program program = context.Programs
        //        .Where(a => a.EnterpriseId == enterpriseId && 
        //            a.FidelityRewardPercent == fidelityRewardPercentage && 
        //            a.CardTypes.Any(x => x.Id == cardType) &&
        //            a.IsActive == true).FirstOrDefault();

        //    return program;
        //}

        //public Program GetRegularProgram(long enterpriseId) 
        //{
        //    Program program = context.Programs
        //        .Where(a => a.EnterpriseId == enterpriseId &&
        //            a.ProgramTypeId == (int)ProgramTypeEnum.Regular &&
        //            a.IsActive == true).FirstOrDefault();

        //    return program;
        //}

        //public Program GetPrestigeProgram(long enterpriseId)
        //{
        //    Program program = context.Programs
        //        .Where(a => a.EnterpriseId == enterpriseId &&
        //            a.ProgramTypeId == (int)ProgramTypeEnum.Prestige &&
        //            a.IsActive == true).FirstOrDefault();

        //    return program;
        //}

        public DateTime GetExpiryDateWithProgram(long programId, DateTime creationDate)
        {
            Program program = context.Programs.FirstOrDefault(a => a.Id == programId);

            creationDate = creationDate.AddMonths(program.ExpirationInMonth);

            return creationDate.Date;
        }

        public async Task<List<ProgramFee>> GetUpgradeProgram(long programId)
        {
            Program program = await context.Programs.FirstOrDefaultAsync(a => a.Id == programId && a.IsActive == true);

            if (program == null)
            {
                throw new Exception("Program Not found");
            }

            ProgramFee currentProgramFees = program.ProgramFees.FirstOrDefault(x => x.IsActive);

            if (currentProgramFees == null)
            {
                throw new Exception("ProgramFee Not found");
            }

            var currentProgramFeesAmount = currentProgramFees.AssociatedFees;

            List<ProgramFee> UpgradableProgramsFees = await context.ProgramFees.Where(a => a.AssociatedFees > currentProgramFeesAmount).ToListAsync();

            return UpgradableProgramsFees;
        }
    }
}
