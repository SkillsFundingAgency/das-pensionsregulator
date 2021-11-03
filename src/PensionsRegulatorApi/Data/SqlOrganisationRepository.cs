using PensionsRegulatorApi.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PensionsRegulatorApi.Data
{
    public class SqlOrganisationRepository : IOrganisationRepository
    {
        private readonly IDbConnection _connection;

        public SqlOrganisationRepository(IDbConnection connection)
        {
            _connection = connection;
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

            using (var command = commandToExecute(_connection as SqlConnection))
            {
                _connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        retrievedOrganisations.Add(MapDataReaderToOrganisation(reader));
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