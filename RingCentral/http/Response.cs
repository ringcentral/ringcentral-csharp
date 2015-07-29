using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace RingCentral.Http
{
    class Response : Headers
    {

        private int Status;
        private string StatusText;
        private string Body;

        public bool IsMultiPartResponse { get; set; }


        public Response(int status, string statusText, string body, HttpContentHeaders headers)
        {
            Body = body;
            StatusText = statusText;
            Body = body;
            Status = status;

            SetHeaders(headers);
        }

        public bool CheckStatus()
        {
            return Status >= 200 && Status < 300;
        }

        public string GetBody()
        {
            return Body;
        }

        public JObject GetJson()
        {
            if (!IsJson())
            {
                throw new Exception("Response is not JSON");
            }

             return JObject.Parse(Body);

        }
        
        //TODO: Method unimplemented
        public List<string> GetResponse()
        {
            return null;
        }

        public int GetStatus()
        {
            return Status;
        }

        public string GetStatusText()
        {
            return StatusText;
        }

        public string GetError()
        {
            if (CheckStatus())
            {
                return null;
            }

            var message = GetStatus().ToString();

            var data = GetJson();

            if (!string.IsNullOrEmpty((string) (data["message"])))
            {
                message = (string) (data["message"]);
            }
            if (!string.IsNullOrEmpty((string)(data["error_description"])))
            {
                message = (string)(data["error_description"]);
            }
            if (!string.IsNullOrEmpty((string)(data["description"])))
            {
                message = (string)(data["description"]);
            }

            return message;
        }

        
    }
}
