using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using FluentAssertions;
using MediatR;
using NSubstitute.ExceptionExtensions;
using PensionsRegulatorApi.Application.Queries;
using PensionsRegulatorApi.Data;
using PensionsRegulatorApi.Domain;

namespace SFA.DAS.PensionsRegulatorApi.UnitTests.Application.Queries.GetOrganisations
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class Given_Repository_Throws_An_Exception
    {
        private IOrganisationRepository _mockRepository;
        private IRequestHandler<global::PensionsRegulatorApi.Application.Queries.GetOrganisations, IEnumerable<Organisation>> _sut;

        [SetUp]
        public void Given()
        {
            _mockRepository = Substitute.For<IOrganisationRepository>();

            _mockRepository
                .GetOrganisationsForPAYEReference(String.Empty)
                .ThrowsForAnyArgs<Exception>();

            _sut = new GetOrganisationsHandler(_mockRepository);
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
                                    new global::PensionsRegulatorApi.Application.Queries.GetOrganisations(
                                        "anynonemptytext"),
                                    CancellationToken.None)
                );
        }
    }
}