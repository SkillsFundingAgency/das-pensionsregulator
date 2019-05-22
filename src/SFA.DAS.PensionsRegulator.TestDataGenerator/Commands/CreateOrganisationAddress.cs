using System;
using MediatR;
using PensionsRegulatorApi.Domain;

namespace SFA.DAS.PensionsRegulator.TestDataGenerator.Commands
{
    public class CreateOrganisationAddress : IRequest
    {
        public CreateOrganisationAddress(Address address, int employerSurrogateKey)
        {
            Address = address ?? throw new ArgumentNullException(nameof(address));
            EmployerSurrogateKey = employerSurrogateKey;
        }

        public Address Address { get;  }
        public int EmployerSurrogateKey { get;  }
    }
}