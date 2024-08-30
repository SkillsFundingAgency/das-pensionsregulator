namespace PensionsRegulatorApi.Domain;

public record Organisation
{
    public string Name { get; set; }
    public string Status { get; set; }
    public long UniqueIdentity { get; set; }
    public Address Address { get; set; }
}