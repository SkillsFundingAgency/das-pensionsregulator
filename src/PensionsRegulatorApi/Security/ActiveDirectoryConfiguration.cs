namespace PensionsRegulatorApi.Security;

public record ActiveDirectoryConfiguration
{
    public string Tenant { get; set; }
    public string IdentifierUri { get; set; }
}