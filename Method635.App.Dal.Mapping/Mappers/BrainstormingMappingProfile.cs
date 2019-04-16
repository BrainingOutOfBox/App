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
            MapBrainstormingFindings();
            MapBrainSheets();
            MapBrainWaves();
            MapIdeas();
        }

        private void MapIdeas()
        {
            CreateMap<Idea, IdeaDto>()
                .Include<NoteIdea, NoteIdeaDto>()
                .Include<SketchIdea, SketchIdeaDto>()
                .Include<PatternIdea, PatternIdeaDto>();

            CreateMap<NoteIdea, NoteIdeaDto>();
            CreateMap<SketchIdea, SketchIdeaDto>();
            CreateMap<PatternIdea, PatternIdeaDto>();

            CreateMap<IdeaDto, Idea>()
                .Include<NoteIdeaDto, NoteIdea>()
                .Include<SketchIdeaDto, SketchIdea>()
                .Include<PatternIdeaDto, PatternIdea>();

            CreateMap<NoteIdeaDto, NoteIdea>();
            CreateMap<SketchIdeaDto, SketchIdea>();
            CreateMap<PatternIdeaDto, PatternIdea>();
        }

        private void MapBrainWaves()
        {
            CreateMap<BrainWave, BrainWaveDto>();
            CreateMap<BrainWaveDto, BrainWave>();
        }

        private void MapBrainSheets()
        {
            CreateMap<BrainSheetDto, BrainSheet>();
            CreateMap<BrainSheet, BrainSheetDto>();
        }

        private void MapBrainstormingFindings()
        {
            CreateMap<BrainstormingFindingDto, BrainstormingFinding>();
            CreateMap<BrainstormingFinding, BrainstormingFindingDto>();
        }
    }
}
