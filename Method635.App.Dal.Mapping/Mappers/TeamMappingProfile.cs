using AutoMapper;
using Method635.App.Models;

namespace Method635.App.Dal.Mapping.Mappers
{
    public class TeamMappingProfile : Profile
    {
        public TeamMappingProfile()
        {
            CreateMap<BrainstormingTeamDto, BrainstormingTeam>();
            CreateMap<BrainstormingTeam, BrainstormingTeamDto>();
        }
    }
}
