using System;
using System.Collections.Generic;
using MediatR;
using PensionsRegulatorApi.Domain;

namespace PensionsRegulatorApi.Application.Queries
{
    public class GetOrganisations : IRequest<IEnumerable<Organisation>>
    {
        public GetOrganisations(string payeReference)
        {
            if (string.IsNullOrWhiteSpace(payeReference))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(payeReference));
            PAYEReference = payeReference;
        }

        public string PAYEReference { get;  }
    }
}