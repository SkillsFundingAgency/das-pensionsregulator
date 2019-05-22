using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using PensionsRegulatorApi.Domain;

namespace SFA.DAS.PensionsRegulator.TestDataGenerator.Data
{
    public class SqlOrganisationRepository
    {
        private readonly string _connectionString;

        public SqlOrganisationRepository(
            string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(connectionString));
            _connectionString = connectionString;
        }

        public int CreateSingleOrganisation(string name, int uniqueId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand()
                {
                    CommandText = $"INSERT [dbo].[Organisation] ([TPR_Unique_Id], [OrganisationName]) VALUES ({uniqueId}, '{name}') SELECT CAST(SCOPE_IDENTITY() AS INT)",
                    CommandType = CommandType.Text,
                    Connection = connection,
                })
                {
                    return
                        (int) command
                            .ExecuteScalar();
                }
            }
        }

        public void CreateOrganisationPayeRef(
            string payeRef,
            int employerSurrogateKey)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand()
                {
                    CommandText =
                        $"INSERT [dbo].[OrganisationPAYEScheme] ([Employer_SK], [PAYESchemeRef], [SchemeType], [CountOfEmployments], [TaxYear], [LatestAnnualisedGrossEarningForPAYEScheme], [CountOfEmployees], [Status], [CreatedDate], [UpdatedDate], [SourceTableName], [SourceSK]) VALUES ({employerSurrogateKey}, '{payeRef}', 1, 100, 1819, 1000000, 200, N'TestStatus', CAST(N'2019-04-19T00:00:00.0000000' AS DateTime2), CAST(N'2019-04-19T00:00:00.0000000' AS DateTime2), N'n/a', 0)",
                    CommandType = CommandType.Text,
                    Connection = connection,
                })
                {
                    command
                        .ExecuteNonQuery();
                }
            }
        }

        public void CreateOrganisationAddress(
            Address addressToCreate,
            int employerSurrogateKey)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand()
                {
                    CommandText =
                        $"INSERT [dbo].[OrganisationAddress] ([Employer_SK], [AddressLine1], [AddressLine2], [AddressLine3], [AddressLine4], [AddressLine5], [PostCode], [OrganisationFullName], [OrganisationFullAddress]) VALUES ({employerSurrogateKey}, '{addressToCreate.Line1.Substring(0, 35)}', '{addressToCreate.Line2.Substring(0, 35)}', '{addressToCreate.Line3.Substring(0, 35)}', '{addressToCreate.Line4.Substring(0, 35)}', '{addressToCreate.Line5.Substring(0, 35)}', '{addressToCreate.Postcode.Substring(0, 10)}', 'Full_Name', 'Full_Address')",
                    CommandType = CommandType.Text,
                    Connection = connection,
                })
                {
                    command
                        .ExecuteNonQuery();
                }
            }
        }
    }
}