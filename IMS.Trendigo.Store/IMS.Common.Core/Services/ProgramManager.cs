using IMS.Common.Core.Data;
using IMS.Common.Core.DataCommands;
using IMS.Common.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

        public async Task<Program> GetProgram(string programName)
        {
            Program program = null;

            program = await context.Programs.Where(a => a.Description.ToLower().Contains(programName)).FirstOrDefaultAsync();

            return program;
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

        public async Task<Program> GetBillingProgram(long enterpriseId, int currencyId, long merchantId, IMSEntities context)
        {
            Enterprise enterprise = await context.Enterprises.Where(a => a.Id == enterpriseId).FirstOrDefaultAsync();

            if (enterprise == null)
                throw new Exception("Enteprise not found");

            Program exist = await context.Programs.Where(a => a.Description.Contains("TrendigoBilling") && a.ProgramFees.Any(b => b.CurrencyId == currencyId)).FirstOrDefaultAsync();

            if (exist == null)
            {
                Currency currency = await context.Currencies.Where(a => a.Id == currencyId).FirstOrDefaultAsync();

                if (currency == null)
                    throw new Exception("Currency not found");

                Merchant merchant = await context.Merchants.Where(a => a.Id == merchantId).FirstOrDefaultAsync();

                if (merchant == null)
                    throw new Exception("Merchant not found");

                Program newBillingProgram = new Program();
                newBillingProgram.Description = string.Concat("TrendigoBilling", currency.Code);
                newBillingProgram.ProgramTypeId = (int)ProgramTypeEnum.Personnal;
                newBillingProgram.CreationDate = DateTime.Now;
                newBillingProgram.LoyaltyCostUsingPoints = 0;
                newBillingProgram.LoyaltyValueGainingPoints = 0;
                newBillingProgram.FidelityRewardPercent = 0;
                newBillingProgram.IsActive = false;
                newBillingProgram.CardTypeId = (int)IMSCardType.MembershipCard;
                newBillingProgram.EnterpriseId = enterprise.Id;
                newBillingProgram.Enterprise = enterprise;
                newBillingProgram.Merchants.Add(merchant);

                try
                {
                    var cmd = DataCommandFactory.AddProgramCommand(newBillingProgram, context);
                    var result = await cmd.Execute();

                    if (result != DataCommandResult.Success)
                    {

                        logger.ErrorFormat(string.Format("ProgramManager - GetBillingProgram - result {0}", result));
                        throw new Exception("UnableToAddBillingProgram");
                    }
                }
                catch (Exception ex)
                {
                    logger.ErrorFormat("ProgramManager - GetBillingProgram - MerchantId {0} Exception {1} InnerException {2}", merchant.Id, ex.ToString(), ex.InnerException);
                    throw new Exception("UnableToAddBillingProgram");
                }

                ProgramFee pf = new ProgramFee();
                pf.AssociatedFees = Convert.ToDecimal(ConfigurationManager.AppSettings["IMS.Service.Program.ProgramFee.Default"]);
                pf.CreationDate = DateTime.Now;
                pf.CurrencyId = currencyId;
                pf.IsActive = true;
                pf.ProgramId = newBillingProgram.Id;

                try
                {
                    context.Entry(pf).State = EntityState.Added;
                    await context.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    logger.ErrorFormat("ProgramManager - GetBillingProgram - MerchantId {0} Exception {1} InnerException {2}", merchant.Id, ex.ToString(), ex.InnerException);
                    throw new Exception("UnableToAddBillingProgram");
                }

                return newBillingProgram;
            }

            return exist;
        }
    }
}
