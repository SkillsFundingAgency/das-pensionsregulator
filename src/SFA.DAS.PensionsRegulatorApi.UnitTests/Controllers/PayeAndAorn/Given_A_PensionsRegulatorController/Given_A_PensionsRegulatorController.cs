using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using PensionsRegulatorApi.Application.Queries;
using PensionsRegulatorApi.Controllers;
using PensionsRegulatorApi.Domain;

namespace SFA.DAS.PensionsRegulatorApi.UnitTests.Controllers.PayeAndAorn.Given_A_PensionsRegulatorController
{
    [ExcludeFromCodeCoverage]
    public class Given_A_PensionsRegulatorController
    {
        protected PensionsRegulatorController SUT;
        protected IMediator MockMediatr;
        protected IEnumerable<Organisation> ExpectedOrganisations;
        protected string PayeRef = "payes";
        protected string Aorn = "aorn";

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

            ExpectedOrganisations =
                new Fixture()
                    .CreateMany<Organisation>(
                        new Random()
                            .Next(1, 15));

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
                .Returns(
                    ExpectedOrganisations);
        }

        [ExcludeFromCodeCoverage]
        public class When_Organisations_Are_Request_By_Paye_And_AORN
            : Given_A_PensionsRegulatorController
        {
            private ActionResult<IEnumerable<Organisation>> _organisations;

            [SetUp]
            public async Task When()
            {
                _organisations
                    =
                    await
                        SUT
                            .Aorn(
                                Aorn,
                                PayeRef);
            }

            [Test]
            public void Then_Data_Is_Retrieved_Using_Both_Paye_And_AORN()
            {
                MockMediatr
                    .Received()
                    .Send(
                        Arg.Is<GetOrganisationsByPayeRefAndAorn>(
                            arg => arg.PAYEReference.Equals(
                                       PayeRef,
                                       StringComparison.Ordinal)
                                   && arg.AccountOfficeReferenceNumber.Equals(
                                       Aorn,
                                       StringComparison.Ordinal)));

                _organisations
                    .Should()
                    .NotBeNull();

                _organisations
                    .Value
                    .Should()
                    .BeEquivalentTo(ExpectedOrganisations);
            }

            [Test]
            public void Then_Date_Is_Not_Retrieved_Using_Paye_Only()
            {
                MockMediatr.DidNotReceive()
                    .Send(Arg.Any<GetOrganisationsByPayeRef>());
            }

            [Test]
            public void Then_Date_Is_Not_Retrieved_Using_Id_Only()
            {
                MockMediatr.DidNotReceive()
                    .Send(Arg.Any<GetOrganisationById>());
            }
        }
    }
}