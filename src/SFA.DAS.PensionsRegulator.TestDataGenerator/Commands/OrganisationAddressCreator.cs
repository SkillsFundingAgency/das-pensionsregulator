using System;
using MediatR;
using SFA.DAS.PensionsRegulator.TestDataGenerator.Data;

namespace SFA.DAS.PensionsRegulator.TestDataGenerator.Commands
{
    public class OrganisationAddressCreator : RequestHandler<CreateOrganisationAddress>
    {
        private readonly SqlOrganisationRepository _repository;

        public OrganisationAddressCreator(
            SqlOrganisationRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        protected override void Handle(CreateOrganisationAddress request)
        {
            _repository
                .CreateOrganisationAddress(
                    request
                        .Address,
                    request
                        .EmployerSurrogateKey);
        }
    }
}