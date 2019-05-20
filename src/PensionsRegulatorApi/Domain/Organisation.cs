namespace PensionsRegulatorApi.Domain
{
    public class Organisation
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public int UniqueIdentity { get; set; }
        public Address Address { get; set; }
    }
}