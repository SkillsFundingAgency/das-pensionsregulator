using System;
using MediatR;
using SFA.DAS.PensionsRegulator.TestDataGenerator.Data;

namespace SFA.DAS.PensionsRegulator.TestDataGenerator.Commands
{
    public class OrganisationPayeRefCreator : RequestHandler<CreateOrganisationPayeRef>
    {
        private readonly SqlOrganisationRepository _repository;

        public OrganisationPayeRefCreator(
            SqlOrganisationRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        protected override void Handle(CreateOrganisationPayeRef request)
        {
            _repository
                .CreateOrganisationPayeRef(
                    request
                        .PayeRef,
                    request
                        .EmployerSurrogateKey);
        }
    }
}