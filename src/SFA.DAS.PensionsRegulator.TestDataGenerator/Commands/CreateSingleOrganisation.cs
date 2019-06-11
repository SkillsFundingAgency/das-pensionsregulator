using System;
using MediatR;
using PensionsRegulatorApi.Domain;

namespace SFA.DAS.PensionsRegulator.TestDataGenerator.Commands
{
    public class CreateSingleOrganisation : IRequest<SingleOrganisationCreated>
    {
        public CreateSingleOrganisation(Organisation organisation, string accountOfficeReferenceNumber = null)
        {
            Organisation = organisation ?? throw new ArgumentNullException(nameof(organisation));
            AccountOfficeReferenceNumber = accountOfficeReferenceNumber;
        }

        public Organisation Organisation { get; }

        public string AccountOfficeReferenceNumber { get; }
    }
}