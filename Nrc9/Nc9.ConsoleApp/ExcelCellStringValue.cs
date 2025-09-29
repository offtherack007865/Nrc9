using Nrc9.Data.Models;
using Spire.Xls;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nrc9.ConsoleApp
{
    public class ExcelCellStringValue
    {
        public ExcelCellStringValue
        (
            Worksheet inputWorksheet
            , qy_GetNrcConfigOutputColumns inputConfigOptions
            , string inputColumnName
            , int inputRowNumber
            , int inputColumnNumber
        )
        {
            MyWorksheet = inputWorksheet;
            MyConfigOptions = inputConfigOptions;
            MyColumnName = inputColumnName;
            MyRowNumber = inputRowNumber;
            MyColumnNumber = inputColumnNumber;
            if (MyColumnNumber == 14)
            {
                int i = 0;
                i++;
            }
        }
        public Worksheet MyWorksheet { get; set; }
        public qy_GetNrcConfigOutputColumns MyConfigOptions { get; set; }
        public string MyColumnName { get; set; }
        public int MyRowNumber { get; set; }
        public int MyColumnNumber { get; set; }
        public string MyColumnDesignation
        {
            get
            {
                switch (MyColumnNumber)
                {
                    case 1:
                        return "A";
                    case 2:
                        return "B";
                    case 3:
                        return "C";
                    case 4:
                        return "D";
                    case 5:
                        return "E";
                    case 6:
                        return "F";
                    case 7:
                        return "G";
                    case 8:
                        return "H";
                    case 9:
                        return "I";
                    case 10:
                        return "J";
                    case 11:
                        return "K";
                    case 12:
                        return "L";
                    case 13:
                        return "M";
                    case 14:
                        return "N";
                    case 15:
                        return "O";
                    case 16:
                        return "P";
                    case 17:
                        return "Q";
                    case 18:
                        return "R";
                    case 19:
                        return "S";
                    case 20:
                        return "T";
                    case 21:
                        return "U";
                    case 22:
                        return "V";
                    case 23:
                        return "W";
                    case 24:
                        return "X";
                    case 25:
                        return "Y";
                    default:
                        return string.Empty;
                }
            }
        }
        public string MyCellDesignation
        {
            get
            {
                if (MyColumnDesignation.Length == 0)
                {
                    return string.Empty;
                }
                return $"{MyColumnDesignation}{MyRowNumber.ToString()}";
            }
        }


        public GetExcelCellStringValueOutput GetExcelCellStringValue()
        {
            GetExcelCellStringValueOutput returnOutput = new GetExcelCellStringValueOutput();
            string? cellValue = null;

            // If we have run out of columns to get return.
            if (MyCellDesignation.Length == 0)
            {
                return returnOutput;
            }
            cellValue = MyWorksheet.Range[MyCellDesignation].Text;

            if (cellValue == null)
            {
                cellValue = MyWorksheet.Range[MyCellDesignation].NumberValue.ToString();
            }


            // If this is NOT the first column and it is null or emptystring, return empty string.
            if (MyColumnNumber != 1 && (cellValue == null || cellValue.Length == 0))
            {
                cellValue = string.Empty;
            }

            cellValue = cellValue.Trim().Replace(',', '^');
            if (cellValue.CompareTo("NaN") == 0)
            {
                cellValue = string.Empty;
            }

            string nonBlankColumnValue = cellValue.Trim();
            // If FullName column has a blank value, 
            if (MyCellDesignation.StartsWith("") && nonBlankColumnValue.Length == 0)
            {
                returnOutput.OutputStringValue = "BlankLine";
            }
            else
            {
                returnOutput.OutputStringValue = nonBlankColumnValue;
            }

            return returnOutput;
        }
    }
}
