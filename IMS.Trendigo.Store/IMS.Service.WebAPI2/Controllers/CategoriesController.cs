using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using IMS.Common.Core.Services;
using IMS.Common.Core.Data;
using IMS.Common.Core.DTO;
using System.Web.Http.Cors;
using IMS.Service.WebAPI2.Filters;
using IMS.Service.WebAPI2.Bindings;
using IMS.Service.WebAPI2.Services;

namespace IMS.Service.WebAPI2.Controllers
{
    [RoutePrefix("categories")]
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
    public class CategoriesController : ApiController
    {
        private readonly IMSEntities db = new IMSEntities();

        [HttpGet]
        [JwtAuthentication]
        public async Task<IHttpActionResult> GetCategories([fromHeader] string locale = "en")
        {
            List<CategoryDTO> categories = new List<CategoryDTO>();

            TagService tagservice = new TagService();

            try
            {
                categories = await tagservice.GetCategories();
            }
            catch(Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToRetrieveTheCategory_", locale));
            }

            return Content(HttpStatusCode.OK, categories);
        }

        [HttpGet]
        [Route("{categoryId:int}/getTagsByCategory")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> GetTagsByCategory(int categoryId, [fromHeader] string locale = "en")
        {
            List<CategoryDTO> tags = new List<CategoryDTO>();

            TagService tagservice = new TagService();

            try
            {
                tags = await tagservice.GetTags(categoryId);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToRetrieveTheTag_", locale));
            }

            return Content(HttpStatusCode.OK, tags);
        }
    }
}
