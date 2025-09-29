using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nrc9.Data.Models
{
    public class di_ExtraSurveyQuestionWorkDaysOutput
    {
        public di_ExtraSurveyQuestionWorkDaysOutput()
        {
            IsOk = true;
            ErrorMessage = string.Empty;
            di_ExtraSurveyQuestionWorkDaysOutputColumnsList =
                new List<di_ExtraSurveyQuestionWorkDaysOutputColumns>();
        }
        public bool IsOk {  get; set; }
        public string ErrorMessage { get; set; }
        public List<di_ExtraSurveyQuestionWorkDaysOutputColumns>
            di_ExtraSurveyQuestionWorkDaysOutputColumnsList
            { get; set; }
    }
}
