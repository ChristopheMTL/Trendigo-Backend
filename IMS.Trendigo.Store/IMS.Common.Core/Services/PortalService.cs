using IMS.Common.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Services
{
    public class PortalService
    {
        IMSEntities context = new IMSEntities();
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IMSEnterpriseParameter GetPortalWithId(long portalId) 
        {
            IMSEnterpriseParameter portal = context.IMSEnterpriseParameters.FirstOrDefault(a => a.Id == portalId);

            return portal;
        }

        public IMSEnterpriseParameter GetPortalWithTransaxEnterpriseId(long trxEnterpriseId)
        {
            IMSEnterpriseParameter portal = context.IMSEnterpriseParameters.FirstOrDefault(a => a.EnterpriseId == trxEnterpriseId);

            return portal;
        }

    }
}
