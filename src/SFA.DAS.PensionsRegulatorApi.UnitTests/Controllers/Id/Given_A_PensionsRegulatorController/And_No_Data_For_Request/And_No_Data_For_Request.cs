using Microsoft.AspNetCore.Mvc;
using PensionsRegulatorApi.Application.Queries;
using PensionsRegulatorApi.Controllers;
using PensionsRegulatorApi.Domain;

namespace SFA.DAS.PensionsRegulatorApi.UnitTests.Controllers.Id.Given_A_PensionsRegulatorController.And_No_Data_For_Request;

[ExcludeFromCodeCoverage]
public class And_No_Data_For_Request
{
    private readonly PensionsRegulatorController _sut;
    private const long TPRUniqueKey = 123456;

    protected And_No_Data_For_Request()
    {
        var mockMediatr = Substitute.For<IMediator>();
        _sut = new PensionsRegulatorController(mockMediatr, Substitute.For<ILogger<PensionsRegulatorController>>());
        mockMediatr.Send(Arg.Is<GetOrganisationById>(request => request.TPRUniqueKey.Equals(TPRUniqueKey))).Returns((Organisation)null);
    }

    [ExcludeFromCodeCoverage]
    public class When_Organisations_Are_Request_By_Id_Only : And_No_Data_For_Request
    {
        private ActionResult<Organisation> _organisation;

        [SetUp]
        public async Task When()
        {
            _organisation = await _sut.Query(TPRUniqueKey);
        }

        [Test]
        public void Then_NotFoundResult_Is_Returned()
        {
            _organisation
                .Should()
                .NotBeNull();

            _organisation.Result
                .Should()
                .BeAssignableTo<NotFoundResult>();
        }
    }
}