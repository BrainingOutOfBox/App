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
        [SetUp]
        public void SetUp()
        {

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
            //IBrainstormingMapper testee = new BrainstormingMappingProfile();
            //var outputBo = testee.MapFromDto(inputFinding);
            //Assert.AreEqual(outputBo.Id, targetFinding.Id);
            //Assert.AreEqual(outputBo.BaseRoundTime, targetFinding.BaseRoundTime);
            //Assert.AreEqual(outputBo.CurrentRound, targetFinding.CurrentRound);
            //Assert.AreEqual(outputBo.Name, targetFinding.Name);
            //Assert.AreEqual(outputBo.NrOfIdeas, targetFinding.NrOfIdeas);
            //Assert.AreEqual(outputBo.ProblemDescription, targetFinding.ProblemDescription);
            //Assert.AreEqual(outputBo.TeamId, targetFinding.TeamId);
        }
    }
}
