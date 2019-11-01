using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Cosmos.Table;
namespace PensionsRegulator.Functions.Domain.Configuration
{
    public class ConfigurationItem : TableEntity
    {
        public string Data { get; set; }
    }
}
