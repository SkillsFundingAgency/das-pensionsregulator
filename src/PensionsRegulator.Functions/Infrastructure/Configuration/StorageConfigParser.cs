using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using PensionsRegulator.Functions.Domain.Configuration;

namespace PensionsRegulator.Functions.Infrastructure.Configuration
{
    public class StorageConfigParser
    {
        public Dictionary<string, string> ParseConfig(ConfigurationItem configItem, string defaultSectionName)
        {
            var configDictionary = new Dictionary<string, string>();

            var jsonObject = JObject.Parse(configItem.Data);

            foreach (var child in jsonObject.Children())
            {
                foreach (var jToken in child.Children().Children())
                {
                    var child1 = (JProperty)jToken;
                    configDictionary.Add($"{child.Path}:{child1.Name}", child1.Value.ToString());
                }

                if (string.IsNullOrEmpty(defaultSectionName))
                {
                    continue;
                }

                foreach (var jToken in child.Children())
                {
                    if (!jToken.Children().Any())
                    {
                        configDictionary.Add($"{defaultSectionName}:{jToken.Path}", jToken.Value<string>());
                    }
                }

            }
            return configDictionary;
        }
    }
}
