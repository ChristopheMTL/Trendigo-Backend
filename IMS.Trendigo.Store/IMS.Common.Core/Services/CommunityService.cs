using AutoMapper;
using IMS.Common.Core.Data;
using IMS.Common.Core.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Services
{
    public class CommunityService
    {
        private IMSEntities db = new IMSEntities();

        public async Task<List<CommunityDTO>> GetCommunities()
        {
            List<Program> programs = await db.Programs.Where(a => a.IsActive == true).ToListAsync();

            List<CommunityDTO> communities = new List<CommunityDTO>();

            if (programs.Count() > 0)
            {
                var map = Mapper.CreateMap<Program, CommunityDTO>();
                map.ForMember(x => x.communityId, o => o.MapFrom(model => model.Id));
                map.ForMember(x => x.name, o => o.MapFrom(model => model.Description));
                map.ForMember(x => x.communityTypeId, o => o.MapFrom(model => model.ProgramTypeId));

                communities = Mapper.Map<List<CommunityDTO>>(programs);
            }

            return communities;
        }

        public async Task<List<CommunityTypeDTO>> GetCommunityTypes()
        {
            List<ProgramType> programTypes = await db.ProgramTypes.ToListAsync();

            List<CommunityTypeDTO> communityTypes = new List<CommunityTypeDTO>();

            if (programTypes.Count() > 0)
            {
                var map1 = Mapper.CreateMap<ProgramTypeFee, CommunityTypeFeeDTO>();
                map1.ForMember(x => x.currency, o => o.MapFrom(model => model.Currency.Code));
                map1.ForMember(x => x.amount, o => o.MapFrom(model => model.Amount));

                var map = Mapper.CreateMap<ProgramType, CommunityTypeDTO>();
                map.ForMember(x => x.communityTypeId, o => o.MapFrom(model => model.Id));
                map.ForMember(x => x.description, o => o.MapFrom(model => model.Description));
                map.ForMember(x => x.fees, o => o.MapFrom(model => model.ProgramTypeFees));

                communityTypes = Mapper.Map<List<CommunityTypeDTO>>(programTypes);
            }

            return communityTypes;
        }
    }
}
