using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Logging;
using PensionsRegulator.Functions.Domain.Data;

namespace PensionsRegulator.Functions.Infrastructure.Data
{
    public class PensionRegulatorRepository : IPensionRegulatorRepository
    {

        private readonly string _connectionString;
        private readonly ILogger _logger;

        public PensionRegulatorRepository(string connectionString, ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }


        public void LoadPensionRegulatorFile()
        {
            using (var connection = Connection)
            {
                try
                {
                    connection.Open();
                    connection.Execute("LoadTPRFile", commandType: CommandType.StoredProcedure);
                }
                catch (Exception e)
                {
                    _logger.LogError("SP execution of LoadTPRFile Failed:", e);
                    throw;
                }
            }

        }

        public void InsertPensionRegulatorFilename(string filename)
        {
            //LoadSrcFileDetails    
            using (var connection = Connection)
            {
                try
                {
                    connection.Open();
                    connection.Execute("LoadSrcFileDetails",new {SourceFileName = filename}, commandType: CommandType.StoredProcedure);
                }
                catch (Exception e)
                {
                    _logger.LogError($"SP execution of LoadSrcFileDetails for filename {filename} successfully", e);
                    throw;
                }
            }
        }

        public IDbConnection Connection => new SqlConnection(_connectionString);
    }
}
