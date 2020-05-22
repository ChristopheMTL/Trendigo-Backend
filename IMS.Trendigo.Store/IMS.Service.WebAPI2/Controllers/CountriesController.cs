using AutoMapper;
using IMS.Common.Core.Data;
using IMS.Common.Core.DTO;
using IMS.Common.Core.Services;
using IMS.Service.WebAPI2.Bindings;
using IMS.Service.WebAPI2.Filters;
using IMS.Service.WebAPI2.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace IMS.Service.WebAPI2.Controllers
{
    [RoutePrefix("countries")]
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
    public class CountriesController : ApiController
    {
        private readonly IMSEntities db = new IMSEntities();

        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetCountries([fromHeader] string locale = "en")
        {
            List<Country> countries = new List<Country>();
            List<CountryDTO> countriesDTO = new List<CountryDTO>();

            try
            {
                countries = await db.Countries.Where(a => a.IsActive == true).ToListAsync();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToRetrieveTheCountry_", locale));
            }

            countriesDTO = Mapper.Map<List<Country>, List<CountryDTO>>(countries);

            return Content(HttpStatusCode.OK, countriesDTO);
        }

        [HttpGet]
        [Route("{countryId:int}/getStates")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetStatesByCountries(int countryId, [fromHeader] string locale = "en")
        {
            List<State> states = new List<State>();
            List<StateDTO> statesDTO = new List<StateDTO>();

            TagService tagservice = new TagService();

            try
            {
                states = await db.States.Where(a => a.CountryId == countryId && a.IsActive == true).ToListAsync();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToRetrieveTheState_", locale));
            }

            statesDTO = Mapper.Map<List<State>, List<StateDTO>>(states);

            return Content(HttpStatusCode.OK, statesDTO);
        }
    }
}