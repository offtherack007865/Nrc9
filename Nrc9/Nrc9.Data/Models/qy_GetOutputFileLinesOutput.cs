using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nrc9.Data.Models
{
    public class qy_GetOutputFileLinesOutput
    {
        public qy_GetOutputFileLinesOutput()
        {
            IsOk = true;
            ErrorMessage = string.Empty;
            qy_GetOutputFileLinesOutputColumnsList =
                new List<qy_GetOutputFileLinesOutputColumns>();
        }
        public bool IsOk { get; set; }
        public string ErrorMessage { get; set; }
        public List<qy_GetOutputFileLinesOutputColumns> qy_GetOutputFileLinesOutputColumnsList { get; set; }
    }
}
