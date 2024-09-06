using Microsoft.AspNetCore.Mvc;
using NSubstitute.ExceptionExtensions;
using PensionsRegulatorApi.Application.Queries;
using PensionsRegulatorApi.Controllers;
using PensionsRegulatorApi.Domain;

namespace SFA.DAS.PensionsRegulatorApi.UnitTests.Controllers.PayeAndAorn.Given_A_PensionsRegulatorController.
    And_Error_Occurs;

[ExcludeFromCodeCoverage]
public class And_Error_Occurs
{
    private readonly PensionsRegulatorController _sut;
    private const string PayeRef = "payes";
    private const string Aorn = "aorn";
    private const string ExceptionMessage = "Exceptional.";

    protected And_Error_Occurs()
    {
        var mockMediatr = Substitute.For<IMediator>();

        _sut = new PensionsRegulatorController(
            mockMediatr,
            Substitute.For<ILogger<PensionsRegulatorController>>());

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
            .Throws(
                new TestException(ExceptionMessage));
    }

    [ExcludeFromCodeCoverage]
    public class When_Organisations_Are_Request_By_Paye_And_AORN : And_Error_Occurs
    {
        [Test]
        public Task Then_Error_Is_Propagated()
        {
            Assert.ThrowsAsync(
                Is.TypeOf<TestException>()
                    .And
                    .Message
                    .EqualTo(ExceptionMessage),
                () => _sut.Aorn(Aorn, PayeRef));

            return Task.CompletedTask;
        }
    }
}