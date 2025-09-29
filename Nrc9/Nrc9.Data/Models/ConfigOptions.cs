using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nrc9.Data.Models
{
    public class ConfigOptions
    {
        public string BaseWebUrl { get; set; }

        public string DbConfigSettingsApplication { get; set; }
        public string DbConfigSettingsType { get; set; }
        public string DbConfigSettingsProcess { get; set; }
        public string DbConfigSettingsNameFilter { get; set; }
        public string DbConfigSettingsUser { get; set; }

    }
}
