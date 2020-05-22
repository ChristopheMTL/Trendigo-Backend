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
                map.ForMember(x => x.name, o => o.MapFrom(model => model.ShortDescription));

                communities = Mapper.Map<List<CommunityDTO>>(programs);
            }

            return communities;
        }
    }
}
