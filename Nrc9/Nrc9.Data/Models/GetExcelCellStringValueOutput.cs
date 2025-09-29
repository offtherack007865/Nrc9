using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nrc9.Data.Models
{
    public class GetExcelCellStringValueOutput
    {
        public GetExcelCellStringValueOutput()
        {
            IsOk = true;
            ErrorMessage = string.Empty;
            OutputStringValue = string.Empty;
        }
        public bool IsOk { get; set; }
        public string ErrorMessage { get; set; }
        public string OutputStringValue { get; set; }
    }
}
