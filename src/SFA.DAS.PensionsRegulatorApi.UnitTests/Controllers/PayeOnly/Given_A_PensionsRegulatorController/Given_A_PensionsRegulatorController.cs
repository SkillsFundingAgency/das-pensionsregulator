using Microsoft.AspNetCore.Mvc;
using PensionsRegulatorApi.Application.Queries;
using PensionsRegulatorApi.Controllers;
using PensionsRegulatorApi.Domain;

namespace SFA.DAS.PensionsRegulatorApi.UnitTests.Controllers.PayeOnly.Given_A_PensionsRegulatorController;

[ExcludeFromCodeCoverage]
public class GivenAPensionsRegulatorController
{
    private readonly PensionsRegulatorController _sut;
    private readonly IMediator _mockMediatr;
    private readonly IEnumerable<Organisation> _expectedOrganisations;
    private const string PayeRef = "payes";

    protected GivenAPensionsRegulatorController()
    {
        _mockMediatr = Substitute.For<IMediator>();

        _sut = new PensionsRegulatorController(_mockMediatr, Substitute.For<ILogger<PensionsRegulatorController>>());

        _expectedOrganisations =
            new Fixture()
                .CreateMany<Organisation>(
                    new Random()
                        .Next(1, 15));

        _mockMediatr
            .Send(
                Arg.Is<GetOrganisationsByPayeRef>(
                    request => request.PAYEReference.Equals(
                        PayeRef,
                        StringComparison.Ordinal)))
            .Returns(
                _expectedOrganisations);
    }

    [ExcludeFromCodeCoverage]
    public class WhenOrganisationsAreRequestByPayeOnly : GivenAPensionsRegulatorController
    {
        private OkObjectResult _organisations;

        [SetUp]
        public async Task When()
        {
            _organisations = await _sut.PayeRef(PayeRef) as OkObjectResult;
        }

        [Test]
        public void Then_Data_Is_Retrieved_Using_Paye_Only()
        {
            _mockMediatr
                .Received()
                .Send(
                    Arg.Is<GetOrganisationsByPayeRef>(
                        arg => arg.PAYEReference.Equals(
                            PayeRef,
                            StringComparison.Ordinal)));

            _organisations
                .Should()
                .NotBeNull();

            _organisations
                .Value
                .Should()
                .BeEquivalentTo(_expectedOrganisations);
        }

        [Test]
        public void Then_Data_Is_Not_Retrieved_Using_Paye_And_AORN()
        {
            _mockMediatr
                .DidNotReceive()
                .Send(
                    Arg.Any<GetOrganisationsByPayeRefAndAorn>());
        }

        [Test]
        public void Then_Date_Is_Not_Retrieved_Using_Id_Only()
        {
            _mockMediatr
                .DidNotReceive()
                .Send(Arg.Any<GetOrganisationById>());
        }
    }
}