using PensionsRegulatorApi.Domain;

namespace PensionsRegulatorApi.Application.Queries;

public class GetOrganisationsByPayeRef : IRequest<IEnumerable<Organisation>>
{
    public string PAYEReference { get;  }
        
    public GetOrganisationsByPayeRef(string payeReference)
    {
        if (string.IsNullOrWhiteSpace(payeReference))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(payeReference));
        }

        PAYEReference = payeReference;
    }
}