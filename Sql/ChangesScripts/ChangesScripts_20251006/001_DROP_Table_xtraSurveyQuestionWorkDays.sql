-- SQL Server Instance:  smg-sql01
IF (@@SERVERNAME <> 'smg-sql01')
BEGIN
PRINT 'Invalid SQL Server Connection'
RETURN
END

USE [CUSTOMERSURVEY];


DROP TABLE [nrc].[ExtraSurveyQuestionWorkDays];
