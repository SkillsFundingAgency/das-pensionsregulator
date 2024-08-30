using NSubstitute.ExceptionExtensions;
using PensionsRegulatorApi.Application.Queries;
using PensionsRegulatorApi.Data;
using PensionsRegulatorApi.Domain;

namespace SFA.DAS.PensionsRegulatorApi.UnitTests.Application.Queries;

[TestFixture]
[ExcludeFromCodeCoverage]
public class WhenIGetOrganisationsByPayeRef
{
    [Test]
    public async Task Returns_Data_For_PayeRef()
    {
        var testFixture = new WhenIGetOrganisationsByPayeRef();

        testFixture.DataSourceReturnsData();
        var organisations = await testFixture.Handle();
        testFixture.ReturnDataIsCorrect(organisations.ToList());
    }

    [Test]
    public void Propagates_Errors()
    {
        var testFixture = new WhenIGetOrganisationsByPayeRef();

        testFixture.DataRetrievalThrowsException();
        var action = testFixture.HandleExceptionalCase();
        action.Should().Throws<TestException>();
    }

    private string _payeReference;
    private List<Organisation> _repositoryResults;
    private GetOrganisationsByPayeRefHandler _sut;

    private void DataRetrievalThrowsException()
    {
        var mockRepository = Substitute.For<IOrganisationRepository>();

        _payeReference = Guid.NewGuid().ToString().Substring(0, 25);
        mockRepository
            .GetOrganisationsForPAYEReference(_payeReference)
            .Throws<TestException>();

        _sut = new GetOrganisationsByPayeRefHandler(mockRepository);
    }

    private void ReturnDataIsCorrect(List<Organisation> organisations)
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
                    .Next(1, 15)).ToList();

        var mockRepository = Substitute.For<IOrganisationRepository>();

        _payeReference = Guid.NewGuid().ToString().Substring(0, 25);
        mockRepository
            .GetOrganisationsForPAYEReference(_payeReference)
            .Returns(_repositoryResults);

        _sut = new GetOrganisationsByPayeRefHandler(mockRepository);
    }
}