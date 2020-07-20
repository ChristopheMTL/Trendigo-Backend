using AutoMapper;
using IMS.Common.Core.Data;
using IMS.Service.WebAPI2.Bindings;
using IMS.Service.WebAPI2.Filters;
using IMS.Service.WebAPI2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace IMS.Service.WebAPI2.Controllers
{
    [RoutePrefix("creditCards")]
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
    public class CreditCardController : ApiController
    {
        private IMSEntities db = new IMSEntities();

        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpGet]
        [JwtAuthentication]
        [Route("creditCardTypes")]
        public async Task<IHttpActionResult> GetCreditCardTypes([fromHeader] string locale = "en")
        {
            List<CreditCardTypeRS> CreditCardTypes = new List<CreditCardTypeRS>();

            List<CreditCardType> ccTypes = await db.CreditCardTypes.ToListAsync();

            if (ccTypes == null)
            {
                return Content(HttpStatusCode.OK, CreditCardTypes);
            }

            CreditCardTypes = Mapper.Map<List<CreditCardTypeRS>>(ccTypes);
            return Content(HttpStatusCode.OK, CreditCardTypes);
        }

        [HttpGet]
        [JwtAuthentication]
        [Route("creditCardIssuers")]
        public async Task<IHttpActionResult> GetCreditCardIssuers([fromHeader] string locale = "en")
        {
            List<CreditCardIssuerRS> CreditCardIssuers = new List<CreditCardIssuerRS>();

            List<CreditCardIssuer> ccIssuers = await db.CreditCardIssuers.Where(a => a.IsActive == true).ToListAsync();

            if (ccIssuers == null)
            {
                return Content(HttpStatusCode.OK, CreditCardIssuers);
            }

            CreditCardIssuers = Mapper.Map<List<CreditCardIssuerRS>>(ccIssuers);
            return Content(HttpStatusCode.OK, CreditCardIssuers);
        }
    }
}