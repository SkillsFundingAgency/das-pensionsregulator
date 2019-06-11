using System;
using MediatR;
using PensionsRegulatorApi.Domain;

namespace SFA.DAS.PensionsRegulator.TestDataGenerator.Commands
{
    public class CreateSingleOrganisation : IRequest<SingleOrganisationCreated>
    {
        public CreateSingleOrganisation(Organisation organisation, string accountOfficeReferenceNumberPrefix = null)
        {
            Organisation = organisation ?? throw new ArgumentNullException(nameof(organisation));
            AccountOfficeReferenceNumberPrefix = accountOfficeReferenceNumberPrefix;
        }

        public Organisation Organisation { get; }

        public string AccountOfficeReferenceNumberPrefix { get; }
    }
}