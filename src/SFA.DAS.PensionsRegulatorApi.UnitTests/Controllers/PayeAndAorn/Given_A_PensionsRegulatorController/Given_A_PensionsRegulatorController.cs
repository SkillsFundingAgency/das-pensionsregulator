using Microsoft.AspNetCore.Mvc;
using PensionsRegulatorApi.Application.Queries;
using PensionsRegulatorApi.Controllers;
using PensionsRegulatorApi.Domain;

namespace SFA.DAS.PensionsRegulatorApi.UnitTests.Controllers.PayeAndAorn.Given_A_PensionsRegulatorController;

[ExcludeFromCodeCoverage]
public class GivenAPensionsRegulatorController
{
    private readonly PensionsRegulatorController _sut;
    private readonly IMediator _mockMediatr;
    private readonly IEnumerable<Organisation> _expectedOrganisations;
    private const string PayeRef = "payes";
    private const string Aorn = "aorn";

    protected GivenAPensionsRegulatorController()
    {
        _mockMediatr = Substitute.For<IMediator>();
        _sut = new PensionsRegulatorController(_mockMediatr, Substitute.For<ILogger<PensionsRegulatorController>>());

        _expectedOrganisations = new Fixture().CreateMany<Organisation>(new Random().Next(1, 15));

        _mockMediatr
            .Send(
                Arg.Is<GetOrganisationsByPayeRefAndAorn>(
                    request =>
                        request.AccountOfficeReferenceNumber.Equals(
                            Aorn,
                            StringComparison.Ordinal)
                        && request.PAYEReference.Equals(
                            PayeRef,
                            StringComparison.Ordinal)))
            .Returns(
                _expectedOrganisations);
    }

    [ExcludeFromCodeCoverage]
    public class WhenOrganisationsAreRequestByPayeAndAorn : GivenAPensionsRegulatorController
    {
        private OkObjectResult _organisations;

        [SetUp]
        public async Task When()
        {
            _organisations = await _sut.Aorn(Aorn, PayeRef) as OkObjectResult;
        }

        [Test]
        public void Then_Data_Is_Retrieved_Using_Both_Paye_And_AORN()
        {
            _mockMediatr
                .Received()
                .Send(
                    Arg.Is<GetOrganisationsByPayeRefAndAorn>(
                        arg => arg.PAYEReference.Equals(
                                   PayeRef,
                                   StringComparison.Ordinal)
                               && arg.AccountOfficeReferenceNumber.Equals(
                                   Aorn,
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
        public void Then_Date_Is_Not_Retrieved_Using_Paye_Only()
        {
            _mockMediatr.DidNotReceive().Send(Arg.Any<GetOrganisationsByPayeRef>());
        }

        [Test]
        public void Then_Date_Is_Not_Retrieved_Using_Id_Only()
        {
            _mockMediatr.DidNotReceive().Send(Arg.Any<GetOrganisationById>());
        }
    }
}