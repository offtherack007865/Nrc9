using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nrc9.ConsoleApp
{
    public class ExtraSurveyQuestion3Of5Workdays
    {
        public ExtraSurveyQuestion3Of5Workdays()
        {
            ExtraSurveyQuestionWorkDayList = new List<int>();
        }
        public List<int> ExtraSurveyQuestionWorkDayList { get; set; }
        public void RandomlyGenerate()
        {
            ExtraSurveyQuestionWorkDayList = new List<int>();
            while(ExtraSurveyQuestionWorkDayList.Count < 1)
            {
                Random random = new Random();
                int randomNumber = random.Next(1, 6);
                if (!ExtraSurveyQuestionWorkDayList.Contains(randomNumber))
                {
                    ExtraSurveyQuestionWorkDayList.Add(randomNumber);
                }
            }
        }
    }
}
