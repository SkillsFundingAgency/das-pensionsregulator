using System;
using System.Reflection;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.PensionsRegulator.TestDataGenerator.Commands;
using SFA.DAS.PensionsRegulator.TestDataGenerator.Data;

namespace SFA.DAS.PensionsRegulator.TestDataGenerator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Attempting to create one Organisation.");

            try
            {
                var provider = ConfigureServices(String.Empty);

                var mediatr = provider.GetService<IMediator>();

                await 
                    mediatr
                        .Send(
                            new CreateRandomNumberOfOrganisations(String.Empty));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("Done.");
        }

        public static IServiceProvider ConfigureServices(string connectionString)
        {
            var services = new ServiceCollection();

            services.AddMediatR(typeof(Program).GetTypeInfo().Assembly);

            services.AddTransient<SqlOrganisationRepository>( sp => new SqlOrganisationRepository(connectionString));

            return ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(services);
        }
    }
}
