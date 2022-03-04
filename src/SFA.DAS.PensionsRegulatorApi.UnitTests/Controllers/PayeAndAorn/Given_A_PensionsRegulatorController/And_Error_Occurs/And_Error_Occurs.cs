using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using PensionsRegulatorApi.Application.Queries;
using PensionsRegulatorApi.Controllers;
using PensionsRegulatorApi.Domain;

namespace SFA.DAS.PensionsRegulatorApi.UnitTests.Controllers.PayeAndAorn.Given_A_PensionsRegulatorController.And_Error_Occurs
{
    [ExcludeFromCodeCoverage]
    public class And_Error_Occurs
    {
        protected PensionsRegulatorController SUT;
        protected IMediator MockMediatr;
        protected string PayeRef = "payes";
        protected string Aorn = "aorn";
        private string _exceptionMessage = "Exceptional.";

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
                    Arg.Is<GetOrganisationsByPayeRefAndAorn>(
                        request =>
                            request.AccountOfficeReferenceNumber.Equals(
                                Aorn,
                                StringComparison.Ordinal)
                            && request.PAYEReference.Equals(
                                PayeRef,
                                StringComparison.Ordinal)))
                .Throws(
                    new TestException(_exceptionMessage));
        }

        [ExcludeFromCodeCoverage]
        public class When_Organisations_Are_Request_By_Paye_And_AORN
            : And_Error_Occurs
        {
            private ActionResult<IEnumerable<Organisation>> _organisations;

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
                                .Aorn(
                                    Aorn,
                                    PayeRef));
            }
        }
    }
}