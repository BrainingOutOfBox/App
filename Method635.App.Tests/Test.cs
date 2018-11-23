using AutoMapper;
using Method635.App.Forms.BusinessModels;
using Method635.App.Forms.Dto;
using Method635.App.Forms.RestAccess.ResponseModel;
using NUnit.Framework;

namespace Method635.App.Tests
{
    [TestFixture]
    public class Test
    {

        [Test]
        public void TestMappingOptions()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<BrainSheetDto, BrainSheet>();
                cfg.CreateMap<BrainstormingFindingDto, BrainstormingFinding>();
                cfg.CreateMap<BrainstormingTeamDto, BrainstormingTeam>();
                cfg.CreateMap<BrainWaveDto, BrainWave>();
                cfg.CreateMap<ModeratorDto, Moderator>();
                cfg.CreateMap<ParticipantDto, Participant>();
                cfg.CreateMap<TextIdeaDto, TextIdea>();
                cfg.CreateMap<RestLoginResponseDto, RestLoginResponse>();
            });
            Mapper.AssertConfigurationIsValid();
        }
    }
}
