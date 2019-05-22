using MediatR;

namespace SFA.DAS.PensionsRegulator.TestDataGenerator.Commands
{
    public class CreateOrganisationPayeRef : IRequest
    {
        public CreateOrganisationPayeRef(int employerSurrogateKey, string payeRef)
        {
            EmployerSurrogateKey = employerSurrogateKey;
            PayeRef = payeRef;
        }

        public int EmployerSurrogateKey { get; }

        public string PayeRef { get; }
    }
}