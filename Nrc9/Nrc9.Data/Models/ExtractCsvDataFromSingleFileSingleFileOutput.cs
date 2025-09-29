using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nrc9.Data.Models
{
    public class ExtractCsvDataFromSingleFileSingleFileOutput
    {
        public ExtractCsvDataFromSingleFileSingleFileOutput()
        {
            IsOk = true;
            ErrorMessage = string.Empty;
            MyCsvLineList = new List<string>();
        }
        public bool IsOk { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> MyCsvLineList { get; set; }
    }
}
