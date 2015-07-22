/*

This is a simple web page which lets you upload file or image and pass it to web api via HTTP Client call. We
also attach meta data along with the file i.e. FileName

Run:
 * Make both project to start togather from solution properties. 
 * Make sure web config app setting key "WebApi.Server" points to correct webapi.server URL
*/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace WebApi.Client.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            using (var client = new HttpClient())
            {
                using (var formData = new MultipartFormDataContent())
                {
                    Stream contentStream = file.InputStream;                 
                    contentStream.Seek(0, SeekOrigin.Begin);
                    var fileContent = new StreamContent(contentStream);                                       
                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = file.FileName };
                    formData.Add(fileContent, "filePost");
                    formData.Add(new StringContent(file.FileName),"FileName");

                    // Make PostAsync call to Upload File
                    var result = client.PostAsync(ConfigurationManager.AppSettings["WebApi.Server"], formData).Result;
                    
                    // Read Error Message here from Web Api
                    var msg = result.Content.ReadAsStringAsync();
                    ViewData["result"] = msg.Result;
                }
            }

            return View(ViewData);
        }

    }
}