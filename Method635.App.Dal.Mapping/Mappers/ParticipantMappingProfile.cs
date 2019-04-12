using AutoMapper;
using Method635.App.Models;
using System.Collections.Generic;

namespace Method635.App.Dal.Mapping.Mappers
{
    public class ParticipantMappingProfile : Profile
    {
        public ParticipantMappingProfile()
        {
            CreateMap<ParticipantDto, Participant>()
                .Include<ModeratorDto,Moderator>();
            CreateMap<Participant, ParticipantDto>()
                .Include<Moderator, ModeratorDto>();

            CreateMap<Moderator, ModeratorDto>();
            CreateMap<ModeratorDto, Moderator>();
        }
    }
}
