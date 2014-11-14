using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace HeadSpin.Core.Utilities
{
    public static class ConfigHelper
    {
        public static int GetAppSettingValueAsInt(string settingName)
        {
            return int.Parse(GetAppSettingValue(settingName));
        }

        public static int GetAppSettingValueAsInt(string settingName, int defaultVal)
        {
            return int.Parse(GetAppSettingValue(settingName, defaultVal.ToString()));
        }

        public static string GetAppSettingValue(string settingName)
        {
            string config = ConfigurationManager.AppSettings[settingName];

            if (string.IsNullOrWhiteSpace(config))
            {
                throw new Exception(string.Format("The app settting {0} must be specified", settingName));
            }

            return config;
        }

        public static string GetAppSettingValue(string settingName, string defaultVal)
        {
            string config = ConfigurationManager.AppSettings[settingName];

            if (string.IsNullOrWhiteSpace(config))
            {
                return defaultVal;
            }

            return config;
        }
    }
}
