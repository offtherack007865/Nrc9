using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nrc9.Data.Models
{
    public class di_FinalizeInputFileOutput
    {
        public di_FinalizeInputFileOutput()
        {
            IsOk = true;
            ErrorMessage = string.Empty;
            di_FinalizeInputFileOutputColumnsList =
                new List<di_FinalizeInputFileOutputColumns> ();
        }
        public bool IsOk { get; set; }
        public string ErrorMessage { get; set; }
        public List<di_FinalizeInputFileOutputColumns>
            di_FinalizeInputFileOutputColumnsList
            { get; set; }
    }
}
