using Microsoft.AspNetCore.Mvc;
using PensionsRegulatorApi.Application.Queries;
using PensionsRegulatorApi.Controllers;
using PensionsRegulatorApi.Domain;

namespace SFA.DAS.PensionsRegulatorApi.UnitTests.Controllers.PayeOnly.Given_A_PensionsRegulatorController;

[ExcludeFromCodeCoverage]
public class Given_A_PensionsRegulatorController
{
    protected PensionsRegulatorController SUT;
    protected IMediator MockMediatr;
    protected IEnumerable<Organisation> ExpectedOrganisations;
    protected string PayeRef = "payes";

    public Given_A_PensionsRegulatorController()
    {
        MockMediatr
            =
            Substitute.For<IMediator>();

        SUT
            =
            new PensionsRegulatorController(
                MockMediatr,
                Substitute.For<ILogger<PensionsRegulatorController>>());

        ExpectedOrganisations =
            new Fixture()
                .CreateMany<Organisation>(
                    new Random()
                        .Next(1, 15));

        MockMediatr
            .Send(
                Arg.Is<GetOrganisationsByPayeRef>(
                    request => request.PAYEReference.Equals(
                        PayeRef,
                        StringComparison.Ordinal)))
            .Returns(
                ExpectedOrganisations);
    }

    [ExcludeFromCodeCoverage]
    public class When_Organisations_Are_Request_By_Paye_Only
        : Given_A_PensionsRegulatorController
    {
        private ActionResult<IEnumerable<Organisation>> _organisations;
        [SetUp]
        public async Task When()
        {
            _organisations
                =
                await 
                    SUT
                        .PayeRef(
                            PayeRef);
        }

        [Test]
        public void Then_Data_Is_Retrieved_Using_Paye_Only()
        {
            MockMediatr
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
                .BeEquivalentTo(ExpectedOrganisations);
        }

        [Test]
        public void Then_Data_Is_Not_Retrieved_Using_Paye_And_AORN()
        {
            MockMediatr
                .DidNotReceive()
                .Send(
                    Arg.Any<GetOrganisationsByPayeRefAndAorn>());
        }

        [Test]
        public void Then_Date_Is_Not_Retrieved_Using_Id_Only()
        {
            MockMediatr
                .DidNotReceive()
                .Send(Arg.Any<GetOrganisationById>());
        }
    }
}