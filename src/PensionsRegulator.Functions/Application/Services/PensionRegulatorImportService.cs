using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using PensionsRegulator.Functions.Domain.Data;
using PensionsRegulator.Functions.Domain.Services;



namespace PensionsRegulator.Functions.Application.Services
{
    public class PensionRegulatorImportService : IPensionRegulatorImportService
    {
        private readonly ILogger _log;
        private readonly IPensionRegulatorRepository _pensionRegulatorRepository;

        public PensionRegulatorImportService(ILogger log, IPensionRegulatorRepository pensionRegulatorRepository)
        {
            _log = log;
            _pensionRegulatorRepository = pensionRegulatorRepository;
        }

        public void ProcessFiles()
        {
            _pensionRegulatorRepository.LoadPensionRegulatorFile();
        }

        public void RegisterNewTrpFile(string fileName)
        {
            if (String.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("A valid filename must be provided but the value is either NULL or empty");
            }

            _pensionRegulatorRepository.InsertPensionRegulatorFilename(fileName);
        }
    }
}
