using IMS.Common.Core.Data;
using IMS.Common.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Services
{
    public class DeviceRequestService
    {
        IMSEntities db = new IMSEntities();

        public DeviceRequest AddDeviceRequest(long memberId, long membershipId, Int32 deviceTypeId) 
        {
            DeviceRequest request = new DeviceRequest();
            request.MemberId = memberId;
            request.IMSMembershipId = membershipId;
            request.DeviceRequestStatutId = (int)DeviceRequestStatus.REQUESTED;
            request.DeviceTypeId = deviceTypeId;
            request.CreationDate = DateTime.Now;

            try
            {
                db.DeviceRequests.Add(request);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return request;
        }

        public void UpdateDeviceRequestStatus(long memberId, int actualDeviceRequestStatusId, int newDeviceRequestStatusId)
        {
            DeviceRequest deviceReq = db.DeviceRequests.FirstOrDefault(a => a.MemberId == memberId && a.DeviceRequestStatutId == actualDeviceRequestStatusId);

            if (deviceReq != null)
            {
                deviceReq.DeviceRequestStatutId = newDeviceRequestStatusId;
                deviceReq.ModificationDate = DateTime.Now;
                db.Entry(deviceReq).State = System.Data.Entity.EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
        }
    }
}
