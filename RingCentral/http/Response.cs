using System;
using System.Collections.Generic;
using System.Linq;
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


        public Response(int status, string statusText, string body, Dictionary<string, string> headers)
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

        /// <summary>
        ///     Parses a multipart response into List of responses that can be accessed by index.
        /// </summary>
        /// <param name="multiResult">The multipart response that needs to be broken up into a list of responses</param>
        /// <returns>A List of responses from a multipart response</returns>
        public List<string> GetMultiPartResponses(string multiResult)
        {
            string[] output = Regex.Split(multiResult, "--Boundary([^;]+)");

            string[] splitString = output[1].Split(new[] { "--" }, StringSplitOptions.None);

            var responses = new List<string>();

            //We Can convert this to linq but for the sake of readability we'll leave it like this.
            foreach (string s in splitString)
            {
                if (s.Contains("{"))
                {
                    string json = s.Substring(s.IndexOf('{'));

                    JToken token = JObject.Parse(json);

                    responses.Add(token.ToString());
                }
            }

            return responses;
        }
    }
}
