using AutoMapper;
using Method635.App.Dal.Mapping;
using Method635.App.Dal.Mapping.Mappers;
using Method635.App.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Method635.App.Tests.MappingTests
{
    public class TeamMappingTest
    {
        private IMapper _mapper;

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
        public void TestTeamMapping()
        {
            var teamDto = new BrainstormingTeamDto()
            {
                CurrentNrOfParticipants = 1,
                Id = "aoijao",
                Moderator = new ModeratorDto()
                {
                    FirstName = "o",
                    LastName = "d",
                    Password = "cleartextpw",
                    UserName = "od"
                },
                Name = "teamy",
                NrOfParticipants = 1,
                Participants = new List<ParticipantDto>(),
                Purpose = "funteam"
            };
            var teamBo = _mapper.Map<BrainstormingTeam>(teamDto);
            Assert.IsNotNull(teamBo);
            Assert.AreEqual(teamDto.Id, teamBo.Id);
            Assert.AreEqual(teamDto.CurrentNrOfParticipants, teamBo.CurrentNrOfParticipants);
            Assert.AreEqual(teamDto.Name, teamBo.Name);
            Assert.AreEqual(teamDto.NrOfParticipants, teamBo.NrOfParticipants);
            Assert.AreEqual(teamDto.Purpose, teamBo.Purpose);
            Assert.AreEqual(teamDto.Participants, teamBo.Participants);
            Assert.AreEqual(teamDto.Moderator.FirstName, teamBo.Moderator.FirstName);
            Assert.AreEqual(teamDto.Moderator.LastName, teamBo.Moderator.LastName);
            Assert.AreEqual(teamDto.Moderator.Password, teamBo.Moderator.Password);
            Assert.AreEqual(teamDto.Moderator.UserName, teamBo.Moderator.UserName);
        }


        [Test]
        public void TestTeamListMapping()
        {
            var teamsDto = new List<BrainstormingTeamDto>(){
                new BrainstormingTeamDto()
                {
                    CurrentNrOfParticipants = 1,
                    Id = "aoijao",
                    Moderator = new ModeratorDto()
                    {
                        FirstName = "o",
                        LastName = "d",
                        Password = "cleartextpw",
                        UserName = "od"
                    },
                    Name = "teamy",
                    NrOfParticipants = 1,
                    Participants = new List<ParticipantDto>(),
                    Purpose = "funteam"
                }
            };

            var teamsBo = _mapper.Map<List<BrainstormingTeam>>(teamsDto);
            Assert.IsNotNull(teamsBo);
            Assert.AreEqual(teamsDto.Count, teamsBo.Count);
            Assert.AreEqual(teamsDto[0].Id, teamsBo[0].Id);
            Assert.AreEqual(teamsDto[0].CurrentNrOfParticipants, teamsBo[0].CurrentNrOfParticipants);
            Assert.AreEqual(teamsDto[0].Name, teamsBo[0].Name);
            Assert.AreEqual(teamsDto[0].NrOfParticipants, teamsBo[0].NrOfParticipants);
            Assert.AreEqual(teamsDto[0].Purpose, teamsBo[0].Purpose);
            Assert.AreEqual(teamsDto[0].Participants, teamsBo[0].Participants);
            Assert.AreEqual(teamsDto[0].Moderator.FirstName, teamsBo[0].Moderator.FirstName);
            Assert.AreEqual(teamsDto[0].Moderator.LastName, teamsBo[0].Moderator.LastName);
            Assert.AreEqual(teamsDto[0].Moderator.Password, teamsBo[0].Moderator.Password);
            Assert.AreEqual(teamsDto[0].Moderator.UserName, teamsBo[0].Moderator.UserName);
        }
    }
}
