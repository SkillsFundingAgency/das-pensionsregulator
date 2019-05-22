using System;
using MediatR;
using PensionsRegulatorApi.Domain;

namespace SFA.DAS.PensionsRegulator.TestDataGenerator.Commands
{
    public class CreateSingleOrganisation : IRequest<SingleOrganisationCreated>
    {
        public CreateSingleOrganisation(Organisation organisation, string payeRef)
        {
            if (string.IsNullOrWhiteSpace(payeRef))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(payeRef));
            Organisation = organisation ?? throw new ArgumentNullException(nameof(organisation));
            PayeRef = payeRef;
        }

        public Organisation Organisation { get; }

        public string PayeRef { get; }
    }
}