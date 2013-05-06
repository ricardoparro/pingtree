using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using EpPingtree.Datalayer.Interfaces;

namespace EpPingtree.Datalayer.Repository
{
    public class ConfigRepository : BaseRepository, IConfigRepository
    {

        public bool IsLive
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["IsLive"]); }
        }

        


        /// <summary>
        /// Checks whether there is a specific setting for the tier otherwise get the general setting
        /// </summary>
        private string GetConfigValueForTier(string tierPrefix, int tierNumber, string tierSuffix)
        {
            if (!tierPrefix.EndsWith("."))
                tierPrefix += ".";

            //First check whether a key for that particular tier
            string configKey = tierPrefix + "Tier" + tierNumber + "." + tierSuffix;
            string configValue = ConfigurationManager.AppSettings[configKey];

            //No key for that tier, check general setting
            if (string.IsNullOrEmpty(configValue))
                configValue = ConfigurationManager.AppSettings[tierPrefix + tierSuffix];

            //Return
            return configValue;
        }
    }
}
