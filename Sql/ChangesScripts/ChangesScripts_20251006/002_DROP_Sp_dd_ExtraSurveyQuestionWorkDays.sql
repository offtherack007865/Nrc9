-- SQL Server Instance:  smg-sql01
IF (@@SERVERNAME <> 'smg-sql01')
BEGIN
PRINT 'Invalid SQL Server Connection'
RETURN
END

USE [CUSTOMERSURVEY];



-- STEP 001 of 001
-- Drop SP [nrc].[dd_ExtraSurveyQuestionWorkDays];
SELECT 1;
-- 1 record 

BEGIN TRANSACTION;

DROP PROCEDURE [nrc].[dd_ExtraSurveyQuestionWorkDays];

IF @@ERROR > 0 BEGIN
  SELECT 0;
  ROLLBACK TRANSACTION;
END
ELSE BEGIN
  SELECT 1;
  COMMIT TRANSACTION;
END
