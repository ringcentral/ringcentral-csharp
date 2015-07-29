using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace RingCentral.Http
{
    public class Response : Headers
    {
        private readonly string _body;
        private readonly int _status;


        public Response(int status, string body, HttpContentHeaders headers)
        {
            _body = body;
            _status = status;

            SetHeaders(headers);
        }

        public bool IsMultiPartResponse { get; set; }

        public bool CheckStatus()
        {
            return _status >= 200 && _status < 300;
        }

        public string GetBody()
        {
            return _body;
        }

        public JObject GetJson()
        {
            if (!IsJson())
            {
                throw new Exception("Response is not JSON");
            }
            return JObject.Parse(_body);
        }

        /// <summary>
        ///     Parses a multipart response into List of responses that can be accessed by index.
        /// </summary>
        /// <param name="multiResult">The multipart response that needs to be broken up into a list of responses</param>
        /// <returns>A List of responses from a multipart response</returns>
        public List<string> GetMultiPartResponses()
        {
            string[] output = Regex.Split(_body, "--Boundary([^;]+)");

            string[] splitString = output[1].Split(new[] {"--"}, StringSplitOptions.None);

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

        public int GetStatus()
        {
            return _status;
        }

        public string GetError()
        {
            if (CheckStatus())
            {
                return null;
            }

            string message = GetStatus().ToString();

            JObject data = GetJson();

            if (!string.IsNullOrEmpty((string) (data["message"])))
            {
                message = (string) (data["message"]);
            }
            if (!string.IsNullOrEmpty((string) (data["error_description"])))
            {
                message = (string) (data["error_description"]);
            }
            if (!string.IsNullOrEmpty((string) (data["description"])))
            {
                message = (string) (data["description"]);
            }

            return message;
        }
    }
}