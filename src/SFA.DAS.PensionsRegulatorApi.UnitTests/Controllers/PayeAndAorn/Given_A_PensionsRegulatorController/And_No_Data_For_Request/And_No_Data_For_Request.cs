using Microsoft.AspNetCore.Mvc;
using PensionsRegulatorApi.Application.Queries;
using PensionsRegulatorApi.Controllers;
using PensionsRegulatorApi.Domain;

namespace SFA.DAS.PensionsRegulatorApi.UnitTests.Controllers.PayeAndAorn.Given_A_PensionsRegulatorController.And_No_Data_For_Request;

[ExcludeFromCodeCoverage]
public class And_No_Data_For_Request
{
    private readonly PensionsRegulatorController _sut;
    private const string PayeRef = "payes";
    private const string Aorn = "aorn";

    protected And_No_Data_For_Request()
    {
        var mockMediatr = Substitute.For<IMediator>();

        _sut = new PensionsRegulatorController(mockMediatr, Substitute.For<ILogger<PensionsRegulatorController>>());
  
        mockMediatr
            .Send(
                Arg.Is<GetOrganisationsByPayeRefAndAorn>(
                    request =>
                        request.AccountOfficeReferenceNumber.Equals(
                            Aorn,
                            StringComparison.Ordinal)
                        && request.PAYEReference.Equals(
                            PayeRef,
                            StringComparison.Ordinal)))
            .Returns([]);
    }

    [ExcludeFromCodeCoverage]
    public class When_Organisations_Are_Request_By_Paye_And_AORN : And_No_Data_For_Request
    {
        private IActionResult _organisations;

        [SetUp]
        public async Task When()
        {
            _organisations = await _sut.Aorn(Aorn, PayeRef);
        }

        [Test]
        public void Then_NotFoundResult_Is_Returned()
        {
            _organisations
                .Should()
                .NotBeNull();

            _organisations
                .Should()
                .BeAssignableTo<NotFoundResult>();
        }
    }
}