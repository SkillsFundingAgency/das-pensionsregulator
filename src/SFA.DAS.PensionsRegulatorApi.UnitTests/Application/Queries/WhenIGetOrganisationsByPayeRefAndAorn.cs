using NSubstitute.ExceptionExtensions;
using PensionsRegulatorApi.Application.Queries;
using PensionsRegulatorApi.Data;
using PensionsRegulatorApi.Domain;

namespace SFA.DAS.PensionsRegulatorApi.UnitTests.Application.Queries;

[TestFixture]
[ExcludeFromCodeCoverage]
public class WhenIGetOrganisationsByPayeRefAndAorn
{
    [Test]
    public async Task Returns_Data_For_PayeRef_And_AORN()
    {
        var testFixture = new WhenIGetOrganisationsByPayeRefAndAorn();

        testFixture.DataSourceReturnsData();
        var organisations = await testFixture.Handle();
        testFixture.ReturnDataIsCorrect(organisations);
    }

    [Test]
    public void Propagates_Errors()
    {
        var testFixture = new WhenIGetOrganisationsByPayeRefAndAorn();

        testFixture.DataRetrievalThrowsException();
        var action = testFixture.HandleExceptionalCase();
        action.Should().Throws<TestException>();
    }

    private string _payeReference;
    private string _aorn;
    private List<Organisation> _repositoryResults;
    private IRequestHandler<GetOrganisationsByPayeRefAndAorn, IEnumerable<Organisation>> _sut;

    private void DataRetrievalThrowsException()
    {
        var mockRepository = Substitute.For<IOrganisationRepository>();

        _payeReference = Guid.NewGuid().ToString().Substring(0, 25);
        _aorn = Guid.NewGuid().ToString().Substring(0, 25);
        mockRepository
            .GetOrganisationsForPAYEReferenceAndAORN(_payeReference, _aorn)
            .Throws<TestException>();

        _sut
            =
            new GetOrganisationsByPayeRefAndAornHandler(mockRepository);
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
                    new GetOrganisationsByPayeRefAndAorn(_payeReference, _aorn),
                    CancellationToken.None);
    }

    private async Task HandleExceptionalCase()
    {
        await
            _sut
                .Handle(
                    new GetOrganisationsByPayeRefAndAorn(_payeReference, _aorn),
                    CancellationToken.None);
    }

    private void DataSourceReturnsData()
    {
        _repositoryResults = new Fixture()
            .CreateMany<Organisation>(
                new Random()
                    .Next(1, 15)).ToList();

        var mockRepository = Substitute.For<IOrganisationRepository>();

        _payeReference = Guid.NewGuid().ToString().Substring(0, 25);
        _aorn = Guid.NewGuid().ToString().Substring(0, 25);
        mockRepository
            .GetOrganisationsForPAYEReferenceAndAORN(_payeReference, _aorn)
            .Returns(_repositoryResults);

        _sut = new GetOrganisationsByPayeRefAndAornHandler(mockRepository);
    }
}