using NSubstitute.ExceptionExtensions;
using PensionsRegulatorApi.Application.Queries;
using PensionsRegulatorApi.Controllers;

namespace SFA.DAS.PensionsRegulatorApi.UnitTests.Controllers.Id.Given_A_PensionsRegulatorController.And_Error_Occurs;

[ExcludeFromCodeCoverage]
public class And_Error_Occurs
{
    protected PensionsRegulatorController SUT;
    protected IMediator MockMediatr;
    protected long TPRUniqueKey = 123456;
    private string	 _exceptionMessage = "Exceptional.";

    public And_Error_Occurs()
    {
        MockMediatr = Substitute.For<IMediator>();
        SUT = new PensionsRegulatorController(MockMediatr, Substitute.For<ILogger<PensionsRegulatorController>>());
        MockMediatr.Send(Arg.Is<GetOrganisationById>(request => request.TPRUniqueKey.Equals(TPRUniqueKey))).Throws(new TestException(_exceptionMessage));
    }

    [ExcludeFromCodeCoverage]
    public class When_Organisations_Are_Request_By_Id_Only : And_Error_Occurs
    {
        [Test]
        public async Task Then_Error_Is_Propagated()
        {
            Assert
                .ThrowsAsync(
                    Is.TypeOf<TestException>()
                        .And
                        .Message
                        .EqualTo(_exceptionMessage),
                    () =>
                        SUT.Query(TPRUniqueKey));
        }
    }
}