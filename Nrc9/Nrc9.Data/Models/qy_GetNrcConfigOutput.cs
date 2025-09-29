using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nrc9.Data.Models
{
    public class qy_GetNrcConfigOutput
    {
        public qy_GetNrcConfigOutput()
        {
            IsOk = true;
            ErrorMessage = string.Empty;
            qy_GetNrcConfigOutputColumnsList =
                new List<qy_GetNrcConfigOutputColumns>();
        }
        public bool IsOk { get; set; }
        public string ErrorMessage { get; set; }
        public List<qy_GetNrcConfigOutputColumns> qy_GetNrcConfigOutputColumnsList { get; set; }

    }
}
