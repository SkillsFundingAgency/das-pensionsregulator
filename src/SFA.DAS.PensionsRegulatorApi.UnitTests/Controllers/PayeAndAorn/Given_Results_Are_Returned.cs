using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using PensionsRegulatorApi.Application.Queries;
using PensionsRegulatorApi.Controllers;
using PensionsRegulatorApi.Domain;

namespace SFA.DAS.PensionsRegulatorApi.UnitTests.Controllers.PayeAndAorn
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class Given_Results_Are_Returned
    {
        private IMediator _mockMediator;
        private IEnumerable<Organisation> _handlerResults;
        private string _expectedPayeReference;
        private string _expectedAORN;
        private PensionsRegulatorController _sut;

        public Given_Results_Are_Returned()
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
            _expectedAORN = "AORN-c5739f93b3c54061b81b";

            _mockMediator = Substitute.For<IMediator>();

            _mockMediator
                .Send(Arg.Is<GetValidatedOrganisations>(
                    x => x.PAYEReference == _expectedPayeReference
                         && x.AccountOfficeReferenceNumber == _expectedAORN))
                .Returns(_handlerResults);

            _sut = new PensionsRegulatorController(
                _mockMediator,
                Substitute.For<ILogger<PensionsRegulatorController>>());
        }

        [Test]
        public async Task Then_Equivalent_Results_Are_Returned()
        {
            var organisations =
                await
                    _sut
                        .Get(
                            _expectedAORN,
                            _expectedPayeReference);

            organisations
                .Should()
                .NotBeNull();

            organisations
                .Value
                .Should()
                .BeEquivalentTo(_handlerResults);
        }
    }
}
