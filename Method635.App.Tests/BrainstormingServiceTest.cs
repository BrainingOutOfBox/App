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
    public class BrainstormingServiceTest
    {
        [SetUp]
        public void Setup()
        {
            MockPlatformServices.Init();
        }


        [Test]
        [Parallelizable(ParallelScope.Self)]
        public void StartRoundTest()
        {
            var restMock = new Mock<IBrainstormingDalService>();
            restMock.SetupSequence(request => request.GetFinding(It.IsAny<string>())).
                Returns(BrainstormingModelFactory.CreateFinding(1));

            var brainstormingService = new BrainstormingService(
                restMock.Object,
                BrainstormingModelFactory.CreateContext(0),
                new BrainstormingModel());

            brainstormingService.StartBusinessService();
            Assert.IsTrue(brainstormingService.IsWaiting);
            Thread.Sleep(2600);
            Assert.IsTrue(brainstormingService.IsRunning);
        }

        [Test]
        [Parallelizable(ParallelScope.Self)]
        public void EndRoundTest()
        {
            var restMock = new Mock<IBrainstormingDalService>();
            restMock.SetupSequence(request => request.GetFinding(It.IsAny<string>())).
                Returns(BrainstormingModelFactory.CreateFinding(-1));
            restMock.SetupSequence(req => req.UpdateSheet(It.IsAny<string>(), It.IsAny<BrainSheet>()))
                .Returns(true);

            var model = new BrainstormingModel()
            {
                BrainSheets = new ObservableCollection<BrainSheet>()
            };
            var context = BrainstormingModelFactory.CreateContext(1);
            context.CurrentFinding.BrainSheets = new List<BrainSheet>()
            {
                new BrainSheet()
            };

            var brainstormingService = new BrainstormingService(
                restMock.Object, 
                context,
                model
               );
            brainstormingService.StartBusinessService();


            Assert.IsTrue(brainstormingService.IsRunning);
            Thread.Sleep(5100);
            Assert.IsTrue(brainstormingService.IsEnded);
        }

        [Test]
        [Parallelizable(ParallelScope.Self)]
        public void RemainingTimeTest()
        {
            var restMock = new Mock<IBrainstormingDalService>();
            restMock.SetupSequence(req => req.GetRemainingTime(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(TimeSpan.FromSeconds(10))
                .Returns(TimeSpan.FromSeconds(5));

            var brainstormingService = new BrainstormingService(
                restMock.Object,
                BrainstormingModelFactory.CreateContext(1),
                new BrainstormingModel());

            brainstormingService.StartBusinessService();
            Thread.Sleep(1100);
            var remainingTime = brainstormingService.RemainingTime;
            Thread.Sleep(1100);
            Assert.IsTrue(remainingTime > brainstormingService.RemainingTime);
        }
    }
}