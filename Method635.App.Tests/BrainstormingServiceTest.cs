using Method635.App.BL;
using Method635.App.Dal.Interfaces;
using Method635.App.Forms.Context;
using Method635.App.Models;
using Method635.App.Tests.Setup;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace Tests
{
    public class BrainstormingServiceTest
    {
        [SetUp]
        public void Setup()
        {
            MockPlatformServices.Init();
        }

        private BrainstormingContext CreateContext(int findingRound)
        {
            return new BrainstormingContext()
            {
                CurrentFinding = CreateFinding(findingRound)
            };
        }
        private BrainstormingFinding CreateFinding(int round)
        {
            return new BrainstormingFinding()
            {
                CurrentRound = round
            };
        }

        [Test]
        public void StartRoundTest()
        {
            var restMock = new Mock<IBrainstormingDalService>();
            restMock.SetupSequence(request => request.GetFinding(It.IsAny<string>())).
                Returns(CreateFinding(1));

            var brainstormingService = new BrainstormingService(restMock.Object, CreateContext(0));

            brainstormingService.StartBusinessService();
            Assert.IsTrue(brainstormingService.IsWaiting);
            Thread.Sleep(2600);
            Assert.IsTrue(brainstormingService.IsRunning);
        }

        [Test]
        public void EndRoundTest()
        {
            var restMock = new Mock<IBrainstormingDalService>();
            restMock.SetupSequence(request => request.GetFinding(It.IsAny<string>())).
                Returns(CreateFinding(-1));
            var brainstormingService = new BrainstormingService(restMock.Object, CreateContext(1));


            Assert.IsTrue(brainstormingService.IsRunning);
            Thread.Sleep(2600);
            Assert.IsTrue(brainstormingService.IsEnded);
        }
    }
}