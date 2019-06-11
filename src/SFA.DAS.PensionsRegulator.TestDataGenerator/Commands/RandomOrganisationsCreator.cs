using System;
using System.Collections.Generic;
using AutoFixture;
using MediatR;
using PensionsRegulatorApi.Domain;
using SFA.DAS.PensionsRegulator.TestDataGenerator.Data;

namespace SFA.DAS.PensionsRegulator.TestDataGenerator.Commands
{
    public class RandomOrganisationsCreator : RequestHandler<CreateRandomNumberOfOrganisations, RandomOrganisationsCreated>
    {
        private readonly SqlOrganisationRepository _repository;
        private readonly IMediator _mediator;
        private Fixture _fixture;

        public RandomOrganisationsCreator(
            SqlOrganisationRepository repository,
            IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            _fixture = new Fixture();
        }
        protected override RandomOrganisationsCreated Handle(CreateRandomNumberOfOrganisations request)
        {
            var createdKeys = new HashSet<int>();
            var generatedOrganisations
                =
            _fixture
                .CreateMany<Organisation>(
                    new Random()
                        .Next(1, 10));

            int createdKey;

            foreach (var organisation in generatedOrganisations)
            {
                createdKey = createSingleOrganisation(request, organisation);

                createdKeys.Add(createdKey);

                createPayeRef(request.PayeRef, createdKey);

                createAddress(createdKey, organisation.Address);
            }

            return 
                new RandomOrganisationsCreated(
                    request.PayeRef, 
                    createdKeys);
        }

        private void createAddress(int createdKey, Address organisationAddress)
        {
            _mediator
                .Send(
                    new CreateOrganisationAddress(
                        organisationAddress,
                        createdKey));
        }

        private void createPayeRef(string requestPayeRef, int createdKey)
        {
            _mediator
                .Send(
                    new CreateOrganisationPayeRef(
                        createdKey,
                        requestPayeRef));
        }

        private int createSingleOrganisation(CreateRandomNumberOfOrganisations request, Organisation organisation)
        {
            return
                _mediator
                    .Send(
                        new CreateSingleOrganisation(
                            organisation,
                            request.AccountOfficeReferenceNumber
                        ))
                    .Result
                    .EmployerSurrogateKey;
        }
    }
}