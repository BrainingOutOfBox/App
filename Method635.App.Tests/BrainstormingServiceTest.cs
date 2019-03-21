using Method635.App.BL;
using Method635.App.Forms.Context;
using Method635.App.Forms.RestAccess;
using Method635.App.Models;
using Method635.App.Tests.Setup;
using Moq;
using NUnit.Framework;
using System;
using System.Threading;

namespace Tests
{
    public class Tests
    {
        private BrainstormingService _brainstormingService;

        [SetUp]
        public void Setup()
        {
            MockPlatformServices.Init();

           var _context = new BrainstormingContext()
            {
                CurrentFinding = CreateFinding(0)
            };
            var restMock = new Mock<BrainstormingFindingRestResolver>();
            restMock.SetupSequence(request => request.GetFinding(It.IsAny<BrainstormingFinding>())).
                Returns(CreateFinding(1));

            _brainstormingService = new BrainstormingService(restMock.Object, _context);
        }

        private BrainstormingFinding CreateFinding(int round)
        {
            return new BrainstormingFinding()
            {
                CurrentRound = round
            };
        }

        [Test]
        public void Test1()
        {
            _brainstormingService.StartBusinessService();
            Assert.IsTrue(_brainstormingService.IsWaiting);
            Thread.Sleep(2600);
            Assert.IsTrue(_brainstormingService.IsRunning);
        }
    }
}