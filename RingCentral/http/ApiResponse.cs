using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace RingCentral.Http
{
    public class ApiResponse : HttpHeaders
    {
        public string Body { get; private set; }

        public ApiResponse(HttpResponseMessage response)
        {
            Response = response;
            Status = Convert.ToInt32(response.StatusCode);
            Body = response.Content.ReadAsStringAsync().Result;
            Headers = response.Content.Headers;
            if (!OK)
            {
                throw new Exception(Error);
            }
        }

        public HttpResponseMessage Response { get; private set; }

        public HttpRequestMessage Request
        {
            get
            {
                return Response.RequestMessage;
            }
        }

        /// <summary>
        ///     Checks to be sure the status code is greater than or equal to 200 and less than 300
        /// </summary>
        /// <returns>bool value if status code is successful</returns>
        public bool OK
        {
            get
            {
                return Status >= 200 && Status < 300;
            }
        }

        /// <summary>
        ///     If header content is JSON it will return full formed and parsed json
        /// </summary>
        /// <returns>A JObject parsed body</returns>
        public JObject Json
        {
            get
            {
                if (!IsJson())
                {
                    throw new Exception("Response is not JSON");
                }
                return JObject.Parse(Body);
            }
        }

        /// <summary>
        ///     Parses a multipart response into List of responses that can be accessed by index.
        /// </summary>
        /// <returns>A List of responses from a multipart response</returns>
        public List<string> GetMultiPartResponses()
        {
            var output = Regex.Split(Body, "--Boundary([^;]+)");
            var splitString = output[1].Split(new[] { "--" }, StringSplitOptions.None);
            var responses = new List<string>();
            foreach (var s in splitString)
            {
                if (s.Contains("{"))
                {
                    var json = s.Substring(s.IndexOf('{'));
                    var token = JObject.Parse(json);
                    responses.Add(token.ToString());
                }
            }
            return responses;
        }

        /// <summary>
        ///     Gets the repsonse status
        /// </summary>
        /// <returns>response status code</returns>
        public int Status { get; private set; }

        /// <summary>
        ///     Gets error if status code is outside the range of values checked in <c>CheckStatus()</c>
        /// </summary>
        /// <returns></returns>
        public string Error
        {
            get
            {
                if (OK)
                {
                    return null;
                }

                var message = "Unknown Error";
                var data = Json;
                if (!string.IsNullOrEmpty((string)(data["message"])))
                {
                    message = (string)(data["message"]);
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
}