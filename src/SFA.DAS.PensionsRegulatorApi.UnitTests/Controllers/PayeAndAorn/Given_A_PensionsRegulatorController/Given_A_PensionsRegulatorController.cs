using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Bson;
using NSubstitute;
using NUnit.Framework;
using PensionsRegulatorApi.Application.Queries;
using PensionsRegulatorApi.Controllers;

namespace SFA.DAS.PensionsRegulatorApi.UnitTests.Controllers.PayeAndAorn.Given_A_PensionsRegulatorController
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
        public class When_Organisations_Are_Request_By_Paye_And_AORN
            : Given_A_PensionsRegulatorController
        {
            private string _payeRef = "payes";
            private string _aorn = "aorn";

            [SetUp]
            public async Task When()
            {
                await 
                SUT
                    .Get(
                        _payeRef,
                        _aorn);
            }

            [Test]
            public void Then_Data_Is_Retrieved_Using_Both_Paye_And_AORN()
            {
                MockMediatr
                    .Received()
                    .Send(
                        Arg.Is<GetValidatedOrganisations>(
                            arg => arg.PAYEReference.Equals(
                                       _payeRef,
                                       StringComparison.Ordinal)
                                   && arg.AccountOfficeReferenceNumber.Equals(
                                       _aorn,
                                       StringComparison.Ordinal)));
            }

            [Test]
            public void Then_Date_Is_Not_Retrieved_Using_Paye_Only()
            {
                MockMediatr.DidNotReceive()
                    .Send(Arg.Any<GetOrganisations>());
            }
        }
    }
}