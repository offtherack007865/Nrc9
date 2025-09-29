using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nrc9.Data.Models
{
    public class ExtractDataFromCsvRowOutput
    {
        public ExtractDataFromCsvRowOutput()
        {
            IsOk = true;
            ErrorMessage = string.Empty;
            OutputCsvString = string.Empty;
        }
        public bool IsOk { get; set; }
        public string ErrorMessage { get; set; }
        public string OutputCsvString { get; set; }
    }
}
