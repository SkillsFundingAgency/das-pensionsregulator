using System.Collections.Generic;

namespace SFA.DAS.PensionsRegulator.TestDataGenerator.Commands
{
    public class RandomOrganisationsCreated
    {
        public RandomOrganisationsCreated(string payeRef, IEnumerable<int> createdEmployerSurrogateKeys)
        {
            PayeRef = payeRef;
            CreatedEmployerSurrogateKeys = createdEmployerSurrogateKeys;
        }

        public string PayeRef { get;  }

        public IEnumerable<int> CreatedEmployerSurrogateKeys { get; }
    }
}