/*

This method recieves File as HTTP Post and moves it on server folder Temp. It also checks for attached
parameters data File name. This is a simple way to show how we can use HttpResponseMessage Task to
upload file
 
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApi.Server.Models;

namespace WebApi.Server.Controllers
{
    [RoutePrefix("file")]
    public class FileController : ApiController
    {

        [Route("UploadFile")]
        [HttpPost]
        public Task<HttpResponseMessage> UploadFile()
        {
            HttpRequestMessage request = this.Request;
            string errorMessage = string.Empty;
            MultipartFormDataStreamProvider provider = new MultipartFormDataStreamProvider(HttpContext.Current.Server.MapPath("~/Temp/"));

            var task = request.Content.ReadAsMultipartAsync(provider).ContinueWith<HttpResponseMessage>(o =>
            {
                ApiResultModel result = new ApiResultModel()
                {
                    Status = HttpStatusCode.BadRequest,
                    Data = provider.FormData.ToString()
                };

                //Return error if stream does not have any Files
                if (provider.FileData.Count() == 0)
                {
                    result.ErrorMessage = "Please select a file to upload.";
                    return request.CreateResponse(result.Status, result);
                }

                // Return error if FileName parameter not passed
                string fileName = provider.FormData["FileName"] == null ? null : provider.FormData["FileName"].ToString();                
                if (string.IsNullOrEmpty(fileName))
                {
                    result.ErrorMessage = "Please provide the upload file information. example Key, ErId and CallerId.";
                    return request.CreateResponse(result.Status, result);
                }

                var fileData = provider.FileData[0];
                if (string.IsNullOrEmpty(errorMessage))
                {
                    // Move File on Server from Temp folder
                    string newFile = Path.Combine(HttpRuntime.AppDomainAppPath,"Temp\\" + fileName);
                    if (File.Exists(newFile))
                    {
                        File.Delete(newFile);
                    }

                    File.Move(fileData.LocalFileName, newFile);

                    // Return success Message
                    result.Status = HttpStatusCode.OK;
                    return request.CreateResponse(result.Status, result);
                }
                else
                {
                    result.ErrorMessage = errorMessage;
                    return request.CreateResponse(result.Status, result);
                }
            });

            return task;
        }
    }
}
