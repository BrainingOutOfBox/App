using AutoMapper;
using Method635.App.Models;
using System.Collections.Generic;

namespace Method635.App.Dal.Mapping.Mappers
{
    public class TeamMappingProfile : Profile
    {
        public TeamMappingProfile()
        {
            CreateMap<BrainstormingTeamDto, BrainstormingTeam>();
            CreateMap<BrainstormingTeam, BrainstormingTeamDto>();

            CreateMap<List<BrainstormingTeamDto>, List<BrainstormingTeam>>();
            CreateMap<List<BrainstormingTeam>, List<BrainstormingTeamDto>>();

        }
    }
}
