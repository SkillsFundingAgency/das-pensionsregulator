using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using MediatR;
using NSubstitute;
using NUnit.Framework;
using PensionsRegulatorApi.Application.Queries;
using PensionsRegulatorApi.Data;
using PensionsRegulatorApi.Domain;

namespace SFA.DAS.PensionsRegulatorApi.UnitTests.Application.Queries.GetOrganisations
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class Given_Repository_Returns_Results
    {
        private IOrganisationRepository _mockRepository;
        private IRequestHandler<global::PensionsRegulatorApi.Application.Queries.GetOrganisations, IEnumerable<Organisation>> _sut;
        private IEnumerable<Organisation> _repositoryResults;

        public Given_Repository_Returns_Results()
        {
            _repositoryResults =
                new Fixture()
                    .CreateMany<Organisation>(
                        new Random()
                            .Next(1, 15));
        }

        [SetUp]
        public void Given()
        {
            _mockRepository = Substitute.For<IOrganisationRepository>();

            _mockRepository
                .GetOrganisationsForPAYEReference(Arg.Any<string>())
                .Returns(_repositoryResults);

            _sut = new GetOrganisationsHandler(_mockRepository);
        }

        [Test]
        public async Task Then_Equivalent_Results_Are_Returned()
        {
            var organisations =
                await 
                    _sut
                        .Handle(
                            new global::PensionsRegulatorApi.Application.Queries.GetOrganisations("anynonemptytext"),
                            CancellationToken.None);

            organisations
                .Should()
                .NotBeNull();

            organisations
                .Should()
                .BeEquivalentTo(_repositoryResults);
        }
    }
}