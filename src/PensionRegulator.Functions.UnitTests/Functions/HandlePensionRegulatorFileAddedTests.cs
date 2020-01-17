using System;
using Moq;
using NUnit.Framework;
using Microsoft.Extensions.Logging;
using PensionsRegulator.Functions;
using PensionsRegulator.Functions.Domain.Services;

namespace PensionRegulator.Functions.UnitTests.Functions
{
    public class WhenPensionRegulatorFileAdded
    {
        private Mock<ILogger> _logger;
        private Mock<IPensionRegulatorImportService> _pensionRegulatorService;

        private HandlePensionRegulatorFileAdded _sut;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger>();
            _pensionRegulatorService = new Mock<IPensionRegulatorImportService>(MockBehavior.Strict);

            _pensionRegulatorService.Setup(s => s.RegisterNewTrpFile("success"));
            _pensionRegulatorService.Setup(s => s.RegisterNewTrpFile("failure")).Throws<Exception>();

            _sut = new HandlePensionRegulatorFileAdded(_pensionRegulatorService.Object);
        }

        [Test]
        public void Then_Blob_Is_Handled_Successfully()
        {
            var filename = "success";

            System.IO.Stream stream = new System.IO.MemoryStream();
            _sut.Run(stream, filename, _logger.Object);

            _logger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) =>
                        string.Equals("HandlePensionRegulatorFileAdded blob trigger function processed a request.",
                            o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>) It.IsAny<object>()),
                Times.Once);

            _logger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals($"Begin processing of blob {filename}", o.ToString(),
                        StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>) It.IsAny<object>()),
                Times.Once);

            _pensionRegulatorService.Verify(v => v.RegisterNewTrpFile("success"), Times.Once);

            _logger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals($"Processing of blob '{filename}' completed successfully.",
                        o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>) It.IsAny<object>()),
                Times.Once);
        }

        [Test]
        public void And_Exception_Raised_Then_Blob_Is_Handled_Successfully()
        {
            System.IO.Stream stream = new System.IO.MemoryStream();

            Assert.Throws<Exception>(() => _sut.Run(stream, "failure", _logger.Object));

            _logger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) =>
                        string.Equals("HandlePensionRegulatorFileAdded blob trigger function processed a request.",
                            o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);

            _logger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals("Begin processing of blob failure", o.ToString(),
                        StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);

            _logger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals("Processing of blob 'failure' Failed",
                        o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }
    }
}
