using System;
using System.Collections.Generic;

namespace SFA.DAS.PensionsRegulator.TestDataGenerator.Commands
{
    public class RandomOrganisationsCreatedBuilder
    {
        private string _payeRef = string.Empty;
        private HashSet<int> _uniqueKeys = new HashSet<int>();

        public RandomOrganisationsCreatedBuilder ForPayeRef(string payeRef)
        {
            _payeRef = payeRef;

            return this;
        }

        public RandomOrganisationsCreatedBuilder AddCreatedEmployerSurrogateKey(int key)
        {
            _uniqueKeys.Add(key);

            return this;
        }

        RandomOrganisationsCreated Build()
        {
            if(String.IsNullOrWhiteSpace(_payeRef))
                throw new InvalidOperationException("Must have a valid paye ref.");

            return 
                new RandomOrganisationsCreated(
                    payeRef: _payeRef,
                    createdEmployerSurrogateKeys: _uniqueKeys);
        }
    }
}