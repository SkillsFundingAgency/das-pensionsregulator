using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
    public class Given_No_Results_Are_Returned
    {
        private IMediator _mockMediator;
        private IEnumerable<Organisation> _handlerResults;
        private string _expectedPayeReference;
        private PensionsRegulatorController _sut;

        public Given_No_Results_Are_Returned()
        {
            _handlerResults = new List<Organisation>();
        }

        [SetUp]
        public void Given()
        {
            _expectedPayeReference = "147qey";

            _mockMediator = Substitute.For<IMediator>();

            _mockMediator
                .Send(Arg.Is<GetValidatedOrganisations>(x => x.PAYEReference == _expectedPayeReference))
                .Returns(_handlerResults);

            _sut = new PensionsRegulatorController(_mockMediator, Substitute.For<ILogger<PensionsRegulatorController>>());
        }

        [Test]
        public async Task Then_NotFound_Is_Returned()
        {
            var organisations =
                await
                    _sut
                        .Get(_expectedPayeReference);

            organisations
                .Should()
                .NotBeNull();

            organisations.Result
                .Should()
                .BeAssignableTo<NotFoundResult>();
        }
    }
}