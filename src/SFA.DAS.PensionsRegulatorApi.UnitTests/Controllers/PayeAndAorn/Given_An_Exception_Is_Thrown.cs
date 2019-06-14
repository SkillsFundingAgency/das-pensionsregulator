using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using PensionsRegulatorApi.Application.Queries;
using PensionsRegulatorApi.Controllers;

namespace SFA.DAS.PensionsRegulatorApi.UnitTests.Controllers.PayeAndAorn
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class Given_An_Exception_Is_Thrown
    {
        private IMediator _mockMediator;
        private string _exceptionMessage;
        private PensionsRegulatorController _sut;

        [SetUp]
        public void Given()
        {
            _exceptionMessage = "error calling db";

            _mockMediator = Substitute.For<IMediator>();

            _mockMediator
                .Send(Arg.Any<GetOrganisationsByPayeRefAndAorn>())
                .Throws(new Exception(_exceptionMessage));

            _sut = new PensionsRegulatorController(_mockMediator, Substitute.For<ILogger<PensionsRegulatorController>>());
        }

        [Test]
        public async Task Then_Exception_Is_Thrown()
        {
            Exception actualException = null;
            try
            {
                await _sut.Get("paye", "aorn");
            }
            catch (Exception e)
            {
                actualException = e;
            }

            Assert.That(actualException, Is.Not.Null);
            Assert.That(actualException.Message, Is.EqualTo(_exceptionMessage));
        }
    }
}