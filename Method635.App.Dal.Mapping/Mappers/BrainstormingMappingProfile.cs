using System.Collections.Generic;
using AutoMapper;
using Method635.App.Dal.Mapping.DTO;
using Method635.App.Models;
using Method635.App.Models.Models;

namespace Method635.App.Dal.Mapping.Mappers
{
    public class BrainstormingMappingProfile : Profile
    {
        public BrainstormingMappingProfile()
        {
            CreateMap<BrainstormingFindingDto, BrainstormingFinding>();
            CreateMap<BrainstormingFinding, BrainstormingFindingDto>();

            CreateMap<List<BrainstormingFindingDto>, List<BrainstormingFinding>>();
            CreateMap<List<BrainstormingFinding>, List<BrainstormingFindingDto>>();

            CreateMap<BrainSheetDto, BrainSheet>();
            CreateMap<BrainSheet, BrainSheetDto>();

            CreateMap<Idea, IdeaDto>()
                .Include<TextIdea, TextIdeaDto>()
                .Include<SketchIdea, SketchIdeaDto>()
                .Include<PatternIdea, PatternIdeaDto>();

            CreateMap<TextIdea, TextIdeaDto>();
            CreateMap<SketchIdea, SketchIdeaDto>();
            CreateMap<PatternIdea, PatternIdeaDto>();

            CreateMap<IdeaDto, Idea>()
                .Include<TextIdeaDto, TextIdea>()
                .Include<SketchIdeaDto, SketchIdea>()
                .Include<PatternIdeaDto, PatternIdea>();

            CreateMap<TextIdeaDto, TextIdea>();
            CreateMap<SketchIdeaDto, SketchIdea>();
            CreateMap<PatternIdeaDto, PatternIdea>();
        }
    }
}
