using NSubstitute.ExceptionExtensions;
using PensionsRegulatorApi.Application.Queries;
using PensionsRegulatorApi.Controllers;

namespace SFA.DAS.PensionsRegulatorApi.UnitTests.Controllers.Id.Given_A_PensionsRegulatorController.And_Error_Occurs;

[ExcludeFromCodeCoverage]
public class And_Error_Occurs
{
    private readonly PensionsRegulatorController _sut;
    private readonly IMediator _mockMediatr;
    private const long TPRUniqueKey = 123456;
    private readonly string	 _exceptionMessage = "Exceptional.";

    protected And_Error_Occurs()
    {
        _mockMediatr = Substitute.For<IMediator>();
        _sut = new PensionsRegulatorController(_mockMediatr, Substitute.For<ILogger<PensionsRegulatorController>>());
        _mockMediatr.Send(Arg.Is<GetOrganisationById>(request => request.TPRUniqueKey.Equals(TPRUniqueKey))).Throws(new TestException(_exceptionMessage));
    }

    [ExcludeFromCodeCoverage]
    public class When_Organisations_Are_Request_By_Id_Only : And_Error_Occurs
    {
        [Test]
        public Task Then_Error_Is_Propagated()
        {
            Assert
                .ThrowsAsync(
                    Is.TypeOf<TestException>()
                        .And
                        .Message
                        .EqualTo(_exceptionMessage),
                    () =>
                        _sut.Query(TPRUniqueKey));
            
            return Task.CompletedTask;
        }
    }
}