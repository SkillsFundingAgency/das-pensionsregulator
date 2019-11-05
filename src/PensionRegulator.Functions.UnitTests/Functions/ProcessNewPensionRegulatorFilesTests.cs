using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Moq;
using NUnit.Framework;
using Microsoft.Extensions.Logging;
using PensionsRegulator.Functions;
using PensionsRegulator.Functions.Domain.Services;

namespace PensionRegulator.Functions.UnitTests.Functions
{
    public class WhenProcessPensionRegulatorFileTriggered
    {
        private Mock<ILogger> _logger;
        private Mock<IPensionRegulatorImportService> _pensionRegulatorService;

        private ProcessNewPensionRegulatorFiles _sut;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger>();
            _pensionRegulatorService = new Mock<IPensionRegulatorImportService>(MockBehavior.Strict);

            _pensionRegulatorService.Setup(s => s.ProcessFiles());

            _sut = new ProcessNewPensionRegulatorFiles(_pensionRegulatorService.Object, _logger.Object);
        }

        [Test]
        public void Then_Blob_Is_Handled_Successfully()
        {
            System.IO.Stream stream = new System.IO.MemoryStream();
            _sut.RunTimer(new TimerInfo(new WeeklySchedule(),new ScheduleStatus(),false));

            _pensionRegulatorService.Verify(v => v.ProcessFiles(), Times.Once);
        }
    }
}
