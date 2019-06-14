using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using MediatR;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using PensionsRegulatorApi.Application.Queries;
using PensionsRegulatorApi.Data;
using PensionsRegulatorApi.Domain;
using SFA.DAS.Testing;

namespace SFA.DAS.PensionsRegulatorApi.UnitTests.Application.Queries
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class A_GetOrganisationsByPayeRefHandler_
    : FluentTest<A_GetOrganisationsByPayeRefHandler_>
    {
        [Test]
        public async Task Returns_Data_For_PayeRef()
        {
            await
                TestAsync(
                    testFixture => testFixture.DataSourceReturnsData(),
                    testFixture => testFixture.Handle(),
                    (testFixture, organisations) => testFixture.ReturnDataIsCorrect(organisations));
        }

        [Test]
        public async Task Propagates_Errors()
        {
            await
                TestExceptionAsync(
                    arrange: testFixture => testFixture.DataRetrievalThrowsException(),
                    act: testFixture => testFixture.HandleExceptionalCase(),
                    assert: (testFixture, action) => Assert.ThrowsAsync<TestException>(() => action()));
        }

        private string _payeReference;
        private IEnumerable<Organisation> _repositoryResults;
        private IRequestHandler<GetOrganisationsByPayeRef, IEnumerable<Organisation>> _sut;

        private void DataRetrievalThrowsException()
        {
            var mockRepository = Substitute.For<IOrganisationRepository>();

            _payeReference = Guid.NewGuid().ToString().Substring(0, 25);
            mockRepository
                .GetOrganisationsForPAYEReference(_payeReference)
                .Throws<TestException>();

            _sut
                =
                new GetOrganisationsByPayeRefHandler(mockRepository);
        }

        private void ReturnDataIsCorrect(IEnumerable<Organisation> organisations)
        {
            organisations
                .Should()
                .NotBeNull();

            organisations
                .Should()
                .BeEquivalentTo(_repositoryResults);
        }

        private Task<IEnumerable<Organisation>> Handle()
        {
            return
                _sut
                    .Handle(
                        new GetOrganisationsByPayeRef(_payeReference),
                        CancellationToken.None);
        }

        private async Task HandleExceptionalCase()
        {
            await
                _sut
                    .Handle(
                        new GetOrganisationsByPayeRef(_payeReference),
                        CancellationToken.None);
        }

        private void DataSourceReturnsData()
        {
            _repositoryResults = new Fixture()
                .CreateMany<Organisation>(
                    new Random()
                        .Next(1, 15));

            var mockRepository = Substitute.For<IOrganisationRepository>();

            _payeReference = Guid.NewGuid().ToString().Substring(0, 25);
            mockRepository
                .GetOrganisationsForPAYEReference(_payeReference)
                .Returns(_repositoryResults);

            _sut
                =
                new GetOrganisationsByPayeRefHandler(mockRepository);
        }
    }

    public class TestException : Exception
    {
    }
}