using System;
using MediatR;
using SFA.DAS.PensionsRegulator.TestDataGenerator.Data;

namespace SFA.DAS.PensionsRegulator.TestDataGenerator.Commands
{
    public class SingleOrganisationCreator : RequestHandler<CreateSingleOrganisation, SingleOrganisationCreated>
    {
        private readonly SqlOrganisationRepository _repository;

        public SingleOrganisationCreator(
            SqlOrganisationRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        protected override SingleOrganisationCreated Handle(CreateSingleOrganisation request)
        {
            int createdKey =
                _repository
                    .CreateSingleOrganisation(name: request.Organisation.Name,
                        uniqueId: request.Organisation.UniqueIdentity);

            return new SingleOrganisationCreated(
                createdKey
            );
        }
    }
}