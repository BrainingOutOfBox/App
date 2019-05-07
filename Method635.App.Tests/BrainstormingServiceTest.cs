using Method635.App.BL;
using Method635.App.Dal.Interfaces;
using Method635.App.Models;
using Method635.App.Models.Models;
using Method635.App.Tests.Factories;
using Method635.App.Tests.Setup;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace Tests
{
    // Making the tests run in parallel sometimes throws exceptions within the mockplatformservices
    //[Parallelizable(ParallelScope.All)]
    public class BrainstormingServiceTest
    {
        [SetUp]
        public void SetUp()
        {
            MockPlatformServices.Init();
        }

        [Test]
        public void WaitingStateTest()
        {
            Mock<IBrainstormingDalService> brainstormingDalMock = BrainstormingDalMock(0);

            Mock<ITeamDalService> teamDalMock = TeamDalMock();

            var dalMock = new Mock<IDalService>();
            dalMock.Setup(serv => serv.BrainstormingDalService).Returns(brainstormingDalMock.Object);
            dalMock.Setup(serv => serv.TeamDalService).Returns(teamDalMock.Object);

            var brainstormingService = new BrainstormingService(
                dalMock.Object,
                BrainstormingModelFactory.CreateContext(0),
                new BrainstormingModel());

            brainstormingService.StartBusinessService();
            Assert.IsTrue(brainstormingService.IsWaiting);
            Assert.IsFalse(brainstormingService.IsRunning);
            Assert.IsFalse(brainstormingService.IsEnded);
        }

        private static Mock<ITeamDalService> TeamDalMock()
        {
            var teamDalMock = new Mock<ITeamDalService>();
            teamDalMock.Setup(req => req.GetModeratorByTeamId(It.IsAny<string>()))
                .Returns(BrainstormingModelFactory.CreateModerator());
            return teamDalMock;
        }

        private static Mock<IBrainstormingDalService> BrainstormingDalMock(int initRound)
        {
            var brainstormingDalMock = new Mock<IBrainstormingDalService>();
            brainstormingDalMock.SetupSequence(request => request.GetFinding(It.IsAny<string>())).
                Returns(BrainstormingModelFactory.CreateFinding(initRound));
            return brainstormingDalMock;
        }

        [Test]
        public void RunningStateTest()
        {
            var brainstormingDalMock = BrainstormingDalMock(3);
            var teamDalMock = TeamDalMock();

            var dalMock = new Mock<IDalService>();
            dalMock.Setup(serv => serv.BrainstormingDalService).Returns(brainstormingDalMock.Object);
            dalMock.Setup(serv => serv.TeamDalService).Returns(teamDalMock.Object);

            var brainstormingService = new BrainstormingService(
                dalMock.Object,
                BrainstormingModelFactory.CreateContext(3),
                new BrainstormingModel());

            brainstormingService.StartBusinessService();
            Assert.IsTrue(brainstormingService.IsRunning);
            Assert.IsFalse(brainstormingService.IsWaiting);
            Assert.IsFalse(brainstormingService.IsEnded);
        }

        [Test]
        public void EndedStateTest()
        {
            var brainstormingDalMock = BrainstormingDalMock(-1);
            var teamDalMock = TeamDalMock();

            var dalMock = new Mock<IDalService>();
            dalMock.Setup(serv => serv.BrainstormingDalService).Returns(brainstormingDalMock.Object);
            dalMock.Setup(serv => serv.TeamDalService).Returns(teamDalMock.Object);

            var brainstormingService = new BrainstormingService(
                dalMock.Object,
                BrainstormingModelFactory.CreateContext(-1),
                new BrainstormingModel());

            brainstormingService.StartBusinessService();
            Assert.IsTrue(brainstormingService.IsEnded);
            Assert.IsFalse(brainstormingService.IsWaiting);
            Assert.IsFalse(brainstormingService.IsRunning);
        }

        [Test]
        public void StartRoundTest()
        {
            var brainstormingDalMock = BrainstormingDalMock(1);
            var teamDalMock = TeamDalMock();

            var dalMock = new Mock<IDalService>();
            dalMock.Setup(serv => serv.BrainstormingDalService).Returns(brainstormingDalMock.Object);
            dalMock.Setup(serv => serv.TeamDalService).Returns(teamDalMock.Object);

            var brainstormingService = new BrainstormingService(
                dalMock.Object,
                BrainstormingModelFactory.CreateContext(0),
                new BrainstormingModel());

            brainstormingService.StartBusinessService();
            Assert.IsTrue(brainstormingService.IsWaiting);
            Thread.Sleep(2600);
            Assert.IsTrue(brainstormingService.IsRunning);
        }

        [Test]
        public void EndRoundTest()
        {
            var brainstormingService = BasicServiceSetup();
            brainstormingService.StartBusinessService();


            Assert.IsTrue(brainstormingService.IsRunning);
            Thread.Sleep(7500);
            Assert.IsTrue(brainstormingService.IsEnded);
        }

        [Test]
        public void RemainingTimeTest()
        {
            var brainstormingDalMock = BrainstormingDalMock(1);
            brainstormingDalMock.SetupSequence(req => req.GetRemainingTime(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(TimeSpan.FromSeconds(10))
                .Returns(TimeSpan.FromSeconds(5));
            var teamDalMock = TeamDalMock();

            var dalMock = new Mock<IDalService>();
            dalMock.Setup(serv => serv.BrainstormingDalService).Returns(brainstormingDalMock.Object);
            dalMock.Setup(serv => serv.TeamDalService).Returns(teamDalMock.Object);

            var brainstormingService = new BrainstormingService(
                dalMock.Object,
                BrainstormingModelFactory.CreateContext(1),
                new BrainstormingModel());

            brainstormingService.StartBusinessService();
            Thread.Sleep(1100);
            var remainingTime = brainstormingService.RemainingTime;
            Thread.Sleep(1100);
            Assert.IsTrue(remainingTime > brainstormingService.RemainingTime);
        }

        [Test]
        public void TestSendBrainWave()
        {
            BrainstormingService brainstormingService = BasicServiceSetup();
            brainstormingService.StartBusinessService();

            Assert.IsFalse(brainstormingService.BrainWaveSent);
            brainstormingService.SendBrainWave();
            Assert.IsTrue(brainstormingService.BrainWaveSent);
            Assert.IsTrue(brainstormingService.IsRunning);
            Thread.Sleep(7000);
            Assert.IsTrue(brainstormingService.IsEnded);
        }

        private static BrainstormingService BasicServiceSetup()
        {
            var brainstormingDalMock = new Mock<IBrainstormingDalService>();
            brainstormingDalMock.SetupSequence(request => request.GetFinding(It.IsAny<string>()))
                .Returns(BrainstormingModelFactory.CreateFinding(1))
                .Returns(BrainstormingModelFactory.CreateFinding(-1));

            brainstormingDalMock.Setup(req => req.UpdateSheet(It.IsAny<string>(), It.IsAny<BrainSheet>()))
                .Returns(true);

            var participantDalMock = new Mock<IParticipantDalService>();
            var teamDalMock = TeamDalMock();

            var model = new BrainstormingModel()
            {
                BrainSheets = new ObservableCollection<BrainSheet>()
            };
            var context = BrainstormingModelFactory.CreateContext(1);
            context.CurrentFinding.BrainSheets = new List<BrainSheet>()
            {
                new BrainSheet()
            };

            var dalMock = new Mock<IDalService>();
            dalMock.Setup(s => s.BrainstormingDalService).Returns(brainstormingDalMock.Object);
            dalMock.Setup(s => s.ParticipantDalService).Returns(participantDalMock.Object);
            dalMock.Setup(s => s.TeamDalService).Returns(teamDalMock.Object);

            var brainstormingService = new BrainstormingService(
                dalMock.Object,
                context,
                model
               );
            return brainstormingService;
        }
    }
}