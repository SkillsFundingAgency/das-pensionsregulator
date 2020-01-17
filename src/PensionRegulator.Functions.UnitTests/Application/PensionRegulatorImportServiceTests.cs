using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PensionsRegulator.Functions.Application.Services;
using PensionsRegulator.Functions.Domain.Data;

namespace PensionRegulator.Functions.UnitTests.Application
{
    public class PensionRegulatorImportServiceTests
    {
        private Mock<ILogger> _logger;
        private Mock<IPensionRegulatorRepository> _pensionRegulatorRepository;

        private PensionRegulatorImportService _sut;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger>();
            _pensionRegulatorRepository = new Mock<IPensionRegulatorRepository>();

            _sut = new PensionRegulatorImportService(_logger.Object, _pensionRegulatorRepository.Object);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void When_Adding_File_And_NullOrWhiteSpace_Then_Throw_ArgumentException(string filename)
        {
            Assert.Throws<ArgumentException>(() => _sut.RegisterNewTrpFile(filename));
        }

        [Test]
        public void When_Adding_Valid_File_Then_Inserts_Filename()
        {
            var filename = "filename";

            _sut.RegisterNewTrpFile(filename);

            _pensionRegulatorRepository.Verify(v => v.InsertPensionRegulatorFilename(filename));
        }

        [Test]
        public void When_Processing_LoadedFiles_Then_LoadPensionRegulatorFile()
        {
            _sut.ProcessFiles();

            _pensionRegulatorRepository.Verify(v => v.LoadPensionRegulatorFile());
        }
    }
}
