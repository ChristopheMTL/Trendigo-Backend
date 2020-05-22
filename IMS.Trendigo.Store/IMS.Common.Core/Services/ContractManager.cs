using IMS.Common.Core.Data;
using IMS.Common.Core.DataCommands;
using IMS.Common.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;

namespace IMS.Common.Core.Services
{
    public class ContractManager
    {
        IMSEntities context = new IMSEntities();
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public List<Contract> GetContractWithRole(IMSUser imsUser, String EnterpriseId, Boolean ActiveOnly = false)
        {
            List<Contract> contracts = GetContractWithRole(imsUser, ActiveOnly);

            return contracts.Where(a => a.EnterpriseId.ToString() == EnterpriseId).ToList();
        }

        public List<Contract> GetContractWithRole(IMSUser user, Boolean ActiveOnly = false)
        {
            List<Contract> contracts = new List<Contract>();

            if (HttpContext.Current.User.IsInRole(IMSRole.IMSAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.IMSSupport.ToString())
                || HttpContext.Current.User.IsInRole(IMSRole.IMSAccounting.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.IMSUser.ToString()))
            {
                contracts = context.Contracts.ToList();
                if (ActiveOnly)
                    contracts = contracts.Where(a => a.IsActive == true).ToList();
                return contracts;
            }

            if (HttpContext.Current.User.IsInRole(IMSRole.SponsorAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.SponsorUser.ToString()))
            {
                contracts = context.Contracts.Where(a => a.EnterpriseId == user.EnterpriseId).Distinct().ToList();
            }

            if (ActiveOnly)
                contracts = contracts.Where(a => a.IsActive == true).ToList();

            return contracts;
        }

        private Contract GetContract(TrxFinancialTransaction transaction, Int64 enterpriseId)
        {
            Promotion_Schedules promoSchedule = context.Promotion_Schedules.Where(a => a.Promotion.Locations.FirstOrDefault().Merchant.Enterprises.Any(b => b.Id == enterpriseId) && a.StartDate <= transaction.localDateTime && a.EndDate >= transaction.localDateTime && a.IsActive == true).FirstOrDefault();

            if (promoSchedule == null)
                return null;

            if (promoSchedule.Promotion == null)
                return null;

            if (promoSchedule.Promotion == null)
                return null;

            if (promoSchedule.Promotion.Package == null)
                return null;

            if (promoSchedule.Promotion.Package.Contract == null)
                return null;

            return promoSchedule.Promotion.Package.Contract;
        }

        public Contract AddNewContract()
        {
            return new Contract();
        }

        public Contract UpdateContract(Int64 contractId, Int64 salesRepId, String name, String description, DateTime startDate, DateTime endDate) 
        {
            return new Contract();
        }

        public async Task<ContractLocation> EnrollLocation(Contract contract, Location loc, SalesRep salesRep, IMSUser ims_user, IMSEntities db) 
        { 
            ContractLocation enrollLocation = new ContractLocation();

            enrollLocation.ContractId = contract.Id;
            enrollLocation.LocationId = loc.Id;

            ContractLocation existingEnrolledContract = await db.ContractLocations.Where(a => a.LocationId == loc.Id && a.IsActive == true).OrderByDescending(b => b.Id).FirstOrDefaultAsync();

            if (existingEnrolledContract != null)
            {
                if (String.IsNullOrEmpty(existingEnrolledContract.TransaxEnterpriseContractId) && String.IsNullOrEmpty(existingEnrolledContract.TransaxSalesRepContractId)) 
                {
                    db.Entry(existingEnrolledContract).State = EntityState.Deleted;
                    await db.SaveChangesAsync();

                    var commandAdd = DataCommandFactory.AddEnrollLocationCommand(enrollLocation, salesRep.TransaxId, loc.TransaxId, db);

                    var resultAdd = await commandAdd.Execute();

                    if (resultAdd != DataCommandResult.Success)
                    {
                        enrollLocation.Location = loc;
                        var command5 = DataCommandFactory.DeleteEnrollLocationCommand(enrollLocation, db);
                        var result5 = await command5.Execute();

                        throw new Exception(resultAdd.ToString());
                    }
                }
                else 
                {
                    if (existingEnrolledContract.ContractId != contract.Id) 
                    {
                        enrollLocation.CreationDate = DateTime.Now;
                        enrollLocation.IsActive = true;
                        enrollLocation.TransaxEnterpriseContractId = existingEnrolledContract.TransaxEnterpriseContractId;
                        enrollLocation.TransaxSalesRepContractId = existingEnrolledContract.TransaxSalesRepContractId;

                        db.ContractLocations.Add(enrollLocation);
                        try 
                        {
                            await db.SaveChangesAsync();
                        }
                        catch (Exception ex) 
                        {
                            logger.ErrorFormat("ContractManager - Unable to add ContractLocation - ContractId {0} LocationId {1} Exception {2} InnerException {3}", contract.Id, loc.Id, ex.ToString(), ex.InnerException.ToString());
                            return null;
                        }
                        

                        existingEnrolledContract.IsActive = false;
                        db.Entry(existingEnrolledContract).State = EntityState.Modified;
                        try 
                        {
                            await db.SaveChangesAsync();
                        }
                        catch (Exception ex) 
                        {
                            logger.ErrorFormat("ContractManager - Unable to set inactive old contract - ContractId {0} Exception {1} InnerException {2}", existingEnrolledContract.Id, ex.ToString(), ex.InnerException.ToString());
                            return null;
                        }
                    }

                    //var commandUpdate = DataCommandFactory.UpdateEnrollLocationCommand(enrollLocation, ims_user, salesRep.TransaxId, db);

                    //var resultUpdate = await commandUpdate.Execute();

                    //if (resultUpdate != DataCommandResult.Success)
                    //{
                    //    throw new Exception(resultUpdate.ToString());
                    //}
                }
            }
            else
            {
                var commandAdd = DataCommandFactory.AddEnrollLocationCommand(enrollLocation, salesRep.TransaxId, loc.TransaxId, db);

                var resultAdd = await commandAdd.Execute();

                if (resultAdd != DataCommandResult.Success)
                {
                    enrollLocation.Location = loc;
                    var command5 = DataCommandFactory.DeleteEnrollLocationCommand(enrollLocation, db);
                    var result5 = await command5.Execute();

                    throw new Exception(resultAdd.ToString());
                }
            }

            return enrollLocation;
                
        }
    }
}
