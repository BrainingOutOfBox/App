using AutoMapper;
using Method635.App.Dal.Mapping;
using Method635.App.Dal.Mapping.Mappers;
using Method635.App.Models;
using Method635.App.Tests.Factories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Method635.App.Tests.MappingTests
{
    class BrainstormingMapperTest
    {
        public IMapper _mapper { get; private set; }

        [SetUp]
        public void SetUp()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BrainstormingMappingProfile());
                cfg.AddProfile(new ParticipantMappingProfile());
                cfg.AddProfile(new TeamMappingProfile());
            }
            );
            _mapper = config.CreateMapper();

        }

        [Test]
        public void TestBrainstormingDto()
        {
            var targetFinding = new BrainstormingFinding()
            {
                Id = "123",
                BaseRoundTime = 1,
                BrainSheets = BrainstormingModelFactory.CreateBrainSheets(),
                CurrentRound = 1,
                Name = "TestFinding",
                NrOfIdeas = 4,
                ProblemDescription = "Alterpalaber",
                TeamId = "abc"
            };
            var inputFinding = new BrainstormingFindingDto()
            {
                Id = "123",
                BaseRoundTime = 1,
                BrainSheets = new List<BrainSheetDto>(),
                CurrentRound = 1,
                Name = "TestFinding",
                NrOfIdeas = 4,
                ProblemDescription = "Alterpalaber",
                TeamId = "abc"
            };

            var outputBo = _mapper.Map<BrainstormingFinding>(inputFinding);
            Assert.AreEqual(targetFinding.Id, outputBo.Id);
            Assert.AreEqual(targetFinding.BaseRoundTime, outputBo.BaseRoundTime);
            Assert.AreEqual(targetFinding.CurrentRound, outputBo.CurrentRound);
            Assert.AreEqual(targetFinding.Name, outputBo.Name);
            Assert.AreEqual(targetFinding.NrOfIdeas, outputBo.NrOfIdeas);
            Assert.AreEqual(targetFinding.ProblemDescription, outputBo.ProblemDescription);
            Assert.AreEqual(targetFinding.TeamId, outputBo.TeamId);
        }
    }
}
