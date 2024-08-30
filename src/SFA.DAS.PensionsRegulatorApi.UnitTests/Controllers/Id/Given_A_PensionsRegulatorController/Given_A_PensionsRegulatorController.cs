using Microsoft.AspNetCore.Mvc;
using PensionsRegulatorApi.Application.Queries;
using PensionsRegulatorApi.Controllers;
using PensionsRegulatorApi.Domain;

namespace SFA.DAS.PensionsRegulatorApi.UnitTests.Controllers.Id.Given_A_PensionsRegulatorController;

[ExcludeFromCodeCoverage]
public class Given_A_PensionsRegulatorController
{
    private readonly PensionsRegulatorController _sut;
    private readonly IMediator _mockMediatr;
    private readonly Organisation _expectedOrganisation;
    private const long TPRUniqueKey = 123456;

    protected Given_A_PensionsRegulatorController()
    {
        _mockMediatr = Substitute.For<IMediator>();

        _sut = new PensionsRegulatorController(_mockMediatr, Substitute.For<ILogger<PensionsRegulatorController>>());
        _expectedOrganisation = new Fixture().Create<Organisation>();
        _mockMediatr.Send(Arg.Is<GetOrganisationById>(request => request.TPRUniqueKey.Equals(TPRUniqueKey))).Returns(_expectedOrganisation);
    }

    [ExcludeFromCodeCoverage]
    public class When_Organisation_Are_Request_By_Id_Only : Given_A_PensionsRegulatorController
    {
        private ActionResult<Organisation> _organisation;

        [SetUp]
        public async Task When()
        {
            _organisation = await _sut.Query(TPRUniqueKey);
        }

        [Test]
        public void Then_Data_Is_Retrieved_Using_Paye_Only()
        {
            _mockMediatr.Received().Send(Arg.Is<GetOrganisationById>(arg => arg.TPRUniqueKey.Equals(TPRUniqueKey)));

            _organisation
                .Should()
                .NotBeNull();

            _organisation
                .Value
                .Should()
                .BeEquivalentTo(_expectedOrganisation);
        }

        [Test]
        public void Then_Data_Is_Not_Retrieved_Using_Paye_And_AORN()
        {
            _mockMediatr.DidNotReceive().Send(Arg.Any<GetOrganisationsByPayeRefAndAorn>());
        }

        [Test]
        public void Then_Data_Is_Not_Retrieved_Using_Paye_Only()
        {
            _mockMediatr.DidNotReceive().Send(Arg.Any<GetOrganisationsByPayeRef>());
        }
    }
}