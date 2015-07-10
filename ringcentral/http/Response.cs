using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace ringcentral.http
{
    class Response : Headers
    {

        private int Status;
        private String StatusText;
        private String Body;

        public Response(int status, String statusText, String body, Dictionary<String, String> headers)
        {
            Body = body;
            StatusText = statusText;
            Body = body;
            Status = status;

            SetHeaders(headers);
        }

        public Boolean CheckStatus()
        {
            return Status >= 200 && Status < 300;
        }

        public String GetBody()
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
        public List<String> GetResponse()
        {
            return null;
        }

        public int GetStatus()
        {
            return Status;
        }

        public String GetStatusText()
        {
            return StatusText;
        }

        public String GetError()
        {
            if (CheckStatus())
            {
                return null;
            }

            var message = GetStatus().ToString();

            var data = GetJson();

            if (!String.IsNullOrEmpty((string) (data["message"])))
            {
                message = (string) (data["message"]);
            }
            if (!String.IsNullOrEmpty((string)(data["error_description"])))
            {
                message = (string)(data["error_description"]);
            }
            if (!String.IsNullOrEmpty((string)(data["description"])))
            {
                message = (string)(data["description"]);
            }

            return message;
        }
    }
}
