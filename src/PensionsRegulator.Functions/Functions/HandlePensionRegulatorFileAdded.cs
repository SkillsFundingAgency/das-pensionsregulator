using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PensionsRegulator.Functions.Domain.Services;

namespace PensionsRegulator.Functions
{
    public class HandlePensionRegulatorFileAdded
    {
        private readonly IPensionRegulatorImportService _prImportService;
        private readonly ILogger _log;

        public HandlePensionRegulatorFileAdded(IPensionRegulatorImportService prImportService, ILogger log)
        {
            _prImportService = prImportService;
            _log = log;
        }

        [FunctionName("HandlePensionRegulatorFileAdded")]
        public void Run(
            [BlobTrigger("%PensionRegulatorBlobPath%", Connection = "PensionsRegulatorStorageConnectionString")]Stream myBlob, string name)
        {
            _log.LogInformation("HandlePensionRegulatorFileAdded blob trigger function processed a request.");

            _log.LogInformation($"Begin processing of blob {name}");

            var watch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                _prImportService.RegisterNewTrpFile(name);

                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;

                _log.LogInformation($"Processing of blob '{name}' completed successfully.");
                _log.LogInformation($"blob {{{name}}} processed in {elapsedMs} ms.");

            }
            catch (Exception ex)
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;

                _log.LogError($"Processing of blob '{name}' Failed", ex);
                _log.LogInformation($"blob '{name}' processed in {elapsedMs} ms.");

                throw;
            }

        }
    }
}
