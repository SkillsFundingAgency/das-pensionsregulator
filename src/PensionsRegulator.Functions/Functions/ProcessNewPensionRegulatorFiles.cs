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
    public class ProcessNewPensionRegulatorFiles
    {
        private readonly IPensionRegulatorImportService _prImportService;
        private ILogger _log;

        public ProcessNewPensionRegulatorFiles(IPensionRegulatorImportService prImportService)
        {
            _prImportService = prImportService;
        }

        [FunctionName("HttpProcessNewPensionRegulatorFiles")]
        public void Run(
            [HttpTrigger(AuthorizationLevel.Function)]HttpRequest req, ILogger log)
        {
            _log = log;

            _log.LogInformation("HttpProcessNewPensionRegulatorFiles Http trigger function processed a request.");

            TriggerPensionRegulatorFileImport();
        }

        [FunctionName("TimerProcessNewPensionRegulatorFiles")]
        [Timeout("00:30:00")]
        public void RunTimer([TimerTrigger("%PensionRegulatorImportProcessTimer%")] TimerInfo myTimer, ILogger log)
        {
            _log = log;

            _log.LogInformation("TimerProcessNewPensionRegulatorFiles Timer trigger function processed a request.");

            TriggerPensionRegulatorFileImport();
        }

        private void TriggerPensionRegulatorFileImport()
        {
            _log.LogInformation($"Begining Import of inserted files");

            var watch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                _prImportService.ProcessFiles();

                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;

                _log.LogInformation($"Import of files completed Successfully. Elapsed time: {elapsedMs} ms.");
            }
            catch (Exception ex)
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;

                _log.LogError($"Import of files Failed in {elapsedMs} ms", ex);
                throw;
            }
        }


    }
}
