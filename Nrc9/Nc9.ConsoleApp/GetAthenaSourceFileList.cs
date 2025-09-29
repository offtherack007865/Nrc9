using Nrc9.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nrc9.ConsoleApp
{
    public class GetAthenaSourceFileList
    {
        public GetAthenaSourceFileList(qy_GetNrcConfigOutputColumns inputConfigOptions)
        {
            MyConfigOptions = inputConfigOptions;
        }
        public qy_GetNrcConfigOutputColumns MyConfigOptions { get; set; }

        public List<string> MyOutputListOfFullFilenames { get; set; }   

        public void DoIt()
        {
            MyOutputListOfFullFilenames = Directory.GetFiles(MyConfigOptions.ReadDirectory, $"*{MyConfigOptions.FilenameContainsString}*.txt").ToList();
        }
    }
}
