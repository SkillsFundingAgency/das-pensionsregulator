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
    public class A_GetOrganisationByIdHandler_ : FluentTest<A_GetOrganisationByIdHandler_>
    {
        [Test]
        public async Task Returns_Data_For_Id()
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

        private long _tpruniquekey;
        private Organisation _repositoryResult;
        private IRequestHandler<GetOrganisationById, Organisation> _sut;

        private void DataRetrievalThrowsException()
        {
            var mockRepository = Substitute.For<IOrganisationRepository>();

            _tpruniquekey = 123456789;
            
            mockRepository
                .GetOrganisationById(_tpruniquekey)
                .Throws<TestException>();

            _sut = new GetOrganisationByIdHandler(mockRepository);
        }

        private void ReturnDataIsCorrect(Organisation organisation)
        {
            organisation
                .Should()
                .NotBeNull();

            organisation
                .Should()
                .BeEquivalentTo(_repositoryResult);
        }

        private Task<Organisation> Handle()
        {
            return _sut.Handle(new GetOrganisationById(_tpruniquekey), CancellationToken.None);
        }

        private async Task HandleExceptionalCase()
        {
            await _sut.Handle(new GetOrganisationById(_tpruniquekey), CancellationToken.None);
        }

        private void DataSourceReturnsData()
        {
            _repositoryResult = new Fixture().Create<Organisation>();

            var mockRepository = Substitute.For<IOrganisationRepository>();

            _tpruniquekey = 987654321;

            mockRepository.GetOrganisationById(_tpruniquekey).Returns(_repositoryResult);

            _sut = new GetOrganisationByIdHandler(mockRepository);
        }
    }
}