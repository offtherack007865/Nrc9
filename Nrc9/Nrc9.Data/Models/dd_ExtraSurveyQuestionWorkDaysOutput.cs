using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nrc9.Data.Models
{
    public class dd_ExtraSurveyQuestionWorkDaysOutput
    {
        public dd_ExtraSurveyQuestionWorkDaysOutput()
        {
            IsOk = true;
            ErrorMessage = string.Empty;
            dd_ExtraSurveyQuestionWorkDaysOutputColumnsList =
                new List<dd_ExtraSurveyQuestionWorkDaysOutputColumns> ();
        }
        public bool IsOk { get; set; }
        public string ErrorMessage { get; set; }
        public List<dd_ExtraSurveyQuestionWorkDaysOutputColumns>
            dd_ExtraSurveyQuestionWorkDaysOutputColumnsList
            { get; set; }
    }
}
