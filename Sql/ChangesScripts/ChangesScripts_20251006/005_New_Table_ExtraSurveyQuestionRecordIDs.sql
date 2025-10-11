-- SQL Instance Name:  smg-sql01
IF (@@SERVERNAME <> 'smg-sql01')
BEGIN
PRINT 'Invalid SQL Server Connection'
RETURN
END

USE [CUSTOMERSURVEY];

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ExtraSurveyQuestionRecordIDs' AND TABLE_SCHEMA = 'nrc')
   DROP TABLE [nrc].[ExtraSurveyQuestionRecordIDs];
GO
/* -----------------------------------------------------------------------------------------------------------
   Table Name  :  nrc.ExtraSurveyQuestionRecordIDs
   Business Analyis:
   Project/Process :   
   Description     :   Based on the number of unique records in the "clinics" file, randomly generate and
                       store 20% of the "clinics" file record ids in this table.  These records will be marked
                       as needing an extra survey question.                       
                        
   Author          :   Philip Morrison
   Create Date     :   10/6/2025 

   ***********************************************************************************************************
   **         Change History                                                                                **
   ***********************************************************************************************************

   Date       Version    Author             Description
   --------   --------   -----------        ------------
   10/6/2025  1.01.001   Philip Morrison    Created
*/ -----------------------------------------------------------------------------------------------------------                                   

-- STEP 001 of 001
-- Create table [nrc].[ExtraSurveyQuestionRecordIDs]
SELECT 1;
-- 1 record 

BEGIN TRANSACTION;

CREATE TABLE [nrc].[ExtraSurveyQuestionRecordIDs](
    [ExtraSurveyQuestionRecordIDsID] [int] IDENTITY ( 1, 1 ) NOT NULL
	,[ExtraSurveyQuestionRecordID] [int] NOT NULL
 CONSTRAINT [pk_nrcExtraSurveyQuestionRecordIDs] PRIMARY KEY CLUSTERED 
(
	[ExtraSurveyQuestionRecordIDsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

IF @@ERROR > 0 BEGIN
  SELECT 0;
  ROLLBACK TRANSACTION;
END
ELSE BEGIN
  SELECT 1;
  COMMIT TRANSACTION;
END
