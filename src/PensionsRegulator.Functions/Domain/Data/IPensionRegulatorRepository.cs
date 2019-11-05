namespace PensionsRegulator.Functions.Domain.Data
{
    public interface IPensionRegulatorRepository
    {
        void LoadPensionRegulatorFile();
        void InsertPensionRegulatorFilename(string filename);
    }
}