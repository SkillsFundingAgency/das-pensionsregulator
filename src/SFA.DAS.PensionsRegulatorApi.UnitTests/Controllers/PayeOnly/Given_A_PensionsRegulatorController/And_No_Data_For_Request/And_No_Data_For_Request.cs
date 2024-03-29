﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using PensionsRegulatorApi.Application.Queries;
using PensionsRegulatorApi.Controllers;
using PensionsRegulatorApi.Domain;

namespace SFA.DAS.PensionsRegulatorApi.UnitTests.Controllers.PayeOnly.Given_A_PensionsRegulatorController.And_No_Data_For_Request
{
    [ExcludeFromCodeCoverage]
    public class And_No_Data_For_Request
    {
        protected PensionsRegulatorController SUT;
        protected IMediator MockMediatr;
        protected string PayeRef = "payes";

        public And_No_Data_For_Request()
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
                .Returns(
                    Enumerable.Empty<Organisation>());
        }

        [ExcludeFromCodeCoverage]
        public class When_Organisations_Are_Request_By_Paye_Only
            : And_No_Data_For_Request
        {
            private ActionResult<IEnumerable<Organisation>> _organisations;

            [SetUp]
            public async Task When()
            {
                _organisations
                    =
                    await
                        SUT
                            .PayeRef(
                                PayeRef);
            }

            [Test]
            public void Then_NotFoundResult_Is_Returned()
            {
                _organisations
                    .Should()
                    .NotBeNull();

                _organisations.Result
                    .Should()
                    .BeAssignableTo<NotFoundResult>();
            }
    }
    }
}