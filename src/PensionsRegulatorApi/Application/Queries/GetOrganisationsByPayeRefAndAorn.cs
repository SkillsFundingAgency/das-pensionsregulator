using PensionsRegulatorApi.Domain;

namespace PensionsRegulatorApi.Application.Queries;

public class GetOrganisationsByPayeRefAndAorn : IRequest<IEnumerable<Organisation>>
{
    public string PAYEReference { get;  }
    public string AccountOfficeReferenceNumber { get; }
    
    public GetOrganisationsByPayeRefAndAorn(string payeReference, string accountOfficeReferenceNumber)
    {
        if (string.IsNullOrWhiteSpace(payeReference))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(payeReference));
        }

        if (string.IsNullOrWhiteSpace(accountOfficeReferenceNumber))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(accountOfficeReferenceNumber));
        }

        PAYEReference = payeReference;
        AccountOfficeReferenceNumber = accountOfficeReferenceNumber;
    }
}