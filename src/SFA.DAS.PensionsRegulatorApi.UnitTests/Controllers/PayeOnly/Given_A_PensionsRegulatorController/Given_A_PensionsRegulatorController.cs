using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using PensionsRegulatorApi.Application.Queries;
using PensionsRegulatorApi.Controllers;

namespace SFA.DAS.PensionsRegulatorApi.UnitTests.Controllers.PayeOnly.Given_A_PensionsRegulatorController
{
    [ExcludeFromCodeCoverage]
    public class Given_A_PensionsRegulatorController
    {
        protected PensionsRegulatorController SUT;
        protected IMediator MockMediatr;

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
        }

        [ExcludeFromCodeCoverage]
        public class When_Organisations_Are_Request_By_Paye_Only
            : Given_A_PensionsRegulatorController
        {
            private string _payeRef = "payes";

            [SetUp]
            public async Task When()
            {
                await 
                SUT
                    .Get(
                        _payeRef);
            }

            [Test]
            public void Then_Data_Is_Retrieved_Using_Paye_Only()
            {
                MockMediatr
                    .Received()
                    .Send(
                        Arg.Is<GetOrganisations>(
                            arg => arg.PAYEReference.Equals(
                                       _payeRef,
                                       StringComparison.Ordinal)));
            }

            [Test]
            public void Then_Data_Is_Not_Retrieved_Using_Paye_And_AORN()
            {
                MockMediatr
                    .DidNotReceive()
                    .Send(
                        Arg.Any<GetValidatedOrganisations>());
            }
        }
    }
}