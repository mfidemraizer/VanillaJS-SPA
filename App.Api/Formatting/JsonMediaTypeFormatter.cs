using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace App.Api.Formatting
{
    public class JsonNetMediaTypeFormatter : MediaTypeFormatter
    {
        public JsonNetMediaTypeFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
        }

        public override bool CanReadType(Type type)
        {
            if (type == typeof(JValue) || type == typeof(JObject) || type == typeof(JArray))
                return false;

            return true;
        }

        public override bool CanWriteType(Type type)
        {
            //if (type == typeof(IKeyValueModel))

            if (type.Name == "IKeyValueModel")
                return false;

            return true;
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            TaskCompletionSource<object> completionSource = new TaskCompletionSource<object>();

            var settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
            };

            string jsonText;

            using (StreamReader reader = new StreamReader(readStream))
            {
                jsonText = reader.ReadToEnd();
            }

            if (jsonText.Trim().StartsWith("{"))
            {
                completionSource.SetResult(JsonConvert.DeserializeObject<ExpandoObject>(jsonText, new ExpandoObjectConverter()));
            }
            else
            {
                object deserializedObject = JsonConvert.DeserializeObject(jsonText);
                JToken token;

                if ((token = deserializedObject as JToken) != null)
                {
                    Type clrType;

                    switch (token.Type)
                    {
                        case JTokenType.Array:
                            clrType = typeof(object[]);
                            break;

                        default:
                            throw new NotSupportedException("Source type not supported");
                    }

                    completionSource.SetResult(token.ToObject(clrType));
                }
                else
                {
                    completionSource.SetResult(deserializedObject);
                }
            }

            return completionSource.Task;
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            TaskCompletionSource<object> completionSource = new TaskCompletionSource<object>();
            completionSource.SetResult(null);

            var settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            string json = JsonConvert.SerializeObject(value);

            byte[] buf = Encoding.Default.GetBytes(json);
            writeStream.Write(buf, 0, buf.Length);
            writeStream.Flush();

            return completionSource.Task;
        }
    }
}