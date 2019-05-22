using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using PensionsRegulatorApi.Application.Queries;
using PensionsRegulatorApi.Controllers;
using PensionsRegulatorApi.Domain;

namespace SFA.DAS.PensionsRegulatorApi.UnitTests.Controllers
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class Given_Handler_Throws_Exception
    {
        private IRequestHandler<GetOrganisations, IEnumerable<Organisation>> _mockHandler;
        private string _exceptionMessage;
        private PensionsRegulatorController _sut;

        [SetUp]
        public void Given()
        {
            _exceptionMessage = "error calling db";

            _mockHandler = Substitute.For<IRequestHandler<GetOrganisations, IEnumerable<Organisation>>>();

            _mockHandler
                .Handle(Arg.Any<GetOrganisations>(), Arg.Any<CancellationToken>())
                .Throws(new Exception(_exceptionMessage));

            _sut = new PensionsRegulatorController(_mockHandler, Substitute.For<ILogger<PensionsRegulatorController>>());
        }

        [Test]
        public async Task Then_Exception_Is_Thrown()
        {
            Exception actualException = null;
            try
            {
                await _sut.Get("test");
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