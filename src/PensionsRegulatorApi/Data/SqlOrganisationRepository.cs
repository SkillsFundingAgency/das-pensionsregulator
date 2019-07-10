using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Options;
using PensionsRegulatorApi.Configuration;
using PensionsRegulatorApi.Domain;

namespace PensionsRegulatorApi.Data
{
    public class SqlOrganisationRepository : IOrganisationRepository
    {
        private readonly string _connectionString;

        public SqlOrganisationRepository(IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionString = connectionStrings.Value.PensionsRegulatorSql;
        }

        public IEnumerable<Organisation> GetOrganisationsForPAYEReference(string payeReference)
        {

            return
                RetrieveRowsAndMapToOrganisations(
                    connection => new SqlCommand()
                    {
                        CommandText = @"[dbo].GetOrganisationsByPAYEReference",
                        CommandType = CommandType.StoredProcedure,
                        Connection = connection,
                        Parameters =
                        {
                            new SqlParameter
                            {
                                ParameterName = "@PAYESchemeReference",
                                Value = payeReference
                            }
                        }
                    });
        }

        public IEnumerable<Organisation> GetOrganisationsForPAYEReferenceAndAORN(
            string payeReference,
            string aorn)
        {
            return
                RetrieveRowsAndMapToOrganisations(
                    connection => new SqlCommand()
                    {
                        CommandText = @"[dbo].[GetOrganisationsByPAYEReferenceAndAORN]",
                        CommandType = CommandType.StoredProcedure,
                        Connection = connection,
                        Parameters =
                        {
                            new SqlParameter
                            {
                                ParameterName = "@PAYESchemeReference",
                                Value = payeReference
                            },
                            new SqlParameter
                            {
                                ParameterName = "@AORN",
                                Value = aorn
                            }
                        }
                    });
        }

        private IEnumerable<Organisation> RetrieveRowsAndMapToOrganisations(
            Func<SqlConnection, SqlCommand> commandToExecute)
        {
            var retrievedOrganisations = new List<Organisation>(0);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = commandToExecute(connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            retrievedOrganisations
                                .Add(
                                    MapDataReaderToOrganisation(
                                        reader));
                        }
                    }
                }
            }

            return retrievedOrganisations.AsReadOnly();
        }

        private Organisation MapDataReaderToOrganisation(SqlDataReader reader)
        {
            return
                new Organisation
                {
                    Name = reader["OrganisationName"].ToString(),
                    Status = reader["OrganisationStatus"].ToString(),
                    UniqueIdentity = reader.GetInt64(2),
                    Address =
                        new Address
                        {
                            Line1 = reader["AddressLine1"].ToString(),
                            Line2 = reader["AddressLine2"].ToString(),
                            Line3 = reader["AddressLine3"].ToString(),
                            Line4 = reader["AddressLine4"].ToString(),
                            Line5 = reader["AddressLine5"].ToString(),
                            Postcode = reader["PostCode"].ToString()
                        }
                };
        }
    }
}