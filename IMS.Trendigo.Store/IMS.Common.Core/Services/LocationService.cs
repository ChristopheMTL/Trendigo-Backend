using IMS.Common.Core.Data;
using IMS.Common.Core.Enumerations;
using IMS.Common.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using IMS.Common.Core.DTO;

namespace IMS.Common.Core.Services
{
    public class LocationService
    {
        IMSEntities context = new IMSEntities();
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public List<Location> GetLocationListWithRole(IMSUser user, String EnterpriseId, Boolean ActiveOnly = false)
        {
            List<Location> locations = GetLocationListWithRole(user, ActiveOnly);

            return locations.Where(a => a.Merchant.Enterprises.Any(b => b.Id.ToString() == EnterpriseId)).ToList();
        }

        public List<Location> GetLocationListWithRole(IMSUser user, Boolean ActiveOnly = false)
        {
            List<Location> locations = new List<Location>();

            if (HttpContext.Current.User.IsInRole(IMSRole.IMSAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.IMSSupport.ToString())
                || HttpContext.Current.User.IsInRole(IMSRole.IMSAccounting.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.IMSUser.ToString()))
            {
                locations = context.Locations.ToList();
                if (ActiveOnly)
                    locations = locations.Where(a => a.IsActive == true).ToList();
                return locations;
            }

            if (HttpContext.Current.User.IsInRole(IMSRole.SponsorAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.SponsorUser.ToString()))
            {
                locations = context.Locations.Where(a => a.Merchant.Enterprises.Any(b => b.Id == user.Id)).Distinct().ToList();
            }

            if (HttpContext.Current.User.IsInRole(IMSRole.MerchantAdmin.ToString()) || HttpContext.Current.User.IsInRole(IMSRole.MerchantUser.ToString()))
            {
                locations = context.Locations.Where(a => a.IMSUsers.Any(c => c.Id == user.Id)).Distinct().ToList();
            }

            if (ActiveOnly)
                locations = locations.Where(a => a.IsActive == true).ToList();

            return locations;
        }

        public Location GetLocationWithRole(IMSUser user, String locationId)
        {
            List<Location> locations = GetLocationListWithRole(user);
            var listOfLocationId = locations.Select(m => m.Id);

            return context.Locations.Where(a => a.Id.ToString() == locationId).Where(a => listOfLocationId.Contains(a.Id)).FirstOrDefault();
        }

        public async Task AddLocationMemberDistance() 
        {
            List<Int64> members = await context.Members.Select(a => a.Id).ToListAsync();
            List<Int64> locations = await context.Locations.Select(a => a.Id).ToListAsync();

            List<LocationMemberDistanceDTO> lmd_new = new List<LocationMemberDistanceDTO>();
            foreach (Int64 member in members)
            {
                foreach (Int64 location in locations)
                {
                    LocationMemberDistanceDTO lmd = new LocationMemberDistanceDTO();
                    lmd.locationId = location;
                    lmd.memberId = member;
                    lmd_new.Add(lmd);
                }
            }

            List<LocationMemberDistance> lmds = await context.LocationMemberDistances.ToListAsync();
            List<LocationMemberDistanceDTO> lmd_exist = new List<LocationMemberDistanceDTO>();

            foreach (LocationMemberDistance lmd in lmds)
            {
                LocationMemberDistanceDTO lmd2 = new LocationMemberDistanceDTO();
                lmd2.locationId = lmd.LocationId;
                lmd2.memberId = lmd.MemberId;
                lmd_exist.Add(lmd2);
            }

            List<LocationMemberDistanceDTO> firstNotSecond = lmd_new.Except(lmd_exist).ToList();
            List<LocationMemberDistanceDTO> secondNotFirst = lmd_exist.Except(lmd_new).ToList();

            lmd_new = firstNotSecond.Where(l2 => !secondNotFirst.Any(l1 => l1.memberId == l2.memberId && l1.locationId == l2.locationId)).ToList();

            //try
            //{
            //    await new LocationService().UpdateLocationMemberDistance(lmd_new);
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(string.Format("LocationService - UpdateLocationMemberDistance Exception {0}", ex.InnerException.ToString()));
            //}
        }

        //public async Task UpdateLocationMemberDistance(Int64? locationId = null, Int64? memberId = null)
        //{
        //    List<Location> locations = await context.Locations.Where(a => a.Address.Longitude != null && a.Address.Latitude != null).ToListAsync();
        //    if (locationId.HasValue)
        //        locations = locations.Where(a => a.Id == locationId.Value).ToList();
        //    List<Member> members = await context.Members.Where(a => a.Address.Longitude != null && a.Address.Latitude != null).ToListAsync();
        //    if (memberId.HasValue)
        //        members = members.Where(a => a.Id == memberId.Value).ToList();
        //    List<LocationMemberDistance> lmds_exist = await context.LocationMemberDistances.ToListAsync();
        //    List<LocationMemberDistance> lmds_new = new List<LocationMemberDistance>();

        //    Double? distance = null;

        //    foreach (Location location in locations)
        //    {
        //        foreach (Member member in members)
        //        {
        //            LocationMemberDistance lmd_exist = lmds_exist.Where(a => a.MemberId == member.Id && a.LocationId == location.Id).FirstOrDefault();

        //            if (lmd_exist != null)
        //                continue;

        //            try
        //            {
        //                distance = null;
        //                distance = new UtilityManager().HaversineDistance(Convert.ToDouble(member.Address.Longitude), Convert.ToDouble(member.Address.Latitude), Convert.ToDouble(location.Address.Longitude), Convert.ToDouble(location.Address.Latitude), DistanceUnit.Kilometers);
        //            }
        //            catch (Exception ex)
        //            {
        //                logger.ErrorFormat("LocationService HaversineDistance MemberId {0} LocationId {1} Exception {2}", member.Id.ToString(), location.Id.ToString(), ex.InnerException.ToString());
        //                continue;
        //            }

        //            if (distance.HasValue)
        //            {
        //                LocationMemberDistance lmd = new LocationMemberDistance();
        //                lmd.Distance = Convert.ToDecimal(distance.Value);
        //                lmd.LocationId = location.Id;
        //                lmd.MemberId = member.Id;

        //                context.LocationMemberDistances.Add(lmd);
        //            }
        //        }
        //    }

        //    try
        //    {
        //        await context.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.ErrorFormat("LocationService AddLocationMemberDistances Exception {0}", ex.InnerException.ToString());
        //        throw new Exception(string.Format("LocationService AddLocationMemberDistances Exception {0}", ex.InnerException.ToString()));
        //    }
        //}

        //public async Task UpdateLocationMemberDistance(List<LocationMemberDistanceDTO> lmds)
        //{
        //    List<Location> locations = await context.Locations.Where(a => a.Address.Longitude != null && a.Address.Latitude != null).ToListAsync();
        //    List<Member> members = await context.Members.Where(a => a.Address.Longitude != null && a.Address.Latitude != null).ToListAsync();
        //    List<LocationMemberDistance> lmds_exist = await context.LocationMemberDistances.ToListAsync();

        //    Double? distance = null;

        //    foreach (LocationMemberDistanceDTO lmd in lmds)
        //    {
        //        #region Validation Section

        //        if (lmd.locationId == null || lmd.memberId == null)
        //            continue;

        //        Location location = locations.Where(a => a.Id == lmd.locationId).FirstOrDefault();

        //        if (location == null)
        //            throw new Exception(string.Format("location not found for locationId {0}", lmd.locationId));

        //        Member member = members.Where(a => a.Id == lmd.memberId).FirstOrDefault();

        //        if (member == null)
        //            throw new Exception(string.Format("member not found for memberId {0}", lmd.memberId));

        //        LocationMemberDistance lmd_exist = lmds_exist.Where(a => a.MemberId == member.Id && a.LocationId == location.Id).FirstOrDefault();

        //        if (lmd_exist != null)
        //            continue;

        //        if (member.Address == null || location.Address == null)
        //            continue;

        //        if (member.Address.Longitude == null || member.Address.Latitude == null || location.Address.Longitude == null || location.Address.Latitude == null)
        //            continue;

        //        #endregion

        //        try
        //        {
        //            distance = null;
        //            distance = new UtilityManager().HaversineDistance(Convert.ToDouble(member.Address.Longitude), Convert.ToDouble(member.Address.Latitude), Convert.ToDouble(location.Address.Longitude), Convert.ToDouble(location.Address.Latitude), DistanceUnit.Kilometers);
        //        }
        //        catch (Exception ex)
        //        {
        //            logger.ErrorFormat("LocationService HaversineDistance MemberId {0} LocationId {1} Exception {2}", member.Id.ToString(), location.Id.ToString(), ex.InnerException.ToString());
        //            //throw new Exception(string.Format("LocationService HaversineDistance MemberId {0} LocationId {1} Exception {2}", member.Id.ToString(), location.Id.ToString(), ex.InnerException.ToString()));
        //            continue;
        //        }

        //        if (distance.HasValue)
        //        {
        //            LocationMemberDistance lmd_new = new LocationMemberDistance();
        //            lmd_new.MemberId = lmd.memberId;
        //            lmd_new.LocationId = lmd.locationId;
        //            lmd_new.Distance = Convert.ToDecimal(distance.Value);

        //            context.LocationMemberDistances.Add(lmd_new);
        //        }

        //        await Task.Delay(10);
        //    }

        //    try
        //    {
        //        await context.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.ErrorFormat("LocationService AddLocationMemberDistances Exception {0}", ex.InnerException.ToString());
        //        throw new Exception(string.Format("LocationService AddLocationMemberDistances Exception {0}", ex.InnerException.ToString()));
        //    }
        //}
    }
}
