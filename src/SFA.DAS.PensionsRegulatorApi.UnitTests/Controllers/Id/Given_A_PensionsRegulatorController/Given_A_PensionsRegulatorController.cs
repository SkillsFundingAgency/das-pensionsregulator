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

namespace SFA.DAS.PensionsRegulatorApi.UnitTests.Controllers.Id.Given_A_PensionsRegulatorController
{
    [ExcludeFromCodeCoverage]
    public class Given_A_PensionsRegulatorController
    {
        protected PensionsRegulatorController SUT;
        protected IMediator MockMediatr;
        protected Organisation ExpectedOrganisation;
        protected long TPRUniqueKey = 123456;

        public Given_A_PensionsRegulatorController()
        {
            MockMediatr = Substitute.For<IMediator>();

            SUT = new PensionsRegulatorController(MockMediatr, Substitute.For<ILogger<PensionsRegulatorController>>());
            ExpectedOrganisation = new Fixture().Create<Organisation>();
            MockMediatr.Send(Arg.Is<GetOrganisationById>(request => request.TPRUniqueKey.Equals(TPRUniqueKey))).Returns(ExpectedOrganisation);
        }

        [ExcludeFromCodeCoverage]
        public class When_Organisation_Are_Request_By_Id_Only : Given_A_PensionsRegulatorController
        {
            private ActionResult<Organisation> _organisation;

            [SetUp]
            public async Task When()
            {
                _organisation = await SUT.Query(TPRUniqueKey);
            }

            [Test]
            public void Then_Data_Is_Retrieved_Using_Paye_Only()
            {
                MockMediatr.Received().Send(Arg.Is<GetOrganisationById>(arg => arg.TPRUniqueKey.Equals(TPRUniqueKey)));

                _organisation
                    .Should()
                    .NotBeNull();

                _organisation
                    .Value
                    .Should()
                    .BeEquivalentTo(ExpectedOrganisation);
            }

            [Test]
            public void Then_Data_Is_Not_Retrieved_Using_Paye_And_AORN()
            {
                MockMediatr.DidNotReceive().Send(Arg.Any<GetOrganisationsByPayeRefAndAorn>());
            }

            [Test]
            public void Then_Data_Is_Not_Retrieved_Using_Paye_Only()
            {
                MockMediatr.DidNotReceive().Send(Arg.Any<GetOrganisationsByPayeRef>());
            }
        }
    }
}