using NSubstitute.ExceptionExtensions;
using PensionsRegulatorApi.Application.Queries;
using PensionsRegulatorApi.Controllers;

namespace SFA.DAS.PensionsRegulatorApi.UnitTests.Controllers.PayeOnly.Given_A_PensionsRegulatorController.
    And_Error_Occurs;

[ExcludeFromCodeCoverage]
public class AndErrorOccurs
{
    private readonly PensionsRegulatorController _sut;
    private const string PayeRef = "payes";
    private const string ExceptionMessage = "Exceptional.";

    protected AndErrorOccurs()
    {
        var mockMediatr = Substitute.For<IMediator>();

        _sut = new PensionsRegulatorController(mockMediatr, Substitute.For<ILogger<PensionsRegulatorController>>());

        mockMediatr
            .Send(
                Arg.Is<GetOrganisationsByPayeRef>(
                    request => request.PAYEReference.Equals(
                        PayeRef,
                        StringComparison.Ordinal)))
            .Throws(
                new TestException(ExceptionMessage));
    }

    [ExcludeFromCodeCoverage]
    public class WhenOrganisationsAreRequestByPayeOnly : AndErrorOccurs
    {
        [Test]
        public Task Then_Error_Is_Propagated()
        {
            Assert
                .ThrowsAsync(
                    Is.TypeOf<TestException>()
                        .And
                        .Message
                        .EqualTo(ExceptionMessage),
                    () => _sut.PayeRef(PayeRef));
            return Task.CompletedTask;
        }
    }
}