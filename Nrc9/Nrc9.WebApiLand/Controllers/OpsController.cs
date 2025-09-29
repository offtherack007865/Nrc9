using log4net;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Nrc9.Data.Models;

namespace Nrc9.WebApiLand.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OpsController : ControllerBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OpsController));

        public OpsController(NrcContext inputNrcContext)
        {
            MyContext = inputNrcContext;

            log.Info($"Start of OpsController Connection String:  {MyContext.MyConnectionString}");

        }
        public NrcContext MyContext { get; set; }


        // GET /api/Ops/qy_GetNrcConfig?inputApplicationName=Nrc&inputType=Default&inputProcessName=ImportAndConvert&inputNameFilter=NULL&inputUser=AppUser
        [HttpGet]
        public qy_GetNrcConfigOutput
                    qy_GetNrcConfig
                    (
                       [FromQuery] string inputApplicationName
                      , [FromQuery] string inputType
                      , [FromQuery] string inputProcessName
                      , [FromQuery] string inputNameFilter
                      , [FromQuery] string inputUser
                    )
        {
            qy_GetNrcConfigOutput
                returnOutput =
                    new qy_GetNrcConfigOutput();

            string sql = $"nrc.qy_GetNrcConfig @inputApplicationName, @inputType, @inputProcessName, @inputNameFilter, @inputUser";

            List<SqlParameter> parms = new List<SqlParameter>();

            /* @inputApplicationName [varchar] (128)
  ,@inputType [varchar] (50)  
  ,@inputProcessName [varchar] (128)
  ,@inputNameFilter [varchar] (128)
  ,@inputUser [varchar] (50)
             */

            // @inputApplicationName [varchar] (128)
            SqlParameter parm =
                new SqlParameter
                {
                    ParameterName = "@inputApplicationName",
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    Size = 128,
                    Value = inputApplicationName
                };
            parms.Add(parm);

            // ,@inputType [varchar] (50) 
            parm =
                new SqlParameter
                {
                    ParameterName = "@inputType",
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    Size = 50,
                    Value = inputType
                };
            parms.Add(parm);

            // ,@inputProcessName [varchar] (128)
            parm =
                new SqlParameter
                {
                    ParameterName = "@inputProcessName",
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    Size = 128,
                    Value = inputProcessName
                };
            parms.Add(parm);

            // @inputNameFilter [varchar] (128)
            parm =
                new SqlParameter
                {
                    ParameterName = "@inputNameFilter",
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    Size = 128,
                    Value = inputNameFilter
                };
            parms.Add(parm);

            // @inputUser [varchar] (128)
            parm =
                new SqlParameter
                {
                    ParameterName = "@inputUser",
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    Size = 50,
                    Value = inputUser
                };
            parms.Add(parm);


            try
            {
                returnOutput.qy_GetNrcConfigOutputColumnsList =
                    MyContext
                    .qy_GetNrcConfigOutputColumnsList
                    .FromSqlRaw<qy_GetNrcConfigOutputColumns>
                    (
                          sql
                        , parms.ToArray()
                    )
                    .ToList();
            }
            catch (Exception ex)
            {
                returnOutput.IsOk = false;

                string myErrorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    myErrorMessage = $"{myErrorMessage}.  InnerException:  {ex.InnerException.Message}";
                }
                returnOutput.ErrorMessage = myErrorMessage;
                return returnOutput;
            }
            return returnOutput;
        }

        // GET /api/Ops/dd_InputFile
        [HttpGet]
        public dd_InputFileOutput
                    dd_InputFile()
        {
            dd_InputFileOutput
                returnOutput =
                    new dd_InputFileOutput();

            string sql = $"nrc.dd_InputFile";

            List<SqlParameter> parms = new List<SqlParameter>();

            try
            {
                returnOutput.dd_InputFileOutputColumnsList =
                    MyContext
                    .dd_InputFileOutputColumnsList
                    .FromSqlRaw<dd_InputFileOutputColumns>
                    (
                          sql
                        , parms.ToArray()
                    )
                    .ToList();
            }
            catch (Exception ex)
            {
                returnOutput.IsOk = false;

                string myErrorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    myErrorMessage = $"{myErrorMessage}.  InnerException:  {ex.InnerException.Message}";
                }
                returnOutput.ErrorMessage = myErrorMessage;
                return returnOutput;
            }
            return returnOutput;
        }

        // GET /api/Ops/di_FinalizeInputFile?inputOutputFullFilename=test.csv
        [HttpGet]
        public di_FinalizeInputFileOutput
                    di_FinalizeInputFile([FromQuery] string inputOutputFullFilename)
        {
            di_FinalizeInputFileOutput
                returnOutput =
                    new di_FinalizeInputFileOutput();
                               
            string sql = $"nrc.di_FinalizeInputFile @inputOutputFullFilename";

            List<SqlParameter> parms = new List<SqlParameter>();
            SqlParameter parm =
                new SqlParameter
                {
                    ParameterName = "@inputOutputFullFilename",
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 300,
                    Value = inputOutputFullFilename
                };
            parms.Add(parm);

            try
            {
                returnOutput.di_FinalizeInputFileOutputColumnsList =
                    MyContext
                    .di_FinalizeInputFileOutputColumnsList
                    .FromSqlRaw<di_FinalizeInputFileOutputColumns>
                    (
                          sql
                        , parms.ToArray()
                    )
                    .ToList();
            }
            catch (Exception ex)
            {
                returnOutput.IsOk = false;

                string myErrorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    myErrorMessage = $"{myErrorMessage}.  InnerException:  {ex.InnerException.Message}";
                }
                returnOutput.ErrorMessage = myErrorMessage;
                return returnOutput;
            }
            return returnOutput;
        }










        // GET /api/Ops/qy_GetOutputFileLines
        [HttpGet]
        public qy_GetOutputFileLinesOutput
                    qy_GetOutputFileLines()
        {
            qy_GetOutputFileLinesOutput
                returnOutput =
                    new qy_GetOutputFileLinesOutput();

            string sql = $"nrc.qy_GetOutputFileLines";

            List<SqlParameter> parms = new List<SqlParameter>();

            try
            {
                returnOutput.qy_GetOutputFileLinesOutputColumnsList =
                    MyContext
                    .qy_GetOutputFileLinesOutputColumnsList
                    .FromSqlRaw<qy_GetOutputFileLinesOutputColumns>
                    (
                          sql
                        , parms.ToArray()
                    )
                    .ToList();
            }
            catch (Exception ex)
            {
                returnOutput.IsOk = false;

                string myErrorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    myErrorMessage = $"{myErrorMessage}.  InnerException:  {ex.InnerException.Message}";
                }
                returnOutput.ErrorMessage = myErrorMessage;
                return returnOutput;
            }
            return returnOutput;
        }


        // GET /api/Ops/dd_ExtraSurveyQuestionWorkDays
        [HttpGet]
        public dd_ExtraSurveyQuestionWorkDaysOutput
                    dd_ExtraSurveyQuestionWorkDays()
        {
            dd_ExtraSurveyQuestionWorkDaysOutput
                returnOutput =
                    new dd_ExtraSurveyQuestionWorkDaysOutput();

            string sql = $"nrc.dd_ExtraSurveyQuestionWorkDays";

            List<SqlParameter> parms = new List<SqlParameter>();

            try
            {
                returnOutput.dd_ExtraSurveyQuestionWorkDaysOutputColumnsList =
                    MyContext
                    .dd_ExtraSurveyQuestionWorkDaysOutputColumnsList
                    .FromSqlRaw<dd_ExtraSurveyQuestionWorkDaysOutputColumns>
                    (
                          sql
                        , parms.ToArray()
                    )
                    .ToList();
            }
            catch (Exception ex)
            {
                returnOutput.IsOk = false;

                string myErrorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    myErrorMessage = $"{myErrorMessage}.  InnerException:  {ex.InnerException.Message}";
                }
                returnOutput.ErrorMessage = myErrorMessage;
                return returnOutput;
            }
            return returnOutput;
        }

        // GET /api/Ops/di_ExtraSurveyQuestionWorkDays?inputWorkDay=2
        [HttpGet]
        public di_ExtraSurveyQuestionWorkDaysOutput
                    di_ExtraSurveyQuestionWorkDays(int inputWorkDay)
        {
            di_ExtraSurveyQuestionWorkDaysOutput
                returnOutput =
                    new di_ExtraSurveyQuestionWorkDaysOutput();

            string sql = $"nrc.di_ExtraSurveyQuestionWorkDays @inputWorkDay";

            List<SqlParameter> parms = new List<SqlParameter>();

            // @inputWorkDay
            SqlParameter parm =
                new SqlParameter
                {
                    ParameterName = "@inputWorkDay",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Value = inputWorkDay
                };
            parms.Add(parm);

            try
            {
                returnOutput.di_ExtraSurveyQuestionWorkDaysOutputColumnsList =
                    MyContext
                    .di_ExtraSurveyQuestionWorkDaysOutputColumnsList
                    .FromSqlRaw<di_ExtraSurveyQuestionWorkDaysOutputColumns>
                    (
                          sql
                        , parms.ToArray()
                    )
                    .ToList();
            }
            catch (Exception ex)
            {
                returnOutput.IsOk = false;

                string myErrorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    myErrorMessage = $"{myErrorMessage}.  InnerException:  {ex.InnerException.Message}";
                }
                returnOutput.ErrorMessage = myErrorMessage;
                return returnOutput;
            }
            return returnOutput;
        
        }
    }
}
