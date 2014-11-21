using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace App.Api
{
    public static class PathHelper
    {
        public static MediaTypeHeaderValue GetMimeType(string path)
        {
            string extension = Path.GetExtension(path);

            switch (extension)
            {
                case ".js":
                    return new MediaTypeHeaderValue("text/javascript");

                case ".css":
                    return new MediaTypeHeaderValue("text/css");

                case ".html":
                    return new MediaTypeHeaderValue("text/html");

                case ".json":
                    return new MediaTypeHeaderValue("application/json");

                case ".less":
                    return new MediaTypeHeaderValue("text/css");

                default:
                    throw new NotSupportedException();
            }
        }
    }
}
