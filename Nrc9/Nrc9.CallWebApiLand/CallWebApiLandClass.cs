using log4net;
using Newtonsoft.Json;
using Nrc9.Data.Models;
namespace Nrc9.CallWebApiLand
{
    public class CallWebApiLandClass
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CallWebApiLandClass));

        public CallWebApiLandClass
        (
            string inputBaseWebApiUrl
        )
        {
            MyBaseWebApiUrl = inputBaseWebApiUrl;
        }
        public string MyBaseWebApiUrl { get; set; }

        // GET /api/Ops/qy_GetNrcConfig?inputApplicationName=Nrc&inputType=Default&inputProcessName=ImportAndConvert&inputNameFilter=NULL&inputUser=AppUser
        public qy_GetNrcConfigOutput
                    qy_GetNrcConfig
                    (
                        string inputApplicationName
                      , string inputType
                      , string inputProcessName
                      , string inputNameFilter
                      , string inputUser
                    )
        {
            qy_GetNrcConfigOutput
                returnOutput =
                    qy_GetNrcConfigAsync
                    (
                        inputApplicationName
                        , inputType
                        , inputProcessName
                        , inputNameFilter
                        , inputUser
                    )
                    .Result;

            return returnOutput;
        }

        public async Task<qy_GetNrcConfigOutput>
                        qy_GetNrcConfigAsync
                        (
                            string inputApplicationName
                            , string inputType
                            , string inputProcessName
                            , string inputNameFilter
                            , string inputUser
                        )
        {
            log.Info($"In qy_GetNrcConfigAsync");
            qy_GetNrcConfigOutput
                returnOutput =
                    new qy_GetNrcConfigOutput();

            string myCompleteUrl = $"{MyBaseWebApiUrl}/api/Ops/qy_GetNrcConfig?inputApplicationName={inputApplicationName}&inputType={inputType}&inputProcessName={inputProcessName}&inputNameFilter={inputNameFilter}&inputUser={inputUser}";
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromHours(1);

                    var result = await client.GetAsync(myCompleteUrl);
                    var response = await result.Content.ReadAsStringAsync();
                    returnOutput = JsonConvert.DeserializeObject<qy_GetNrcConfigOutput>(response);
                }
            }
            catch (Exception ex)
            {
                returnOutput.IsOk = false;
                string myErrorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    myErrorMessage = $"{myErrorMessage}.  Inner Exception:  {ex.InnerException.Message}";
                }
                return returnOutput;
            }

            return returnOutput;
        }


        // GET /api/Ops/dd_InputFile
        public dd_InputFileOutput
                    dd_InputFile()
        {
            dd_InputFileOutput
                returnOutput =
                    dd_InputFileAsync()
                    .Result;

            return returnOutput;
        }

        public async Task<dd_InputFileOutput>
                        dd_InputFileAsync()
        {
            log.Info($"In du_MarkCollectionsEntryLookedUpTodayAsLookedUpAsync");
            dd_InputFileOutput
                returnOutput =
                    new dd_InputFileOutput();

            string myCompleteUrl = $"{MyBaseWebApiUrl}/api/Ops/dd_InputFile";
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromHours(1);

                    var result = await client.GetAsync(myCompleteUrl);
                    var response = await result.Content.ReadAsStringAsync();
                    returnOutput = JsonConvert.DeserializeObject<dd_InputFileOutput>(response);
                }
            }
            catch (Exception ex)
            {
                returnOutput.IsOk = false;
                string myErrorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    myErrorMessage = $"{myErrorMessage}.  Inner Exception:  {ex.InnerException.Message}";
                }
                return returnOutput;
            }

            return returnOutput;
        }


        // GET /api/Ops/di_FinalizeInputFile?inputOutputFullFilename=test.csv
        public di_FinalizeInputFileOutput
                    di_FinalizeInputFile
                    (
                        string inputOutputFullFilename
                    )
        {
            di_FinalizeInputFileOutput
                returnOutput =
                    di_FinalizeInputFileAsync
                    (
                        inputOutputFullFilename
                    )
                    .Result;

            return returnOutput;
        }

        public async Task<di_FinalizeInputFileOutput>
                        di_FinalizeInputFileAsync
                        (
                            string inputOutputFullFilename
                        )
        {
            log.Info($"In di_FinalizeInputFileAsync");
            di_FinalizeInputFileOutput
                returnOutput =
                    new di_FinalizeInputFileOutput();

            string myCompleteUrl = $"{MyBaseWebApiUrl}/api/Ops/di_FinalizeInputFile?inputOutputFullFilename={inputOutputFullFilename}";
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromHours(1);

                    var result = await client.GetAsync(myCompleteUrl);
                    var response = await result.Content.ReadAsStringAsync();
                    returnOutput = JsonConvert.DeserializeObject<di_FinalizeInputFileOutput>(response);
                }
            }
            catch (Exception ex)
            {
                returnOutput.IsOk = false;
                string myErrorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    myErrorMessage = $"{myErrorMessage}.  Inner Exception:  {ex.InnerException.Message}";
                }
                return returnOutput;
            }

            return returnOutput;
        }


        // GET /api/Ops/qy_GetOutputFileLines
        public qy_GetOutputFileLinesOutput
                    qy_GetOutputFileLines
                    ()
        {
            qy_GetOutputFileLinesOutput
                returnOutput =
                    qy_GetOutputFileLinesAsync
                    ()
                    .Result;

            return returnOutput;
        }

        public async Task<qy_GetOutputFileLinesOutput>
                        qy_GetOutputFileLinesAsync
                        ()
        {
            log.Info($"In qy_GetOutputFileLinesOutput");
            qy_GetOutputFileLinesOutput
                returnOutput =
                    new qy_GetOutputFileLinesOutput();

            string myCompleteUrl = $"{MyBaseWebApiUrl}/api/Ops/qy_GetOutputFileLines";
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromHours(1);

                    var result = await client.GetAsync(myCompleteUrl);
                    var response = await result.Content.ReadAsStringAsync();
                    returnOutput = JsonConvert.DeserializeObject<qy_GetOutputFileLinesOutput>(response);
                }
            }
            catch (Exception ex)
            {
                returnOutput.IsOk = false;
                string myErrorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    myErrorMessage = $"{myErrorMessage}.  Inner Exception:  {ex.InnerException.Message}";
                }
                return returnOutput;
            }

            return returnOutput;
        }


    }
}
