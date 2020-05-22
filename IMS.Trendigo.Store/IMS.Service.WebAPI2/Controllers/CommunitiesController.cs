using IMS.Common.Core.Data;
using IMS.Common.Core.DTO;
using IMS.Common.Core.Services;
using IMS.Service.WebAPI2.Bindings;
using IMS.Service.WebAPI2.Filters;
using IMS.Service.WebAPI2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace IMS.Service.WebAPI2.Controllers
{
    [RoutePrefix("communities")]
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
    public class CommunitiesController : ApiController
    {
        private readonly IMSEntities db = new IMSEntities();

        //[HttpPost]
        //[JwtAuthentication]
        //public async Task<IHttpActionResult> AddCommunity([FromBody] [fromHeader] string locale = "en")
        //{
        //    List<CommunityDTO> communities = new List<CommunityDTO>();

        //    CommunityService communityservice = new CommunityService();

        //    try
        //    {
        //        communities = await communityservice.GetCommunities();
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToRetrieveTheCommunity_", locale));
        //    }

        //    return Content(HttpStatusCode.OK, communities);
        //}

        [HttpGet]
        [JwtAuthentication]
        public async Task<IHttpActionResult> GetCommunities([fromHeader] string locale = "en")
        {
            List<CommunityDTO> communities = new List<CommunityDTO>();

            CommunityService communityservice = new CommunityService();

            try
            {
                communities = await communityservice.GetCommunities();
            }
            catch(Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToRetrieveTheCommunity_", locale));
            }

            return Content(HttpStatusCode.OK, communities);
        }
    }
}
