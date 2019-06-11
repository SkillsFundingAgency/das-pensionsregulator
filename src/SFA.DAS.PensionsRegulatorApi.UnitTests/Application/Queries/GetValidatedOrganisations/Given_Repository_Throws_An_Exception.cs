using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using MediatR;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using PensionsRegulatorApi.Application.Queries;
using PensionsRegulatorApi.Data;
using PensionsRegulatorApi.Domain;

namespace SFA.DAS.PensionsRegulatorApi.UnitTests.Application.Queries.GetValidatedOrganisations
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class Given_Repository_Throws_An_Exception
    {
        private IOrganisationRepository _mockRepository;
        private IRequestHandler<global::PensionsRegulatorApi.Application.Queries.GetValidatedOrganisations, IEnumerable<Organisation>> _sut;

        [SetUp]
        public void Given()
        {
            _mockRepository = Substitute.For<IOrganisationRepository>();

            _mockRepository
                .GetOrganisationsForPAYEReferenceAndAORN(Arg.Any<string>(), Arg.Any<string>())
                .Throws<Exception>();

            _sut = new GetValidatedOrganisationsHandler(_mockRepository);
        }

        [Test]
        public void Then_Exception_Propagates_Up()
        {
            Assert
                .ThrowsAsync<Exception>
                (
                    async () =>
                        await
                            _sut
                                .Handle(
                                    new global::PensionsRegulatorApi.Application.Queries.GetValidatedOrganisations(
                                        "anynonemptytext",
                                        "anynonemptytext"),
                                    CancellationToken.None)
                );
        }
    }
}