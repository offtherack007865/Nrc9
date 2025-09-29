using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nrc9.ConsoleApp
{
    public class ReadInConfigOptions
    {
        public ReadInConfigOptions(Microsoft.Extensions.Configuration.IConfiguration myConfig)
        {
            MyConfig = myConfig;
        }

        public Microsoft.Extensions.Configuration.IConfiguration MyConfig { get; set; }

        public Nrc9.Data.Models.ConfigOptions ReadIn()
        {
            Nrc9.Data.Models.ConfigOptions
                returnConfigOptions =
                new Nrc9.Data.Models.ConfigOptions();

            returnConfigOptions.BaseWebUrl =
                MyConfig.GetValue<string>(Nrc9.Data.MyConstants.BaseWebUrl);
            returnConfigOptions.DbConfigSettingsApplication =
                MyConfig.GetValue<string>(Nrc9.Data.MyConstants.DbConfigSettingsApplication);

            returnConfigOptions.DbConfigSettingsType =
                MyConfig.GetValue<string>(Nrc9.Data.MyConstants.DbConfigSettingsType);
            returnConfigOptions.DbConfigSettingsProcess =
                MyConfig.GetValue<string>(Nrc9.Data.MyConstants.DbConfigSettingsProcess);
            returnConfigOptions.DbConfigSettingsNameFilter =
                MyConfig.GetValue<string>(Nrc9.Data.MyConstants.DbConfigSettingsNameFilter);
            returnConfigOptions.DbConfigSettingsUser =
                MyConfig.GetValue<string>(Nrc9.Data.MyConstants.DbConfigSettingsUser);
            return returnConfigOptions;

        }
    }
}
