using Microsoft.Data.SqlClient;
using PensionsRegulatorApi.Domain;

namespace PensionsRegulatorApi.Data;

public class SqlOrganisationRepository(IDbConnection connection) : IOrganisationRepository
{
    public async Task<IEnumerable<Organisation>> GetOrganisationsForPAYEReference(string payeReference)
    {
        return await RetrieveRowsAndMapToOrganisations(connection => new SqlCommand
        {
            CommandText = "[dbo].GetOrganisationsByPAYEReference",
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

    public async Task<IEnumerable<Organisation>> GetOrganisationsForPAYEReferenceAndAORN(string payeReference,
        string aorn)
    {
        return await RetrieveRowsAndMapToOrganisations(connection => new SqlCommand
        {
            CommandText = "[dbo].[GetOrganisationsByPAYEReferenceAndAORN]",
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

    private async Task<IEnumerable<Organisation>> RetrieveRowsAndMapToOrganisations(
        Func<SqlConnection, SqlCommand> commandToExecute)
    {
        var retrievedOrganisations = new List<Organisation>(0);

        await using (var command = commandToExecute(connection as SqlConnection))
        {
            connection.Open();

            await using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    retrievedOrganisations.Add(MapDataReaderToOrganisation(reader));
                }
            }
        }

        return retrievedOrganisations.AsReadOnly();
    }

    private static Organisation MapDataReaderToOrganisation(SqlDataReader reader)
    {
        return new Organisation
        {
            Name = reader["OrganisationName"].ToString(),
            Status = reader["OrganisationStatus"].ToString(),
            UniqueIdentity = reader.GetInt64(2),
            Address = new Address
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

    public async Task<Organisation> GetOrganisationById(long tprUniqueKey)
    {
        var result = await RetrieveRowsAndMapToOrganisations(connection => new SqlCommand
        {
            CommandText = "[dbo].GetOrganisationByTPRUniqueKey",
            CommandType = CommandType.StoredProcedure,
            Connection = connection,
            Parameters =
            {
                new SqlParameter
                {
                    ParameterName = "@TPRUniqueKey",
                    Value = tprUniqueKey
                }
            }
        });

        return result.SingleOrDefault();
    }
}