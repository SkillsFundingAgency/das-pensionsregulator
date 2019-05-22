namespace SFA.DAS.PensionsRegulator.TestDataGenerator.Commands
{
    public class SingleOrganisationCreated
    {
        public SingleOrganisationCreated(int employerSurrogateKey)
        {
            EmployerSurrogateKey = employerSurrogateKey;
        }

        public int EmployerSurrogateKey { get;  }
    }
}