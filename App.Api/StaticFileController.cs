using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;

namespace App.Api
{
    public sealed class StaticFileController : ApiController
    {
        [HttpGet, Route("{*path}")]
        public HttpResponseMessage GetStaticFile(string path)
        {
            if(string.IsNullOrEmpty(path) || !Path.HasExtension(path))
            {
                path = "index.html";
            }

            string absolutePath = Path.Combine(ConfigurationManager.AppSettings["rootDirectory"], path);

            if (!File.Exists(absolutePath))
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "File not found");
            }

            string fileExtension = Path.GetExtension(absolutePath);
            string fileContents = File.ReadAllText(absolutePath, Encoding.UTF8);

            MediaTypeHeaderValue mediaType;

            try
            {
                mediaType = PathHelper.GetMimeType(absolutePath);
            }
            catch (NotSupportedException)
            {
                return Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "MIME type not supported");
            }

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            response.Content = new StringContent(fileContents, Encoding.UTF8);
            response.Content.Headers.ContentType = mediaType;

            return response;
        }
    }
}