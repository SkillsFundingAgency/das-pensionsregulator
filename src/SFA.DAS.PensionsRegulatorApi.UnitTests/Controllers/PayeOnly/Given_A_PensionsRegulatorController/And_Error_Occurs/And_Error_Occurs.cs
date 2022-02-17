using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using PensionsRegulatorApi.Application.Queries;
using PensionsRegulatorApi.Controllers;

namespace SFA.DAS.PensionsRegulatorApi.UnitTests.Controllers.PayeOnly.Given_A_PensionsRegulatorController.And_Error_Occurs
{
    [ExcludeFromCodeCoverage]
    public class And_Error_Occurs
    {
        protected PensionsRegulatorController SUT;
        protected IMediator MockMediatr;
        protected string PayeRef = "payes";
        private string	 _exceptionMessage = "Exceptional.";

        public And_Error_Occurs()
        {
            MockMediatr
                =
                Substitute.For<IMediator>();

            SUT
                =
                new PensionsRegulatorController(
                    MockMediatr,
                    Substitute.For<ILogger<PensionsRegulatorController>>());

            MockMediatr
                .Send(
                    Arg.Is<GetOrganisationsByPayeRef>(
                        request => request.PAYEReference.Equals(
                            PayeRef,
                            StringComparison.Ordinal)))
                .                Throws(
                    new TestException(_exceptionMessage));
        }

        [ExcludeFromCodeCoverage]
        public class When_Organisations_Are_Request_By_Paye_Only
            : And_Error_Occurs
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
                            SUT
                                .PayeRef(
                                    PayeRef));
            }
        }
    }
}