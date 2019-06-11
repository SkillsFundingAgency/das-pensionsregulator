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
            if (request.AccountOfficeReferenceNumberPrefix == null)
                return CreateSingleOrganisationWithoutAccountOfficeReferenceNumber(request);

            return CreateSingleOrganisationWithAccountOfficeReferenceNumber(request);
        }

        private SingleOrganisationCreated CreateSingleOrganisationWithoutAccountOfficeReferenceNumber(
            CreateSingleOrganisation request)
        {
            int createdKey =
                _repository
                    .CreateSingleOrganisation(
                        name: request.Organisation.Name,
                        uniqueId: request.Organisation.UniqueIdentity);

            return new SingleOrganisationCreated(
                createdKey
            );
        }

        private SingleOrganisationCreated CreateSingleOrganisationWithAccountOfficeReferenceNumber(
            CreateSingleOrganisation request)
        {
            var accountOfficeReferenceNumber =
                (request
                     .AccountOfficeReferenceNumberPrefix +
                 '-' +
                 Guid.NewGuid()
                     .ToString("N")
                )
                .Substring(
                    0,
                    25);

            int createdKey =
                _repository
                    .CreateSingleOrganisationWithAccountOfficeReferenceNumber(
                        name: request.Organisation.Name,
                        uniqueId: request.Organisation.UniqueIdentity,
                        accountOfficeReferenceNumber: accountOfficeReferenceNumber);

            return new SingleOrganisationCreated(
                createdKey
            );
        }
    }
}