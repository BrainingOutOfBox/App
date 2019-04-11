using AutoMapper;
using Method635.App.Models;
using System.Collections.Generic;

namespace Method635.App.Dal.Mapping.Mappers
{
    public class ParticipantMappingProfile : Profile
    {
        public ParticipantMappingProfile()
        {
            CreateMap<ParticipantDto, Participant>();
            CreateMap<Participant, ParticipantDto>();

            CreateMap<List<ParticipantDto>, List<Participant>>();
            CreateMap<List<Participant>, List<ParticipantDto>>();

            CreateMap<Moderator, ModeratorDto>();
            CreateMap<ModeratorDto, Moderator>();
        }
    }
}
