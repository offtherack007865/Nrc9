using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BulkInsert9.Data.Models;

namespace Nrc9.ConsoleApp
{
    public class MassageDataProducingBulkInsertCsvLineList
    {
        public MassageDataProducingBulkInsertCsvLineList
        (
            List<string> inputCsvLines
            , List<spGetColumnDefsForGivenDbAndTableName_OutputColumns> inputColDefs
        )
        {
            MyCsvLines = inputCsvLines;
            MyColDefs = inputColDefs;
        }

        public List<string> MyCsvLines { get; set; }
        public List<spGetColumnDefsForGivenDbAndTableName_OutputColumns> MyColDefs { get; set; }

        public List<string> DoIt()
        {
            List<string> returnOutput = new List<string>();

            foreach (string loopInputLine in MyCsvLines)
            {
                string[] parts = loopInputLine.Split(',');
                if (parts.Length != MyColDefs.Count)
                {
                    return returnOutput;
                }

                List<string> outputPartsList = new List<string>();
                for (int partCtr = 0; partCtr < parts.Length; partCtr++)
                {
                    string myPart = parts[partCtr];

                    spGetColumnDefsForGivenDbAndTableName_OutputColumns  myColDef = MyColDefs[partCtr];

                    // For string tops adjust for max length.
                    if (
                            myColDef.MyDbColumnType.CompareTo("varchar") == 0
                            ||
                            myColDef.MyDbColumnType.CompareTo("nvarchar") == 0
                       )
                    {
                        if (myPart.Length > MyColDefs[partCtr].MyDbColumnLength)
                        {
                            outputPartsList.Add(myPart.Substring(0, myColDef.MyDbColumnLength));
                        }
                        else
                        {
                            outputPartsList.Add(myPart);
                        }
                    }

                    // For datetime MM/dd/yyyy 
                    else if (
                               MyColDefs[partCtr].MyDbColumnType.CompareTo("datetime MM/dd/yyyy") == 0
                            )
                    {
                        
                        if (myPart.Length < 8 || myPart.Split('/').Length != 3)
                        {
                            outputPartsList.Add("01/01/1900");
                        }
                        else
                        {
                            DateTime myDateTime = new DateTime(1900, 1, 1);
                            string[] myDateParts = myPart.Split("/");
                            string monthCharacterString = myDateParts[0];
                            string dateCharacterString = myDateParts[1];
                            string yearCharacterString = myDateParts[2];

                            int MMInt = 0;
                            int ddInt = 0;
                            int yyyyInt = 0;
                            int.TryParse(monthCharacterString, out MMInt);
                            int.TryParse(dateCharacterString, out ddInt);
                            int.TryParse(yearCharacterString, out yyyyInt);

                            if (MMInt <= 0 || ddInt <= 0 || yyyyInt <= 0)
                            {
                                outputPartsList.Add("01/01/1900");
                            }
                            else
                            {
                                string myDateInMMSlashddSlashyyyyFormat = $"{MMInt.ToString().PadLeft(2, '0')}/{ddInt.ToString().PadLeft(2, '0')}/{yyyyInt.ToString()}";
                                DateTime.TryParse(myDateInMMSlashddSlashyyyyFormat, out myDateTime);
                                if (myDateTime == new DateTime(1900, 1, 1))
                                {
                                    outputPartsList.Add("01/01/1900");
                                }
                                else
                                {
                                    outputPartsList.Add(myDateInMMSlashddSlashyyyyFormat);
                                }
                            }
                        }
                    }
                }
                returnOutput.Add(string.Join(",", outputPartsList));
            }









            return returnOutput;
        }

    }
}
