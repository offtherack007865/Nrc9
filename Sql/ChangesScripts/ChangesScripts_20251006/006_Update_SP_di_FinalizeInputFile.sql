-- SQL Server Instance:  smg-sql01
USE [Utilities];
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('nrc.di_FinalizeInputFile'))
   DROP PROC [nrc].[di_FinalizeInputFile]
GO

BEGIN TRANSACTION;

CREATE PROCEDURE [nrc].[di_FinalizeInputFile]
(
  @inputOutputFullFilename [nvarchar] (300)
)

/* -----------------------------------------------------------------------------------------------------------
   Procedure Name  :  nrc.di_FinalizeInputFile
   Business Analyis:
   Project/Process :   
   Description     :  Finalize Athena InputFile table.
	  
   Author          :  Philip Morrison 
   Create Date     :  6/23/2025

   ***********************************************************************************************************
   **         Change History                                                                                **
   ***********************************************************************************************************

   Date       Version    Author             Description
   --------   --------   -----------        ------------
   6/23/2025  1.01.001   Philip Morrison    Created
   6/26/2025  1.01.002   Philip Morrison    Changed Database name and Table Name to meet DBA standards.       
   7/2/2025   1.01.003   Philip Morrison    Adjust to location change of the OutputFile Table to CustomerSurvey database.
   7/8/2025   1.01.004   Philip Morrison    Output the dept number instead of the dept name for Facility ID and for Location Criteria.
   7/21/2025  1.01.005   Philip Morrison    Delete from OutputFileHistory rows which are both in OutputFileHistory and OutputFile.
                                            (It was deleting the rows from OutputFile which is in error).            
   9/30/2025  1.01.005   Philip Morrison    Change the method to randomly mark 20% of the total "Clinical file" records as needing 
                                            an extra survey question.
*/ -----------------------------------------------------------------------------------------------------------                                   

AS
BEGIN

-- Instance Declarations.
DECLARE @IsOk [bit] = 0;
DECLARE @MyCount [int] = 0;
DECLARE @IndividualImportFilename [nvarchar] (3000) = '';
DECLARE @TwentyPercentOfMasterRecordCount [int] = 0;
DECLARE @RandomlyGeneratedRecordID [int] = 0;
DECLARE @ExistsRandomlyGeneratedRecordID [int] = 0;
DECLARE @RandomlyGeneratedRecordIDCountSoFar [int] = 0;

-- Template Declarations
DECLARE @Application            varchar(128) = 'Nrc' 
DECLARE @Version                varchar(25)  = '1.01.004'

DECLARE @ProcessID              int          = 0
DECLARE @Process                varchar(128) = 'ImportAndConvert'

DECLARE @BatchOutID             int
DECLARE @BatchDescription       varchar(1000) = @@ServerName + '  - ' + @Version
DECLARE @BatchDetailDescription varchar(1000)
DECLARE @BatchMessage           varchar(MAX)
DECLARE @User                   varchar(128) = SUSER_NAME()

DECLARE @AnticipatedRecordCount int 
DECLARE @ActualRecordCount      int

SET NOCOUNT ON

BEGIN TRY

--  Initialize Batch
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  NULL, 'BatchStart', @BatchDescription, @ProcessID, @Process
----------------------------------------------------------------------------------------------------------------------------------------------------

    SET @BatchDetailDescription = '010/110:  Truncate AthenaInputFileForThisNrcRunMaster'
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailStart', @BatchDetailDescription
	
	  SELECT @AnticipatedRecordCount = COUNT(*)
	                                   FROM [CUSTOMERSURVEY].[nrc].[InputFileMaster];
	  
    -- Truncate AthenaInputFileForThisNrcRunMaster
    TRUNCATE TABLE [CUSTOMERSURVEY].[nrc].[InputFileMaster];
	
    SET @ActualRecordCount = @@ROWCOUNT
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailEnd', NULL, NULL, NULL, @AnticipatedRecordCount, @ActualRecordCount
----------------------------------------------------------------------------------------------------------------------------------------------------

    SET @BatchDetailDescription = '020/110:  Copy InputFile to InputFileMaster'
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailStart', @BatchDetailDescription
	
	  SELECT @AnticipatedRecordCount = COUNT(*)
	                                   FROM [Staging].[nrc].[InputFile];
	  
    -- Copy AthenaInputFileForThisNrcRun to AthenaInputFileForThisNrcRunMaster
    INSERT INTO [CUSTOMERSURVEY].[nrc].[InputFileMaster]
	(
	  [PatientFirstName]
      ,[PatientLastName]
      ,[PatientEmail]
      ,[PatientHomePhone]
      ,[PatientMobileNo]
      ,[ApptDate]
      ,[PatientDob]
      ,[patientZip]
      ,[RndrngPrvdrFrstNme]
      ,[RndrngPrvdrLstNme]
      ,[RndrngPrvdrType]
      ,[RndrngPrvdrNpiNo]
      ,[PatientPrimaryInsHldrFi]
      ,[PatientPrimaryInsHldrLa]
      ,[GuarantorPhone]
      ,[GuarantorEmail]
      ,[SvcDeptId]
      ,[SvcDprtmnt]
      ,[PatientId]
      ,[PatientPrimaryInsPkgType]
      ,[PatientPrimaryInsPkgName]
      ,[ProcCode]
      ,[PatientSex]
      ,[Race]
      ,[Ethnicity]
      ,[PatientLang]
      ,[PatientAddress1]
      ,[PatientCity]
      ,[PatientState]
      ,[CurrDeptBillName]
      ,[CurrDeptNpiNo]
      ,[ApptId]
      ,[ApptCheckOutDate]
      ,[PtntDcsdYsn]
      ,[PatientMaritalStatus]
      ,[FullFileName]
	)
    SELECT
       [PatientFirstName]
       ,[PatientLastName]
       ,[PatientEmail]
       ,[PatientHomePhone]
       ,[PatientMobileNo]
       ,[ApptDate]
       ,[PatientDob]
       ,[patientZip]
       ,[RndrngPrvdrFrstNme]
       ,[RndrngPrvdrLstNme]
       ,[RndrngPrvdrType]
       ,[RndrngPrvdrNpiNo]
       ,[PatientPrimaryInsHldrFi]
       ,[PatientPrimaryInsHldrLa]
       ,[GuarantorPhone]
       ,[GuarantorEmail]
       ,[SvcDeptId]
       ,[SvcDprtmnt]
       ,[PatientId]
       ,[PatientPrimaryInsPkgType]
       ,[PatientPrimaryInsPkgName]
       ,[ProcCode]
       ,[PatientSex]
       ,[Race]
       ,[Ethnicity]
       ,[PatientLang]
       ,[PatientAddress1]
       ,[PatientCity]
       ,[PatientState]
       ,[CurrDeptBillName]
       ,[CurrDeptNpiNo]
       ,[ApptId]
       ,[ApptCheckOutDate]
       ,[PtntDcsdYsn]
       ,[PatientMaritalStatus]
       ,[FullFileName]	
	FROM [Staging].[nrc].[InputFile];
	
    SET @ActualRecordCount = @@ROWCOUNT
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailEnd', NULL, NULL, NULL, @AnticipatedRecordCount, @ActualRecordCount

----------------------------------------------------------------------------------------------------------------------------------------------------

    SET @BatchDetailDescription = '030/110:  Delete InputFileHistory rows that are also in InputFileMaster'
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailStart', @BatchDetailDescription
	
	SELECT @AnticipatedRecordCount = COUNT(*)
	FROM [CUSTOMERSURVEY].[nrc].[InputFileHistory] hist
	JOIN [CUSTOMERSURVEY].[nrc].[InputFileMaster] mstr
	ON
	  hist.[PatientFirstName] = mstr.[PatientFirstName]
      AND hist.[PatientLastName] = mstr.[PatientLastName]
      AND hist.[PatientEmail] = mstr.[PatientEmail]
      AND hist.[PatientHomePhone] = mstr.[PatientHomePhone]
      AND hist.[PatientMobileNo] = mstr.[PatientMobileNo]
      AND hist.[ApptDate] = mstr.[ApptDate]
      AND hist.[PatientDob] = mstr.[PatientDob]
      AND hist.[patientZip] = mstr.[patientZip]
      AND hist.[RndrngPrvdrFrstNme] = mstr.[RndrngPrvdrFrstNme]
      AND hist.[RndrngPrvdrLstNme] = mstr.[RndrngPrvdrLstNme]
      AND hist.[RndrngPrvdrType] = mstr.[RndrngPrvdrType]
      AND hist.[RndrngPrvdrNpiNo] = mstr.[RndrngPrvdrNpiNo]
      AND hist.[PatientPrimaryInsHldrFi] = mstr.[PatientPrimaryInsHldrFi]
      AND hist.[PatientPrimaryInsHldrLa] = mstr.[PatientPrimaryInsHldrLa]
      AND hist.[GuarantorPhone] = mstr.[GuarantorPhone]
      AND hist.[GuarantorEmail] = mstr.[GuarantorEmail]
      AND hist.[SvcDeptId] = mstr.[SvcDeptId]
      AND hist.[SvcDprtmnt] = mstr.[SvcDprtmnt]
      AND hist.[PatientId] = mstr.[PatientId]
      AND hist.[PatientPrimaryInsPkgType] = mstr.[PatientPrimaryInsPkgType]
      AND hist.[PatientPrimaryInsPkgName] = mstr.[PatientPrimaryInsPkgName]
      AND hist.[ProcCode] = mstr.[ProcCode]
      AND hist.[PatientSex] = mstr.[PatientSex]
      AND hist.[Race] = mstr.[Race]
      AND hist.[Ethnicity] = mstr.[Ethnicity]
      AND hist.[PatientLang] = mstr.[PatientLang]
      AND hist.[PatientAddress1] = mstr.[PatientAddress1]
      AND hist.[PatientCity] = mstr.[PatientCity]
      AND hist.[PatientState] = mstr.[PatientState]
      AND hist.[CurrDeptBillName] = mstr.[CurrDeptBillName]
      AND hist.[CurrDeptNpiNo] = mstr.[CurrDeptNpiNo]
      AND hist.[ApptId] = mstr.[ApptId]
      AND hist.[ApptCheckOutDate] = mstr.[ApptCheckOutDate]
      AND hist.[PtntDcsdYsn] = mstr.[PtntDcsdYsn]
      AND hist.[PatientMaritalStatus] = mstr.[PatientMaritalStatus]
      AND hist.[FullFileName] = mstr.[FullFileName];	  
	  
    -- Delete AthenaInputFileForThisNrcHistory rows that are also in AthenaInputFileForThisNrcMaster
    DELETE [CUSTOMERSURVEY].[nrc].[InputFileHistory]
	FROM [CUSTOMERSURVEY].[nrc].[InputFileHistory] hist
	JOIN [CUSTOMERSURVEY].[nrc].[InputFileMaster] mstr
	ON
	  hist.[PatientFirstName] = mstr.[PatientFirstName]
      AND hist.[PatientLastName] = mstr.[PatientLastName]
      AND hist.[PatientEmail] = mstr.[PatientEmail]
      AND hist.[PatientHomePhone] = mstr.[PatientHomePhone]
      AND hist.[PatientMobileNo] = mstr.[PatientMobileNo]
      AND hist.[ApptDate] = mstr.[ApptDate]
      AND hist.[PatientDob] = mstr.[PatientDob]
      AND hist.[patientZip] = mstr.[patientZip]
      AND hist.[RndrngPrvdrFrstNme] = mstr.[RndrngPrvdrFrstNme]
      AND hist.[RndrngPrvdrLstNme] = mstr.[RndrngPrvdrLstNme]
      AND hist.[RndrngPrvdrType] = mstr.[RndrngPrvdrType]
      AND hist.[RndrngPrvdrNpiNo] = mstr.[RndrngPrvdrNpiNo]
      AND hist.[PatientPrimaryInsHldrFi] = mstr.[PatientPrimaryInsHldrFi]
      AND hist.[PatientPrimaryInsHldrLa] = mstr.[PatientPrimaryInsHldrLa]
      AND hist.[GuarantorPhone] = mstr.[GuarantorPhone]
      AND hist.[GuarantorEmail] = mstr.[GuarantorEmail]
      AND hist.[SvcDeptId] = mstr.[SvcDeptId]
      AND hist.[SvcDprtmnt] = mstr.[SvcDprtmnt]
      AND hist.[PatientId] = mstr.[PatientId]
      AND hist.[PatientPrimaryInsPkgType] = mstr.[PatientPrimaryInsPkgType]
      AND hist.[PatientPrimaryInsPkgName] = mstr.[PatientPrimaryInsPkgName]
      AND hist.[ProcCode] = mstr.[ProcCode]
      AND hist.[PatientSex] = mstr.[PatientSex]
      AND hist.[Race] = mstr.[Race]
      AND hist.[Ethnicity] = mstr.[Ethnicity]
      AND hist.[PatientLang] = mstr.[PatientLang]
      AND hist.[PatientAddress1] = mstr.[PatientAddress1]
      AND hist.[PatientCity] = mstr.[PatientCity]
      AND hist.[PatientState] = mstr.[PatientState]
      AND hist.[CurrDeptBillName] = mstr.[CurrDeptBillName]
      AND hist.[CurrDeptNpiNo] = mstr.[CurrDeptNpiNo]
      AND hist.[ApptId] = mstr.[ApptId]
      AND hist.[ApptCheckOutDate] = mstr.[ApptCheckOutDate]
      AND hist.[PtntDcsdYsn] = mstr.[PtntDcsdYsn]
      AND hist.[PatientMaritalStatus] = mstr.[PatientMaritalStatus]
      AND hist.[FullFileName] = mstr.[FullFileName];
	
    SET @ActualRecordCount = @@ROWCOUNT
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailEnd', NULL, NULL, NULL, @AnticipatedRecordCount, @ActualRecordCount
----------------------------------------------------------------------------------------------------------------------------------------------------

    SET @BatchDetailDescription = '040/110:  Copy InputFileMaster rows to InputFileHistory'
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailStart', @BatchDetailDescription
	
	  SELECT @AnticipatedRecordCount = COUNT(*)
	                                   FROM [CUSTOMERSURVEY].[nrc].[InputFileMaster];
	  
    -- Copy InputFileMaster rows to InputFileHistory
    INSERT INTO [CUSTOMERSURVEY].[nrc].[InputFileHistory]
	(
	  [PatientFirstName]
      ,[PatientLastName]
      ,[PatientEmail]
      ,[PatientHomePhone]
      ,[PatientMobileNo]
      ,[ApptDate]
      ,[PatientDob]
      ,[patientZip]
      ,[RndrngPrvdrFrstNme]
      ,[RndrngPrvdrLstNme]
      ,[RndrngPrvdrType]
      ,[RndrngPrvdrNpiNo]
      ,[PatientPrimaryInsHldrFi]
      ,[PatientPrimaryInsHldrLa]
      ,[GuarantorPhone]
      ,[GuarantorEmail]
      ,[SvcDeptId]
      ,[SvcDprtmnt]
      ,[PatientId]
      ,[PatientPrimaryInsPkgType]
      ,[PatientPrimaryInsPkgName]
      ,[ProcCode]
      ,[PatientSex]
      ,[Race]
      ,[Ethnicity]
      ,[PatientLang]
      ,[PatientAddress1]
      ,[PatientCity]
      ,[PatientState]
      ,[CurrDeptBillName]
      ,[CurrDeptNpiNo]
      ,[ApptId]
      ,[ApptCheckOutDate]
      ,[PtntDcsdYsn]
      ,[PatientMaritalStatus]
      ,[FullFileName]
	)
    SELECT
       [PatientFirstName]
       ,[PatientLastName]
       ,[PatientEmail]
       ,[PatientHomePhone]
       ,[PatientMobileNo]
       ,[ApptDate]
       ,[PatientDob]
       ,[patientZip]
       ,[RndrngPrvdrFrstNme]
       ,[RndrngPrvdrLstNme]
       ,[RndrngPrvdrType]
       ,[RndrngPrvdrNpiNo]
       ,[PatientPrimaryInsHldrFi]
       ,[PatientPrimaryInsHldrLa]
       ,[GuarantorPhone]
       ,[GuarantorEmail]
       ,[SvcDeptId]
       ,[SvcDprtmnt]
       ,[PatientId]
       ,[PatientPrimaryInsPkgType]
       ,[PatientPrimaryInsPkgName]
       ,[ProcCode]
       ,[PatientSex]
       ,[Race]
       ,[Ethnicity]
       ,[PatientLang]
       ,[PatientAddress1]
       ,[PatientCity]
       ,[PatientState]
       ,[CurrDeptBillName]
       ,[CurrDeptNpiNo]
       ,[ApptId]
       ,[ApptCheckOutDate]
       ,[PtntDcsdYsn]
       ,[PatientMaritalStatus]
       ,[FullFileName]	
	FROM [CUSTOMERSURVEY].[nrc].[InputFileMaster];
	
    SET @ActualRecordCount = @@ROWCOUNT
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailEnd', NULL, NULL, NULL, @AnticipatedRecordCount, @ActualRecordCount
	
----------------------------------------------------------------------------------------------------------------------------------------------------

    SET @BatchDetailDescription = '050/110:  Truncate Table OutputFile'
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailStart', @BatchDetailDescription
	
	  SELECT @AnticipatedRecordCount = COUNT(*)
	                                   FROM [CUSTOMERSURVEY].[nrc].[OutputFile];

      -- Truncate Table OutputFileDataForThisNcrRun									   
	  TRUNCATE TABLE [CUSTOMERSURVEY].[nrc].[OutputFile];
	  
	
    SET @ActualRecordCount = @@ROWCOUNT
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailEnd', NULL, NULL, NULL, @AnticipatedRecordCount, @ActualRecordCount
----------------------------------------------------------------------------------------------------------------------------------------------------

    SET @BatchDetailDescription = '060/110:  Insert into OutputFile based on input Athena values.'
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailStart', @BatchDetailDescription
	
	  SELECT @AnticipatedRecordCount = COUNT(*)
	                                   FROM
                                       (SELECT maxMasterID.[ApptId]
                                               ,MAX(maxMasterID.[InputFileMasterID]) AS [InputFileMasterID]
                                        FROM
                                        (
                                           SELECT [ApptId]
                                           FROM [CUSTOMERSURVEY].[nrc].[InputFileMaster]
                                           WHERE [ApptId] IS NOT NULL
                                           GROUP BY [ApptId]
                                        ) unqAppt
                                        JOIN [CUSTOMERSURVEY].[nrc].[InputFileMaster] maxMasterID
                                        ON maxMasterID.[ApptId] = unqAppt.[ApptId]
                                        GROUP BY maxMasterID.[ApptId]   
                                       ) finalUnq           
                                       JOIN [CUSTOMERSURVEY].[nrc].[InputFileMaster] allInputs
                                       ON allInputs.[InputFileMasterID] = finalUnq.[InputFileMasterID];

      -- Insert into Insert into OutputFile based on input Athena values.									   
	  INSERT INTO [CUSTOMERSURVEY].[nrc].[OutputFile]
      (
	     [PatientNameGiven]
         ,[PatientNameFamily]
         ,[AddressStreet1]
         ,[AddressCity]
         ,[AddressState]
         ,[AddressPostalCode]
         ,[PhoneAreaCityCode]
         ,[PhoneLocalNumber]
         ,[MRN]
         ,[DateOfBirth]
         ,[AdministrativeSex]
         ,[PrimaryLanguage]
         ,[Race]
         ,[EthnicGroup]
         ,[MaritalStatus]
         ,[Email]
         ,[PatientClass]
         ,[FacilityName]
         ,[FacilityNumber]
         ,[VisitNumber]
         ,[AdmitDateTime]
         ,[DischargeDateTime]
         ,[AdmitSource]
         ,[DischargeStatus]
         ,[LocationCriteria]
         ,[Location]
         ,[MSDRG]
         ,[DiagnosisPrimaryICD10]
         ,[Diagnosis2ICD10]
         ,[Diagnosis3ICD10]
         ,[IsDeceased]
         ,[ICU]
         ,[EDAdmit]
         ,[PrimaryPayerID]
         ,[PrimaryPayerName]
         ,[AttendingDoctorNameGiven]
         ,[AttendingDoctorNameSecondGiven]
         ,[AttendingDoctorNameFamily]
         ,[AttendingDoctorNameSuffix]
         ,[AttendingDoctorDegree]
         ,[AttendingDoctorNPI]
         ,[AttendingDoctorSpecialty]
         ,[ProcedurePrimaryCPT]
         ,[Procedure2CPT]
         ,[Procedure3CPT]
         ,[HNumIPDisch]
         ,[PreferredOutreachMode]
         ,[OutputFullFileName]
         ,[ImportFullFileName]
         ,[CGCAHPS]
	  )
      SELECT 
	  	 ISNULL(allInputs.[PatientFirstName],'') AS [PatientNameGiven]
         ,ISNULL(allInputs.[PatientLastName],'') AS [PatientNameFamily]
         ,ISNULL(allInputs.[PatientAddress1],'') AS [AddressStreet1]
         ,ISNULL(allInputs.[PatientCity],'') AS [AddressCity]
         ,ISNULL(allInputs.[PatientState], '') AS [AddressState]
         ,CASE WHEN allInputs.[patientZip] IS NULL THEN ''
		       WHEN LEN(allInputs.[patientZip]) >= 5 THEN SUBSTRING(allInputs.[patientZip], 1, 5)
			   ELSE ''
	      END AS [AddressPostalCode] 		   
		 ,CASE WHEN allInputs.[PatientHomePhone] IS NULL THEN ''
		       WHEN CHARINDEX('(', allInputs.[PatientHomePhone]) = 1 AND CHARINDEX(')', allInputs.[PatientHomePhone]) = 5 THEN SUBSTRING(allInputs.[PatientHomePhone], 2, 3)
			   ELSE ''
	      END AS [PhoneAreaCityCode] 		   
		 ,CASE WHEN allInputs.[PatientHomePhone] IS NULL THEN ''
		       WHEN CHARINDEX('(', allInputs.[PatientHomePhone]) = 1 AND CHARINDEX(')', allInputs.[PatientHomePhone]) = 5 THEN REPLACE(SUBSTRING(allInputs.[PatientHomePhone], 6, LEN(allInputs.[PatientHomePhone])),'-','')
			   ELSE ''
	      END AS [PhoneLocalNumber] 		   
         ,ISNULL(allInputs.[PatientId], '') AS [MRN]
         ,ISNULL(CONVERT(VARCHAR(10), allInputs.[PatientDob], 101), '') AS [DateOfBirth]
		 ,[Support].[nrc].[fn_GetNrcGenderValueGivenAthenaValue] (allInputs.[PatientSex]) AS [AdministrativeSex]
 		 ,[Support].[nrc].[fn_GetNrcLanguageValueGivenAthenaValue] (allInputs.[PatientLang]) AS [PrimaryLanguage]
 		 ,[Support].[nrc].[fn_GetNrcRaceValueGivenAthenaValue] (allInputs.[Race]) AS [Race]
 		 ,[Support].[nrc].[fn_GetNrcEthnicityValueGivenAthenaValue] (allInputs.[Ethnicity]) AS [EthnicGroup]
 		 ,[Support].[nrc].[fn_GetNrcMaritalStatusValueGivenAthenaValue] (allInputs.[PatientMaritalStatus]) AS [MaritalStatus]
         ,ISNULL(allInputs.[PatientEmail], '') AS [Email]
         ,'Outpatient' AS [PatientClass]
         ,ISNULL(allInputs.[SvcDprtmnt], '') AS [FacilityName]
         ,allInputs.[SvcDeptId] AS [FacilityNumber]
         ,ISNULL(allInputs.[ApptId], '') AS [VisitNumber]
         ,allInputs.[ApptDate] AS [AdmitDateTime]
         ,allInputs.[ApptDate] AS [DischargeDateTime]
         ,'' AS [AdmitSource]
         ,'' AS [DischargeStatus]
         ,allInputs.[SvcDeptId] AS [LocationCriteria]
         ,ISNULL(allInputs.[SvcDprtmnt], '') AS [Location]
         ,'' AS [MSDRG]
         ,'' AS [DiagnosisPrimaryICD10]
         ,'' AS [Diagnosis2ICD10]
         ,'' AS [Diagnosis3ICD10]
		 ,CASE WHEN allInputs.[PtntDcsdYsn] IS NULL OR LEN(LTRIM(RTRIM(allInputs.[PtntDcsdYsn]))) = 0 THEN 'N'
		       WHEN UPPER(LTRIM(RTRIM(allInputs.[PtntDcsdYsn]))) = 'Y' THEN 'Y'
			   ELSE 'N'
	      END AS [IsDeceased]
         ,'' AS [ICU]
         ,'' AS [EDAdmit]
         ,ISNULL(allInputs.[ProcCode], '') AS [PrimaryPayerID]
         ,CASE WHEN LEN(ISNULL(allInputs.[PatientPrimaryInsPkgName], '')) > 42 THEN SUBSTRING(allInputs.[PatientPrimaryInsPkgName], 1, 42)
               ELSE ISNULL(allInputs.[PatientPrimaryInsPkgName], '')
          END AS [PrimaryPayerName]
         ,ISNULL(allInputs.[RndrngPrvdrFrstNme], '') AS [AttendingDoctorNameGiven]
         ,'' AS [AttendingDoctorNameSecondGiven]
         ,ISNULL(allInputs.[RndrngPrvdrLstNme], '') AS [AttendingDoctorNameFamily]
         ,'' AS [AttendingDoctorNameSuffix]
         ,ISNULL(allInputs.[RndrngPrvdrType], '') AS [AttendingDoctorDegree]
         ,ISNULL(allInputs.[RndrngPrvdrNpiNo], '') AS [AttendingDoctorNPI]
         ,'' AS [AttendingDoctorSpecialty]
         ,'' AS [ProcedurePrimaryCPT]
         ,'' AS [Procedure2CPT]
         ,'' AS [Procedure3CPT]
         ,'' AS [HNumIPDisch]
         ,'' AS [PreferredOutreachMode]
         ,@inputOutputFullFilename AS [OutputFullFileName]
         ,allInputs.[FullFileName] AS [ImportFullFileName]
         ,'' AS [CGCAHPS]
    FROM
    (SELECT maxMasterID.[ApptId]
            ,MAX(maxMasterID.[InputFileMasterID]) AS [InputFileMasterID]
       FROM
       (
            SELECT [ApptId]
              FROM [CUSTOMERSURVEY].[nrc].[InputFileMaster]
             WHERE [ApptId] IS NOT NULL
             GROUP BY [ApptId]
      ) unqAppt
      JOIN [CUSTOMERSURVEY].[nrc].[InputFileMaster] maxMasterID
        ON maxMasterID.[ApptId] = unqAppt.[ApptId]
      GROUP BY maxMasterID.[ApptId]   
    ) finalUnq           
    JOIN [CUSTOMERSURVEY].[nrc].[InputFileMaster] allInputs
      ON allInputs.[InputFileMasterID] = finalUnq.[InputFileMasterID]
	
    SET @ActualRecordCount = @@ROWCOUNT
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailEnd', NULL, NULL, NULL, @AnticipatedRecordCount, @ActualRecordCount
----------------------------------------------------------------------------------------------------------------------------------------------------
    SET @BatchDetailDescription = '070/110:  Using the unique record count, randomly generate one fifth of the record count worth IDs of the records containing the extra question.'
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailStart', @BatchDetailDescription

    -- Table to hold randomly generated record IDs within the range 1 and the anticipated record count
    TRUNCATE TABLE [CustomerSurvey].[nrc].[ExtraSurveyQuestionRecordIDs];

    -- Get the Import filename from the InputFileMaster
    SELECT @IndividualImportFilename = MAX([FullFileName])
    FROM [CUSTOMERSURVEY].[nrc].[InputFileMaster];
        
    -- If the inputted import filename contains 'NRC Arrived Appointments_'
    -- randomly generate Record IDs between 1 and the anticipated record count.
    IF CHARINDEX('NRC Arrived Appointments_', @IndividualImportFilename) > 0 BEGIN

      -- Use the variable @AnticipatedRecordCount to temporarily hold the total number of Master records.
	  SELECT @AnticipatedRecordCount = COUNT(*)
      FROM [CUSTOMERSURVEY].[nrc].[OutputFile];
                                       
      -- Get the count of random record ids to generate.
      SET @TwentyPercentOfMasterRecordCount = FLOOR(@AnticipatedRecordCount * .2);

	  -- Reset @AnticipatedRecordCount to be 20% of the total master record count.
      SET @AnticipatedRecordCount = @TwentyPercentOfMasterRecordCount;
       
      -- Forever loop which is exited once we've randomly generated the Record IDs of 20% of the master record count
      -- to be marked as needing an extra survey question.
      WHILE (1 = 1) BEGIN
       
        -- If we have the requisite number of randomly generated numbers, get out of the forever loop.
        SELECT @RandomlyGeneratedRecordIDCountSoFar = COUNT(*)
        FROM [CustomerSurvey].[nrc].[ExtraSurveyQuestionRecordIDs];
        IF (@RandomlyGeneratedRecordIDCountSoFar >= @TwentyPercentOfMasterRecordCount) BEGIN
          BREAK;
        END 
         
        -- Randomly generate new Record ID
        SELECT @RandomlyGeneratedRecordID = FLOOR(@AnticipatedRecordCount * RAND());
         
        -- Is the randomly generated record ID is already in the table, generate another.
        SET @ExistsRandomlyGeneratedRecordID = NULL;
        SELECT @ExistsRandomlyGeneratedRecordID = [ExtraSurveyQuestionRecordID]
        FROM [CustomerSurvey].[nrc].[ExtraSurveyQuestionRecordIDs]
        WHERE [ExtraSurveyQuestionRecordID] = @RandomlyGeneratedRecordID;
          
        -- If the randomly generated number is NOT already in the table, insert it.
        IF (@ExistsRandomlyGeneratedRecordID IS NULL) BEGIN
          INSERT INTO [CustomerSurvey].[nrc].[ExtraSurveyQuestionRecordIDs]
          VALUES(@RandomlyGeneratedRecordID);
        END
      END
    END
    ELSE BEGIN
      SET @AnticipatedRecordCount = 0;
    END
  
    SET @ActualRecordCount = @@ROWCOUNT
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailEnd', NULL, NULL, NULL, @AnticipatedRecordCount, @ActualRecordCount
----------------------------------------------------------------------------------------------------------------------------------------------------
    SET @BatchDetailDescription = '080/110:  In the output file, mark the record IDs contained in the randomly generated batch as needing an extra survey question.'
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailStart', @BatchDetailDescription

    -- Get the Import filename from the InputFileMaster
    SELECT @IndividualImportFilename = MAX([FullFileName])
    FROM [CUSTOMERSURVEY].[nrc].[InputFileMaster];
        
    -- If the inputted import filename contains 'NRC Arrived Appointments_'
    -- randomly generate Record IDs between 1 and the anticipated record count.
    IF CHARINDEX('NRC Arrived Appointments_', @IndividualImportFilename) > 0 BEGIN

	  -- @AnticipatedRecordCount is 20% of the total record count.
      SELECT @AnticipatedRecordCount = COUNT(*)
      FROM [CustomerSurvey].[nrc].[ExtraSurveyQuestionRecordIDs];
   
      -- Mark records whose RecordId is in the randomly generated batch of record ids as needing an extra survey question.
      UPDATE [CUSTOMERSURVEY].[nrc].[OutputFile] 
      SET [CGCAHPS] = 'yes'
      FROM  [CUSTOMERSURVEY].[nrc].[OutputFile] myoutput
      JOIN [CustomerSurvey].[nrc].[ExtraSurveyQuestionRecordIDs] extrasur
      ON extrasur.[ExtraSurveyQuestionRecordID] = myoutput.[OutputFileID];
      
      -- Mark records whose RecordId is NOT in the randomly generated batch of record ids as NOT needing an extra survey question.
      UPDATE [CUSTOMERSURVEY].[nrc].[OutputFile] 
      SET [CGCAHPS] = 'no'
      WHERE [CGCAHPS] = '';
      
    END   
    ELSE BEGIN
      SET @AnticipatedRecordCount = 0;
    END
      
    SET @ActualRecordCount = @@ROWCOUNT
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailEnd', NULL, NULL, NULL, @AnticipatedRecordCount, @ActualRecordCount
----------------------------------------------------------------------------------------------------------------------------------------------------

    SET @BatchDetailDescription = '090/110:  Delete [CUSTOMERSURVEY].[nrc].[OutputFileHistory] rows that are also in [Staging].[nrc].[OutputFile]'
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailStart', @BatchDetailDescription
	
	SELECT @AnticipatedRecordCount = COUNT(*)
	FROM [CUSTOMERSURVEY].[nrc].[OutputFileHistory] hist
	JOIN [CUSTOMERSURVEY].[nrc].[OutputFile] mstr
	ON
      hist.[PatientNameGiven] = mstr.[PatientNameGiven]
	  AND hist.[PatientNameFamily] = mstr.[PatientNameFamily]
	  AND hist.[AddressStreet1] = mstr.[AddressStreet1]
      AND hist.[AddressCity] = mstr.[AddressCity]
      AND hist.[AddressState] = mstr.[AddressState]
	  AND hist.[AddressPostalCode] = mstr.[AddressPostalCode]
	  AND hist.[PhoneAreaCityCode] = mstr.[PhoneAreaCityCode]
	  AND hist.[PhoneLocalNumber] = mstr.[PhoneLocalNumber]
	  AND hist.[MRN] = mstr.[MRN]
	  AND hist.[DateOfBirth] = mstr.[DateOfBirth]
	  AND hist.[AdministrativeSex] = mstr.[AdministrativeSex]
	  AND hist.[PrimaryLanguage] = mstr.[PrimaryLanguage]
	  AND hist.[Race] = mstr.[Race]
      AND hist.[EthnicGroup] = mstr.[EthnicGroup]
	  AND hist.[MaritalStatus] = mstr.[MaritalStatus]
	  AND hist.[Email] = mstr.[Email]
	  AND hist.[PatientClass] = mstr.[PatientClass]
	  AND hist.[FacilityName] = mstr.[FacilityName]
	  AND hist.[FacilityNumber] = mstr.[FacilityNumber]
	  AND hist.[VisitNumber]  = mstr.[VisitNumber]
	  AND hist.[AdmitDateTime] = mstr.[AdmitDateTime]
	  AND hist.[DischargeDateTime] = mstr.[DischargeDateTime]
	  AND hist.[AdmitSource] = mstr.[AdmitSource]
      AND hist.[DischargeStatus] = mstr.[DischargeStatus]
	  AND hist.[LocationCriteria] = mstr.[LocationCriteria]
	  AND hist.[Location] = mstr.[Location]
	  AND hist.[MSDRG] = mstr.[MSDRG]
	  AND hist.[DiagnosisPrimaryICD10] = mstr.[DiagnosisPrimaryICD10]
	  AND hist.[Diagnosis2ICD10] = mstr.[Diagnosis2ICD10]
      AND hist.[Diagnosis3ICD10] = mstr.[Diagnosis3ICD10]
	  AND hist.[IsDeceased] = mstr.[IsDeceased]
	  AND hist.[ICU] = mstr.[ICU]
	  AND hist.[EDAdmit] = mstr.[EDAdmit]
      AND hist.[PrimaryPayerID] = mstr.[PrimaryPayerID]
	  AND hist.[PrimaryPayerName] = mstr.[PrimaryPayerName]
	  AND hist.[AttendingDoctorNameGiven] = mstr.[AttendingDoctorNameGiven]
	  AND hist.[AttendingDoctorNameSecondGiven] = mstr.[AttendingDoctorNameSecondGiven]
      AND hist.[AttendingDoctorNameFamily] = mstr.[AttendingDoctorNameFamily]
      AND hist.[AttendingDoctorNameSuffix] = mstr.[AttendingDoctorNameSuffix]
	  AND hist.[AttendingDoctorDegree] = mstr.[AttendingDoctorDegree]
      AND hist.[AttendingDoctorNPI] = mstr.[AttendingDoctorNPI]
	  AND hist.[AttendingDoctorSpecialty] = mstr.[AttendingDoctorSpecialty]
      AND hist.[ProcedurePrimaryCPT] = mstr.[ProcedurePrimaryCPT]
      AND hist.[Procedure2CPT] = mstr.[Procedure2CPT]
      AND hist.[Procedure3CPT] = mstr.[Procedure3CPT]
      AND hist.[HNumIPDisch] = mstr.[HNumIPDisch]
      AND hist.[PreferredOutreachMode] = mstr.[PreferredOutreachMode]
	  AND hist.[OutputFullFileName] = mstr.[OutputFullFileName]
	  AND hist.[ImportFullFileName] = mstr.[ImportFullFileName]
	  AND hist.[CGCAHPS] = mstr.[CGCAHPS];
	  
	  
    -- (This statement used to be deleting from OutputFile when it should be deleting from OutputFileHistory)
    -- Delete [CUSTOMERSURVEY].[nrc].[OutputFileHistory] rows that are also in [CUSTOMERSURVEY].[nrc].[OutputFile]
    DELETE [CUSTOMERSURVEY].[nrc].[OutputFileHistory]
	FROM [CUSTOMERSURVEY].[nrc].[OutputFileHistory] hist
	JOIN [CUSTOMERSURVEY].[nrc].[OutputFile] mstr
	ON
      hist.[PatientNameGiven] = mstr.[PatientNameGiven]
	  AND hist.[PatientNameFamily] = mstr.[PatientNameFamily]
	  AND hist.[AddressStreet1] = mstr.[AddressStreet1]
      AND hist.[AddressCity] = mstr.[AddressCity]
      AND hist.[AddressState] = mstr.[AddressState]
	  AND hist.[AddressPostalCode] = mstr.[AddressPostalCode]
	  AND hist.[PhoneAreaCityCode] = mstr.[PhoneAreaCityCode]
	  AND hist.[PhoneLocalNumber] = mstr.[PhoneLocalNumber]
	  AND hist.[MRN] = mstr.[MRN]
	  AND hist.[DateOfBirth] = mstr.[DateOfBirth]
	  AND hist.[AdministrativeSex] = mstr.[AdministrativeSex]
	  AND hist.[PrimaryLanguage] = mstr.[PrimaryLanguage]
	  AND hist.[Race] = mstr.[Race]
      AND hist.[EthnicGroup] = mstr.[EthnicGroup]
	  AND hist.[MaritalStatus] = mstr.[MaritalStatus]
	  AND hist.[Email] = mstr.[Email]
	  AND hist.[PatientClass] = mstr.[PatientClass]
	  AND hist.[FacilityName] = mstr.[FacilityName]
	  AND hist.[FacilityNumber] = mstr.[FacilityNumber]
	  AND hist.[VisitNumber]  = mstr.[VisitNumber]
	  AND hist.[AdmitDateTime] = mstr.[AdmitDateTime]
	  AND hist.[DischargeDateTime] = mstr.[DischargeDateTime]
	  AND hist.[AdmitSource] = mstr.[AdmitSource]
      AND hist.[DischargeStatus] = mstr.[DischargeStatus]
	  AND hist.[LocationCriteria] = mstr.[LocationCriteria]
	  AND hist.[Location] = mstr.[Location]
	  AND hist.[MSDRG] = mstr.[MSDRG]
	  AND hist.[DiagnosisPrimaryICD10] = mstr.[DiagnosisPrimaryICD10]
	  AND hist.[Diagnosis2ICD10] = mstr.[Diagnosis2ICD10]
      AND hist.[Diagnosis3ICD10] = mstr.[Diagnosis3ICD10]
	  AND hist.[IsDeceased] = mstr.[IsDeceased]
	  AND hist.[ICU] = mstr.[ICU]
	  AND hist.[EDAdmit] = mstr.[EDAdmit]
      AND hist.[PrimaryPayerID] = mstr.[PrimaryPayerID]
	  AND hist.[PrimaryPayerName] = mstr.[PrimaryPayerName]
	  AND hist.[AttendingDoctorNameGiven] = mstr.[AttendingDoctorNameGiven]
	  AND hist.[AttendingDoctorNameSecondGiven] = mstr.[AttendingDoctorNameSecondGiven]
      AND hist.[AttendingDoctorNameFamily] = mstr.[AttendingDoctorNameFamily]
      AND hist.[AttendingDoctorNameSuffix] = mstr.[AttendingDoctorNameSuffix]
	  AND hist.[AttendingDoctorDegree] = mstr.[AttendingDoctorDegree]
      AND hist.[AttendingDoctorNPI] = mstr.[AttendingDoctorNPI]
	  AND hist.[AttendingDoctorSpecialty] = mstr.[AttendingDoctorSpecialty]
      AND hist.[ProcedurePrimaryCPT] = mstr.[ProcedurePrimaryCPT]
      AND hist.[Procedure2CPT] = mstr.[Procedure2CPT]
      AND hist.[Procedure3CPT] = mstr.[Procedure3CPT]
      AND hist.[HNumIPDisch] = mstr.[HNumIPDisch]
      AND hist.[PreferredOutreachMode] = mstr.[PreferredOutreachMode]
	  AND hist.[OutputFullFileName] = mstr.[OutputFullFileName]
	  AND hist.[ImportFullFileName] = mstr.[ImportFullFileName]
	  AND hist.[CGCAHPS] = mstr.[CGCAHPS];
      
    SET @ActualRecordCount = @@ROWCOUNT
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailEnd', NULL, NULL, NULL, @AnticipatedRecordCount, @ActualRecordCount
    
	
    SET @ActualRecordCount = @@ROWCOUNT
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailEnd', NULL, NULL, NULL, @AnticipatedRecordCount, @ActualRecordCount
    
----------------------------------------------------------------------------------------------------------------------------------------------------

    SET @BatchDetailDescription = '100/110:  Insert into [CUSTOMERSURVEY].[nrc].[OutputFileHistory] all the values in [CUSTOMERSURVEY].[nrc].[OutputFile].'
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailStart', @BatchDetailDescription
	
	  SELECT @AnticipatedRecordCount = COUNT(*)
	                                   FROM [CUSTOMERSURVEY].[nrc].[OutputFile];

      -- Insert into [CUSTOMERSURVEY].[nrc].[OutputFileHistory] all the values in [CUSTOMERSURVEY].[nrc].[OutputFile].
	  INSERT INTO [CUSTOMERSURVEY].[nrc].[OutputFileHistory]
      (
	     [PatientNameGiven]
         ,[PatientNameFamily]
         ,[AddressStreet1]
         ,[AddressCity]
         ,[AddressState]
         ,[AddressPostalCode]
         ,[PhoneAreaCityCode]
         ,[PhoneLocalNumber]
         ,[MRN]
         ,[DateOfBirth]
         ,[AdministrativeSex]
         ,[PrimaryLanguage]
         ,[Race]
         ,[EthnicGroup]
         ,[MaritalStatus]
         ,[Email]
         ,[PatientClass]
         ,[FacilityName]
         ,[FacilityNumber]
         ,[VisitNumber]
         ,[AdmitDateTime]
         ,[DischargeDateTime]
         ,[AdmitSource]
         ,[DischargeStatus]
         ,[LocationCriteria]
         ,[Location]
         ,[MSDRG]
         ,[DiagnosisPrimaryICD10]
         ,[Diagnosis2ICD10]
         ,[Diagnosis3ICD10]
         ,[IsDeceased]
         ,[ICU]
         ,[EDAdmit]
         ,[PrimaryPayerID]
         ,[PrimaryPayerName]
         ,[AttendingDoctorNameGiven]
         ,[AttendingDoctorNameSecondGiven]
         ,[AttendingDoctorNameFamily]
         ,[AttendingDoctorNameSuffix]
         ,[AttendingDoctorDegree]
         ,[AttendingDoctorNPI]
         ,[AttendingDoctorSpecialty]
         ,[ProcedurePrimaryCPT]
         ,[Procedure2CPT]
         ,[Procedure3CPT]
         ,[HNumIPDisch]
         ,[PreferredOutreachMode]
         ,[OutputFullFileName]
         ,[ImportFullFileName]
         ,[CGCAHPS]
	  )
      SELECT 
	     [PatientNameGiven]
         ,[PatientNameFamily]
         ,[AddressStreet1]
         ,[AddressCity]
         ,[AddressState]
         ,[AddressPostalCode]
         ,[PhoneAreaCityCode]
         ,[PhoneLocalNumber]
         ,[MRN]
         ,[DateOfBirth]
         ,[AdministrativeSex]
         ,[PrimaryLanguage]
         ,[Race]
         ,[EthnicGroup]
         ,[MaritalStatus]
         ,[Email]
         ,[PatientClass]
         ,[FacilityName]
         ,[FacilityNumber]
         ,[VisitNumber]
         ,[AdmitDateTime]
         ,[DischargeDateTime]
         ,[AdmitSource]
         ,[DischargeStatus]
         ,[LocationCriteria]
         ,[Location]
         ,[MSDRG]
         ,[DiagnosisPrimaryICD10]
         ,[Diagnosis2ICD10]
         ,[Diagnosis3ICD10]
         ,[IsDeceased]
         ,[ICU]
         ,[EDAdmit]
         ,[PrimaryPayerID]
         ,[PrimaryPayerName]
         ,[AttendingDoctorNameGiven]
         ,[AttendingDoctorNameSecondGiven]
         ,[AttendingDoctorNameFamily]
         ,[AttendingDoctorNameSuffix]
         ,[AttendingDoctorDegree]
         ,[AttendingDoctorNPI]
         ,[AttendingDoctorSpecialty]
         ,[ProcedurePrimaryCPT]
         ,[Procedure2CPT]
         ,[Procedure3CPT]
         ,[HNumIPDisch]
         ,[PreferredOutreachMode]
         ,[OutputFullFileName]
         ,[ImportFullFileName]
         ,[CGCAHPS]         
	FROM [CUSTOMERSURVEY].[nrc].[OutputFile]
	
    SET @ActualRecordCount = @@ROWCOUNT
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailEnd', NULL, NULL, NULL, @AnticipatedRecordCount, @ActualRecordCount
----------------------------------------------------------------------------------------------------------------------------------------------------

    SET @BatchDetailDescription = '110/110:  Output whether finalize was successful.'
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailStart', @BatchDetailDescription
	
	  SELECT @AnticipatedRecordCount = 1;
	
      SET @MyCount = 0
      SELECT @MyCount = COUNT(*)
      FROM [CUSTOMERSURVEY].[nrc].[InputFileMaster];

      IF (@MyCount > 0) BEGIN
        SET @IsOk = 1
      END 
      ELSE BEGIN
        SET @IsOk = 0
      END
      SELECT @IsOk AS [IsOk]
	
    SET @ActualRecordCount = @@ROWCOUNT
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailEnd', NULL, NULL, NULL, @AnticipatedRecordCount, @ActualRecordCount
----------------------------------------------------------------------------------------------------------------------------------------------------

--  Close Batch
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT, @BatchOutID, 'BatchEnd'


set @IsOk = 0
select @IsOk as [IsOk]

END TRY


BEGIN CATCH
DECLARE @Err              int
     ,  @ErrorMessage     varchar(Max)
     ,  @ErrorLine        varchar(128)
     ,  @Workstation      varchar(128) = @Application
     ,  @Procedure        VARCHAR(500)

    IF ERROR_NUMBER() IS NULL 
      SET @Err =0;
    ELSE
      SET @Err = ERROR_NUMBER();

    SET @ErrorMessage = ERROR_MESSAGE()
    SET @ErrorLine    = 'SP Line Number: ' + CAST(ERROR_LINE() as varchar(10)) 
    
	SET @Workstation  = HOST_NAME()
	
    SET @Procedure    = @@SERVERNAME + '.' + DB_NAME() + '.' + OBJECT_SCHEMA_NAME(@@ProcID) + '.' + OBJECT_NAME(@@ProcID) + ' - ' + @ErrorLine + ' - ' + LEFT(@BatchDetailDescription, 7)
    EXEC Admin.Utilities.administration.di_ErrorLog  @Application ,@Process, @Version ,0, @ErrorMessage, @Procedure,  @User , @Workstation

    SET @BatchMessage = 'Process Failed:  ' +  @ErrorMessage
    EXEC Admin.Utilities.logs.di_batch @BatchOutID OUTPUT, @BatchOutID, 'BatchEnd', @BatchMessage
	
    RAISERROR(@ErrorMessage, 16,1)

END CATCH


END

