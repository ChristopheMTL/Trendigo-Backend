using IMS.Common.Core.Data;
using IMS.Common.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Services
{
    public class DeviceManager
    {
        IMSEntities context = new IMSEntities();
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DeviceType GetDeviceType(int deviceTypeId) 
        {
            DeviceType deviceType = context.DeviceTypes.Where(a => a.Id == deviceTypeId).FirstOrDefault();
            return deviceType;
        }

        public Boolean CanRequestDevice(long memberId, long membershipId, int deviceTypeId) 
        {
            List<DeviceRequest> deviceRequest = context.DeviceRequests.Where(a => a.MemberId == memberId && a.IMSMembershipId == membershipId && a.DeviceTypeId == deviceTypeId && a.DeviceRequestStatutId != (int)DeviceRequestStatus.REJECTED).ToList();

            //The hardcoded value of 2 should be implemented in the IMSEnterpriseParameter table
            if (deviceRequest != null && deviceRequest.Count() >= 2)
            {
                return false;
            }

            return true;
        }

        private List<DeviceRequest> GetDeviceRequest(long memberId, long membershipId, int deviceTypeId) 
        {
            List<DeviceRequest> deviceRequest = context.DeviceRequests.Where(a => a.MemberId == memberId && a.IMSMembershipId == membershipId && a.DeviceTypeId == deviceTypeId && a.DeviceRequestStatutId != (int)DeviceRequestStatus.REJECTED).ToList();
            return deviceRequest;
        }
    }
}
