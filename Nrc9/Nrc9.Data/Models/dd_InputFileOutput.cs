using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nrc9.Data.Models
{
    public class dd_InputFileOutput
    {
        public dd_InputFileOutput()
        {
            IsOk = true;
            ErrorMessage = string.Empty;
            dd_InputFileOutputColumnsList =
                new List<dd_InputFileOutputColumns> ();

        }
        public bool IsOk { get; set; }
        public string ErrorMessage { get; set; }
        public List<dd_InputFileOutputColumns>
            dd_InputFileOutputColumnsList
            { get; set; }
    }
}
