using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nrc9.ConsoleApp
{
    public class CleanInideOfDoubleQuotesInEntireFileReturningFileLineList
    {
        public CleanInideOfDoubleQuotesInEntireFileReturningFileLineList(string inputFullFilename)
        {
            MyFullFilename = inputFullFilename;
        }
        public string MyFullFilename { get; set; }

        public List<string> DoIt()
        {
            List<string> returnStringList = new List<string>();

            string cleanedLine =
                CleanInsideOfDoubleQuotesForAllTextInFile();
            List<string>
                rawStringList =
                    cleanedLine.Split("\n").ToList();
            foreach (string loopString in rawStringList)
            {
                if (loopString.Trim().Length > 0)
                {
                    returnStringList.Add(loopString.Trim());
                }
            }

            return returnStringList;
        }
        public string CleanInsideOfDoubleQuotesForAllTextInFile()
        {
            StringBuilder returnTextStringBuilder = new StringBuilder();

            const int startState = 0;
            const int foundStartingDoubleQuoteState = 1;

            string allTextInFile = File.ReadAllText(MyFullFilename);

            int myState = startState;

            Int32 currentCharacterPos = 0;
            string currentCharacter = string.Empty;
            while (currentCharacterPos < allTextInFile.Length)
            {
                if (currentCharacterPos == 876)
                {
                    int i = 0;
                    i++;
                }
                currentCharacter = allTextInFile.Substring(currentCharacterPos, 1);
                switch (currentCharacter)
                {
                    case "\"":
                        switch (myState)
                        {
                            case startState:
                                myState = foundStartingDoubleQuoteState;
                                break;
                            case foundStartingDoubleQuoteState:
                                myState = startState;
                                break;
                        }
                        break;
                    case ",":
                        switch (myState)
                        {
                            case startState:
                                returnTextStringBuilder.Append(",");
                                break;
                            case foundStartingDoubleQuoteState:
                                returnTextStringBuilder.Append(" ");
                                break;
                        }
                        break;
                    case "'":
                        switch (myState)
                        {
                            case startState:
                                returnTextStringBuilder.Append("'");
                                break;
                            case foundStartingDoubleQuoteState:
                                returnTextStringBuilder.Append("~");
                                break;
                        }
                        break;
                    case "\n":
                        switch (myState)
                        {
                            case startState:
                                returnTextStringBuilder.Append("\n");
                                break;
                            case foundStartingDoubleQuoteState:
                                returnTextStringBuilder.Append(" ");
                                break;
                        }
                        break;
                    default:
                        returnTextStringBuilder.Append(currentCharacter);
                        break;
                }
                currentCharacterPos++;
            }

            return returnTextStringBuilder.ToString();
        }
    }
}
