using System;
using MediatR;

namespace SFA.DAS.PensionsRegulator.TestDataGenerator.Commands
{
    public class CreateRandomNumberOfOrganisations : IRequest<RandomOrganisationsCreated>
    {
        public CreateRandomNumberOfOrganisations(string payeRef)
        {
            if (string.IsNullOrWhiteSpace(payeRef))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(payeRef));
            PayeRef = payeRef;
        }

        public string PayeRef { get;  }
    }
}