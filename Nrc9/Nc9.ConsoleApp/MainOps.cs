using BulkInsert9.CallWebApiLand;
using BulkInsert9.Data.Models;
using EmailWebApiLand9.Data.Models;
using log4net;
using Nrc9.Data.Models;
using Spire.Xls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;


namespace Nrc9.ConsoleApp
{
    public class MainOps
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MainOps));
        public MainOps
                (
                    qy_GetNrcConfigOutputColumns inputConfigOps
                    , List<string> inputFullFilenameList
                )
        {
            MyConfigOptions = inputConfigOps;
            MyFullFilenameList = inputFullFilenameList;
        }
        public qy_GetNrcConfigOutputColumns MyConfigOptions { get; set; }
        public string MyFullArchiveFilename { get; set; }
        public string MyFullArchiveSavedFilename { get; set; }

        public List<string> MyFullFilenameList { get; set; }

        public MainOpsOutput MyMain()
        {
            MainOpsOutput returnOutput = new MainOpsOutput();

            foreach (string loopFullFilename in MyFullFilenameList)
            {
                string[] 
                    myFileLineArray =
                        File.ReadAllLines(loopFullFilename);

                if (myFileLineArray.Length == 0)
                {
                    FileInfo fiEmptyFile =
                        new FileInfo(loopFullFilename);
                    string
                        archivedInputFilename =
                            Path.Combine
                            (
                                MyConfigOptions.InputArchiveDirectory
                                , fiEmptyFile.Name
                            );
                    if (File.Exists(archivedInputFilename))
                    {
                        File.Delete(archivedInputFilename);
                    }
                    File.Copy(loopFullFilename, archivedInputFilename);
                    if (File.Exists(loopFullFilename))
                    {
                        File.Delete(loopFullFilename);
                    }
                        
                    continue;
                }

                ProcessSingleFileOutput
                    myProcessSingleFileOutput =
                        ProcessSingleFile(loopFullFilename);
                

                if (myProcessSingleFileOutput.EmailBodyLineList.Count > 0)
                {
                    returnOutput.MailBodyLineList.AddRange(myProcessSingleFileOutput.EmailBodyLineList);
                }
            }

            return returnOutput;
        }
        public ProcessSingleFileOutput ProcessSingleFile(string inputFilename)
        {
            ProcessSingleFileOutput returnOutput = new ProcessSingleFileOutput();

            List<string> outputStringList = new List<string>();

            string newInputFilename = inputFilename;
            FileInfo inputFi = new FileInfo(newInputFilename);
            if (!newInputFilename.Contains("20"))
            {
                string filenameSansExtension =
                    inputFi.Name.Replace(inputFi.Extension, "");
                DateTime fileCreationTime =
                    inputFi.CreationTime;
                string MM = fileCreationTime.Month.ToString().PadLeft(2, '0');
                string dd = fileCreationTime.Day.ToString().PadLeft(2, '0');
                string yyyy = fileCreationTime.Year.ToString();
                newInputFilename =
                    Path.Combine(inputFi.DirectoryName, $"{filenameSansExtension}_{yyyy}{MM}{dd}.csv");
                if (File.Exists(newInputFilename))
                {
                    File.Delete(newInputFilename);
                }
                File.Copy(inputFilename, newInputFilename, true);
                if (File.Exists(newInputFilename))
                {
                    File.Delete(inputFilename);
                }
            }

            // Extract Excel data
            ExtractCsvDataFromSingleFileSingleFileOutput
                myExtractExcelDataFromSingleFileSingleFileOutput =
                    ExtractCsvDataFromSingleFileSingleFile(newInputFilename);
            if (!myExtractExcelDataFromSingleFileSingleFileOutput.IsOk)
            {
                returnOutput.IsOk = false;
                returnOutput.ErrorMessage = myExtractExcelDataFromSingleFileSingleFileOutput.ErrorMessage;
                return returnOutput;
            }

            // If we have data to import...
            if (myExtractExcelDataFromSingleFileSingleFileOutput.MyCsvLineList.Count > 0)
            {
                // Truncate Raw table.
                dd_InputFileOutput
                    mydd_InputFileOutput =
                        InputFile();
                if (!mydd_InputFileOutput.IsOk)
                {
                    returnOutput.IsOk = false;
                    returnOutput.ErrorMessage =
                        mydd_InputFileOutput.ErrorMessage;
                    return returnOutput;
                }

                // Bulk Insert 
                BulkInsertOutput
                    myBulkInsertPhysicianLabAndBiometricsDataOutput =
                        BulkInsert
                        (
                            newInputFilename
                            , myExtractExcelDataFromSingleFileSingleFileOutput.MyCsvLineList
                        );

                if (!myBulkInsertPhysicianLabAndBiometricsDataOutput.IsOk)
                {
                    returnOutput.IsOk = false;
                    returnOutput.ErrorMessage =
                        myBulkInsertPhysicianLabAndBiometricsDataOutput.ErrorMessage;
                    return returnOutput;
                }

                // Create Archival Output filename.
                BuildFullOutputFilename(newInputFilename);

                // Finalize CollectionsRaw table.
                di_FinalizeInputFileOutput
                mydi_FinalizeInputFileOutput =
                        FinalizeInputFile
                        (
                            MyConfigOptions.BaseWebApiUrl
                       );
                if (!mydi_FinalizeInputFileOutput.IsOk)
                {
                    returnOutput.IsOk = false;
                    returnOutput.ErrorMessage =
                        mydi_FinalizeInputFileOutput.ErrorMessage;
                    return returnOutput;
                }

                // Get Output File Lines
                qy_GetOutputFileLinesOutput
                    myqy_GetOutputFileLinesOutput =
                        GetOutputFileLines();
                if (!myqy_GetOutputFileLinesOutput.IsOk)
                {
                    returnOutput.IsOk = false;
                    returnOutput.ErrorMessage =
                        myqy_GetOutputFileLinesOutput.ErrorMessage;
                    return returnOutput;
                }

                // Convert output object list to Output Line List.
                outputStringList =
                    myqy_GetOutputFileLinesOutput
                    .qy_GetOutputFileLinesOutputColumnsList
                    .Select(l => l.OutputLine)
                    .ToList();

                // pwm 11/6/2025 - I'm not sure why the header line was not included in one out of hundreds of files,
                // but just in case it happens again, I insert the header line.
                if (!outputStringList[0].StartsWith("PatientNameGiven,"))
                {
                    outputStringList.Insert(0, "PatientNameGiven,PatientNameFamily,AddressStreet1,AddressCity,AddressState,AddressPostalCode,PhoneAreaCityCode,PhoneLocalNumber,MRN,DateOfBirth,AdministrativeSex,PrimaryLanguage,Race,EthnicGroup,MaritalStatus,Email,PatientClass,FacilityName,FacilityNumber,VisitNumber,AdmitDateTime,DischargeDateTime,AdmitSource,DischargeStatus,LocationCriteria,Location,MSDRG,DiagnosisPrimaryICD10,Diagnosis2ICD10,Diagnosis3ICD10,IsDeceased,ICU,EDAdmit,PrimaryPayerID,PrimaryPayerName,AttendingDoctorNameGiven,AttendingDoctorNameSecondGiven,AttendingDoctorNameFamily,AttendingDoctorNameSuffix,AttendingDoctorDegree,AttendingDoctorNPI,AttendingDoctorSpecialty,ProcedurePrimaryCPT,Procedure2CPT,Procedure3CPT,HNumIPDisch,PreferredOutreachMode,CGCAHPS");
                }
            }

            // If input file has just the header, create empty output file.
            else
            {
                // Create Archival Output filename.
                outputStringList =
                    BuildFileWithHeaderLineOnly(newInputFilename);

            }

            // If Archive Output file already exists, delete it.
            if (File.Exists(MyFullArchiveFilename))
            {
                File.Delete(MyFullArchiveFilename);
            }

            // Save Output File.
            System.IO.File.WriteAllLines
            (
                MyFullArchiveFilename
                , outputStringList
            );

            // If Archive Output file already exists, delete it.
            if (File.Exists(MyFullArchiveSavedFilename))
            {
                File.Delete(MyFullArchiveSavedFilename);
            }

            // Save Output File to Saved Archive.
            System.IO.File.WriteAllLines
            (
                MyFullArchiveSavedFilename
                , outputStringList
            );


            // Archive input file
            FileInfo myFi = new FileInfo(newInputFilename);
            string fullArchiveFilename =
                Path.Combine(MyConfigOptions.InputArchiveDirectory, myFi.Name);
            if (File.Exists(fullArchiveFilename))
            {
                File.Delete(fullArchiveFilename);
            }
            File.Copy(newInputFilename, fullArchiveFilename, true);

            if (File.Exists(newInputFilename))
            {
                File.Delete(newInputFilename);
            }

            // Email lines.
            string myEmailBodyLine =
                $"File {inputFilename} was successfully imported as an NRC File.".Replace("\"", " ").Replace("\\", " ");
            returnOutput.EmailBodyLineList.Add(myEmailBodyLine);

            return returnOutput;
        }

        public List<string> BuildFileWithHeaderLineOnly(string inputFilename)
        {
            List<string> returnOutput = new List<string>();

            // Create Archival Output filename.
            BuildFullOutputFilename(inputFilename);

            // If filename contains "SLEEP_"
            if (inputFilename.ToUpper().Contains("SLEEP_"))
            {
                returnOutput.Add
                (
                    "PatientNameGiven,PatientNameFamily,AddressStreet1,AddressCity,AddressState,AddressPostalCode,PhoneAreaCityCode,PhoneLocalNumber,MRN,DateOfBirth,AdministrativeSex,PrimaryLanguage,Race,EthnicGroup,MaritalStatus,Email,PatientClass,FacilityName,FacilityNumber,VisitNumber,AdmitDateTime,DischargeDateTime,AdmitSource,DischargeStatus,LocationCriteria,Location,MSDRG,DiagnosisPrimaryICD10,Diagnosis2ICD10,Diagnosis3ICD10,IsDeceased,ICU,EDAdmit,PrimaryPayerID,PrimaryPayerName,AttendingDoctorNameGiven,AttendingDoctorNameSecondGiven,AttendingDoctorNameFamily,AttendingDoctorNameSuffix,AttendingDoctorDegree,AttendingDoctorNPI,AttendingDoctorSpecialty,ProcedurePrimaryCPT,Procedure2CPT,Procedure3CPT,HNumIPDisch,PreferredOutreachMode"
                );
            }

            else if (inputFilename.ToUpper().Contains("SEC_"))
            {
                returnOutput.Add
                (
                    "PatientNameGiven,PatientNameFamily,AddressStreet1,AddressCity,AddressState,AddressPostalCode,PhoneAreaCityCode,PhoneLocalNumber,MRN,DateOfBirth,AdministrativeSex,PrimaryLanguage,Race,EthnicGroup,MaritalStatus,Email,PatientClass,FacilityName,FacilityNumber,VisitNumber,AdmitDateTime,DischargeDateTime,AdmitSource,DischargeStatus,LocationCriteria,Location,MSDRG,DiagnosisPrimaryICD10,Diagnosis2ICD10,Diagnosis3ICD10,IsDeceased,ICU,EDAdmit,PrimaryPayerID,PrimaryPayerName,AttendingDoctorNameGiven,AttendingDoctorNameSecondGiven,AttendingDoctorNameFamily,AttendingDoctorNameSuffix,AttendingDoctorDegree,AttendingDoctorNPI,AttendingDoctorSpecialty,ProcedurePrimaryCPT,Procedure2CPT,Procedure3CPT,HNumIPDisch,PreferredOutreachMode"
                );
            }
            else if (inputFilename.ToUpper().Contains("NRC ARRIVED APPOINTMENTS_"))
            {
                returnOutput.Add
                (
                    "PatientNameGiven,PatientNameFamily,AddressStreet1,AddressCity,AddressState,AddressPostalCode,PhoneAreaCityCode,PhoneLocalNumber,MRN,DateOfBirth,AdministrativeSex,PrimaryLanguage,Race,EthnicGroup,MaritalStatus,Email,PatientClass,FacilityName,FacilityNumber,VisitNumber,AdmitDateTime,DischargeDateTime,AdmitSource,DischargeStatus,LocationCriteria,Location,MSDRG,DiagnosisPrimaryICD10,Diagnosis2ICD10,Diagnosis3ICD10,IsDeceased,ICU,EDAdmit,PrimaryPayerID,PrimaryPayerName,AttendingDoctorNameGiven,AttendingDoctorNameSecondGiven,AttendingDoctorNameFamily,AttendingDoctorNameSuffix,AttendingDoctorDegree,AttendingDoctorNPI,AttendingDoctorSpecialty,ProcedurePrimaryCPT,Procedure2CPT,Procedure3CPT,HNumIPDisch,PreferredOutreachMode,CGCAHPS"
                );
            }

            return returnOutput;
        }
        public void BuildFullOutputFilename(string inputFullFilename)
        {
            FileInfo myInputFi = new FileInfo(inputFullFilename);

            string[] filenameParts =
                myInputFi.Name.Split('.');
            string filenameSansExtension =
                filenameParts[0];
            string[] filenameSansExtensionParts =
                filenameSansExtension.Split('_');
            string myDateString =
                filenameSansExtensionParts[1];

            string namePrefix = string.Empty;
            if (inputFullFilename.Contains("SEC"))
            {
                namePrefix = "SummitMedicalGroup_ExpressCare_";
            }
            else if (inputFullFilename.Contains("Sleep"))
            {
                namePrefix = "SummitMedicalGroup_Sleep_";
            }
            else
            {
                namePrefix = "SummitMedicalGroup_Clinics_";
            }

            MyFullArchiveFilename =
                Path.Combine(MyConfigOptions.OutputArchiveDirectory, $"{namePrefix}{myDateString}.csv");

            MyFullArchiveSavedFilename =
                Path.Combine($"{MyConfigOptions.OutputArchiveDirectory}Saved", $"{namePrefix}{myDateString}.csv");
        }

        public dd_InputFileOutput InputFile()
        {
            dd_InputFileOutput returnOutput =
                new dd_InputFileOutput();
            Nrc9
            .CallWebApiLand
            .CallWebApiLandClass
                myCallWebApiLandClass =
                    new
                        Nrc9
                        .CallWebApiLand
                        .CallWebApiLandClass
                        (
                            this.MyConfigOptions.BaseWebApiUrl
                        );
            returnOutput =
                myCallWebApiLandClass
                .dd_InputFile();
            if (!returnOutput.IsOk)
            {
                returnOutput.IsOk = false;
                returnOutput.ErrorMessage =
                    returnOutput.ErrorMessage;
                return returnOutput;
            }
            return returnOutput;
        }
        public BulkInsertOutput BulkInsert(string inputFilename, List<string> inputCsvLineList)
        {
            BulkInsertOutput returnOutput =
                new BulkInsertOutput();

            // Make sure the Pk column is in the non-CSV column list.
            List<NonCsvFileColumnDefAndValue>
                myNonCsvFileColumnDefAndValueList =
                    new List<NonCsvFileColumnDefAndValue>();

            // 999 - [FullFilename] [nvarchar](max) NULL
            spGetColumnDefsForGivenDbAndTableName_OutputColumns myPkColDef =
                new spGetColumnDefsForGivenDbAndTableName_OutputColumns
                {
                    MyDbColumnName = "FullFilename",
                    MyDbColumnLength = 300,
                    MyDbColumnType = "nvarchar",
                    MyDbName = "Staging",
                    MyDbTableName = "nrc.InputFile",
                    MyFilePosition = 999
                };
            NonCsvFileColumnDefAndValue myPkNonCsvFileColumnDefAndValue =
                new NonCsvFileColumnDefAndValue
                {
                    MyColDef = myPkColDef,
                    MyValueString = inputFilename
                };
            myNonCsvFileColumnDefAndValueList.Add(myPkNonCsvFileColumnDefAndValue);


            // 1000 - [EmployeeLifeEventsRawID] [int] NOT NULL
            myPkColDef =
                new spGetColumnDefsForGivenDbAndTableName_OutputColumns
                {
                    MyDbColumnName = "InputFileID",
                    MyDbColumnLength = 1,
                    MyDbColumnType = "int",
                    MyDbName = "Staging",
                    MyDbTableName = "nrc.InputFile",
                    MyFilePosition = 1000
                };
            myPkNonCsvFileColumnDefAndValue =
                new NonCsvFileColumnDefAndValue
                {
                    MyColDef = myPkColDef,
                    MyValueString = "0"
                };
            myNonCsvFileColumnDefAndValueList.Add(myPkNonCsvFileColumnDefAndValue);

            // Call Web API Endpoint to perform the Bulk Insert of Tcareb_Patient_Appt_Raw
            CallBulkInsertWebApiLand
                myCallBulkInsert =
                new CallBulkInsertWebApiLand
                (
                    MyConfigOptions.BulkInsertConnectionString //string inputDbConnectionString
                   , MyConfigOptions.BulkInsertDbName // string inputDbName
                   , MyConfigOptions.BulkInsertDbTableName // string inputDbTableName
                   , inputCsvLineList // List<string> inputCsvLineList
                   , myNonCsvFileColumnDefAndValueList // List<NonCsvFileColumnDefAndValue> inputNonCsvFileColumnDefAndValueList
                   , MyConfigOptions.BulkInsertBaseWebApiUrl // string inputBulkInsertWebApiBaseUrl
                );

            BulkInsertOutput myBulkInsertOutput =
                myCallBulkInsert.CallIt();

            if (!myBulkInsertOutput.IsOk)
            {
                returnOutput.IsOk = false;
                returnOutput.ErrorMessage = myBulkInsertOutput.ErrorMessage;
                return returnOutput;
            }
            return returnOutput;
        }
        public di_FinalizeInputFileOutput
                    FinalizeInputFile
                    (
                        string inputProcessorTennCareLogOnId
                    )
        {
            di_FinalizeInputFileOutput returnOutput =
                new di_FinalizeInputFileOutput();
            Nrc9
            .CallWebApiLand
            .CallWebApiLandClass
                myCallWebApiLandClass =
                    new
                        Nrc9
                        .CallWebApiLand
                        .CallWebApiLandClass
                        (
                            this.MyConfigOptions.BaseWebApiUrl
                        );
            returnOutput =
                myCallWebApiLandClass
                .di_FinalizeInputFile(MyFullArchiveFilename);

            if (!returnOutput.IsOk)
            {
                returnOutput.IsOk = false;
                returnOutput.ErrorMessage =
                    returnOutput.ErrorMessage;
                return returnOutput;
            }
            return returnOutput;
        }
        public ExtractCsvDataFromSingleFileSingleFileOutput ExtractCsvDataFromSingleFileSingleFile(string inputFullFilename)
        {
            ExtractCsvDataFromSingleFileSingleFileOutput returnOutput = new ExtractCsvDataFromSingleFileSingleFileOutput();

            log.Info("Clean insid of double-quotes.");
            CleanInideOfDoubleQuotesInEntireFileReturningFileLineList
                myCleanInideOfDoubleQuotesInEntireFileReturningFileLineList =
                    new CleanInideOfDoubleQuotesInEntireFileReturningFileLineList(inputFullFilename);

            List<string> listOfCleanedStrings =
                myCleanInideOfDoubleQuotesInEntireFileReturningFileLineList.DoIt();

            for (int deleteLineCtr =  0; deleteLineCtr < MyConfigOptions.LineWhereDataStarts - 1; deleteLineCtr++)
            {
                listOfCleanedStrings.RemoveAt(deleteLineCtr);
            }

            List<spGetColumnDefsForGivenDbAndTableName_OutputColumns>
                myColDefs =
                    GetColumnDefsForGivenDbAndTableName
                    (
                        MyConfigOptions.BulkInsertDbName
                        , MyConfigOptions.BulkInsertDbTableName
                        , MyConfigOptions.BulkInsertBaseWebApiUrl
                    );

            // The column definitions include columns not actually in the file (like the full filename).  So filter to the fields actually in the file.
            List<spGetColumnDefsForGivenDbAndTableName_OutputColumns>
                myActuallyInFileColDefs =
                    myColDefs.Where(c => c.MyFilePosition < 900).ToList();


            MassageDataProducingBulkInsertCsvLineList
                myMassageDataProducingBulkInsertCsvLineList =
                    new MassageDataProducingBulkInsertCsvLineList
                        (
                            listOfCleanedStrings
                            , myActuallyInFileColDefs
                        );
            returnOutput.MyCsvLineList =
                myMassageDataProducingBulkInsertCsvLineList.DoIt();
            return returnOutput;
        }

        public List<spGetColumnDefsForGivenDbAndTableName_OutputColumns>
                GetColumnDefsForGivenDbAndTableName
                (
                    string inputDbNameForGettingColDefs
                    , string inputDbTableNameForGettingColDefs
                    , string inputBulkInsertWebApiBaseUrl
                )
        {
            List<spGetColumnDefsForGivenDbAndTableName_OutputColumns>
                returnOutput =
                    new List<spGetColumnDefsForGivenDbAndTableName_OutputColumns>();

            CallBulkInsertWebApiLand
                myCallBulkInsertWebApiLand =
                    new CallBulkInsertWebApiLand
                        (
                            inputDbNameForGettingColDefs
                            , inputDbTableNameForGettingColDefs
                            , inputBulkInsertWebApiBaseUrl
                        );
            spGetColumnDefsForGivenDbAndTableNameOutput
                myspGetColumnDefsForGivenDbAndTableNameOutput =
                    myCallBulkInsertWebApiLand.CallSpGetColumnDefsForGivenDbAndTableName();
            if (!myspGetColumnDefsForGivenDbAndTableNameOutput.IsOk)
            {
                return returnOutput;
            }
            returnOutput = myspGetColumnDefsForGivenDbAndTableNameOutput.ColumnDefList;

            return returnOutput;

        }

        public qy_GetOutputFileLinesOutput 
                GetOutputFileLines()
        {
            qy_GetOutputFileLinesOutput returnOutput =
                new qy_GetOutputFileLinesOutput();
            Nrc9
            .CallWebApiLand
            .CallWebApiLandClass
                myCallWebApiLandClass =
                    new
                        Nrc9
                        .CallWebApiLand
                        .CallWebApiLandClass
                        (
                            this.MyConfigOptions.BaseWebApiUrl
                        );
            returnOutput =
                myCallWebApiLandClass
                .qy_GetOutputFileLines();

            if (!returnOutput.IsOk)
            {
                returnOutput.IsOk = false;
                returnOutput.ErrorMessage =
                    returnOutput.ErrorMessage;
                return returnOutput;
            }
            return returnOutput;

        }
    }
}


