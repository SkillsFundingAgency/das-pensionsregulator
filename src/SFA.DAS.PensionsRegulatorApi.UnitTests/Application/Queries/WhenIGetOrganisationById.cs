using NSubstitute.ExceptionExtensions;
using PensionsRegulatorApi.Application.Queries;
using PensionsRegulatorApi.Data;
using PensionsRegulatorApi.Domain;

namespace SFA.DAS.PensionsRegulatorApi.UnitTests.Application.Queries;

[TestFixture]
[ExcludeFromCodeCoverage]
public class WhenIGetOrganisationById 
{
    [Test]
    public async Task Returns_Data_For_Id()
    {
        var testFixture = new WhenIGetOrganisationById();
        testFixture.DataSourceReturnsData();
        var organisations = await testFixture.Handle();
        testFixture.ReturnDataIsCorrect(organisations);
    }

    [Test]
    public void Propagates_Errors()
    {
        var testFixture = new WhenIGetOrganisationById();
        testFixture.DataRetrievalThrowsException();
        var action = testFixture.HandleExceptionalCase();
        action.Should().Throws<TestException>();
    }

    private long _tpruniquekey;
    private Organisation _repositoryResult;
    private GetOrganisationByIdHandler _sut;

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