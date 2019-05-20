﻿using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PensionsRegulatorApi.Domain;

namespace PensionsRegulatorApi.Data
{
    public class SqlOrganisationRepository : OrganisationRepository
    {
        private readonly string _connectionString;

        public SqlOrganisationRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Organisation> GetOrganisationsForPAYEReference(string payeReference)
        {
            var retrievedOrganisations = new List<Organisation>(0);

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand()
                {
                    CommandText = @"[dbo].GetOrganisationsByPAYEReference",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection,
                    Parameters = { new SqlParameter{ParameterName = "@PAYESchemeReference", Value = payeReference } }
                })
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                          retrievedOrganisations
                              .Add(
                                  new Organisation
                                  {
                                      Name = reader["Name"].ToString(),
                                      Status = reader["Status"].ToString(),
                                      UniqueIdentity = reader.GetInt32(2),
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
                                  });
                        }
                    }
                }
            }

            return retrievedOrganisations.AsReadOnly();
        }
    }
}