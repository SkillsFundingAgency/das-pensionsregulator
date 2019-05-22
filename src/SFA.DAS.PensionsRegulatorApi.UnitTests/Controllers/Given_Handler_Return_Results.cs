using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using FluentAssertions.Common;
using MediatR;
using NSubstitute;
using NUnit.Framework;
using PensionsRegulatorApi.Application.Queries;
using PensionsRegulatorApi.Controllers;
using PensionsRegulatorApi.Data;
using PensionsRegulatorApi.Domain;

namespace SFA.DAS.PensionsRegulatorApi.UnitTests.Controllers
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class Given_Handler_Returns_Results
    {
        private IRequestHandler<GetOrganisations, IEnumerable<Organisation>> _mockHandler;
        private IEnumerable<Organisation> _handlerResults;
        private string _expectedPayeReference;
        private PensionsRegulatorController _sut;

        public Given_Handler_Returns_Results()
        {
            _handlerResults =
                new Fixture()
                    .CreateMany<Organisation>(
                        new Random()
                            .Next(1, 15));
        }

        [SetUp]
        public void Given()
        {
            _expectedPayeReference = "147qey";

            _mockHandler = Substitute.For<IRequestHandler<GetOrganisations, IEnumerable<Organisation>>>();

            _mockHandler
                .Handle(Arg.Is<GetOrganisations>(x => x.PAYEReference == _expectedPayeReference), Arg.Any<CancellationToken>())
                .Returns(_handlerResults);

            _sut = new PensionsRegulatorController(_mockHandler);
        }

        [Test]
        public async Task Then_Equivalent_Results_Are_Returned()
        {
            var organisations =
                await
                    _sut
                        .Get(_expectedPayeReference);

            organisations
                .Should()
                .NotBeNull();

            organisations
                .Should()
                .BeEquivalentTo(_handlerResults);
        }
    }
}
