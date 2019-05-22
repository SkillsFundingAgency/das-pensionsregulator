using System;

namespace PensionsRegulatorApi.Dtos
{
    [Obsolete]
    public enum OrganisationSubType
    {
        None = 0,

        // Public sector
        Ons = 1,
        Nhs = 2,
        Police = 3
    }
}