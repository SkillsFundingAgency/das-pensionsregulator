using System;
using System.Collections.Generic;
using System.Text;

namespace PensionsRegulator.Functions.Domain.Services
{
    public interface IPensionRegulatorImportService
    {
        void ProcessFiles();
        void RegisterNewTrpFile(string fileName);

    }
}
