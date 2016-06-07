using System;

namespace RingCentral.Http
{
    public class ApiException : Exception
    {
        public ApiResponse Response { get; private set; }
        public ApiException(string message, ApiResponse response) : base(message)
        {
            Response = response;
        }
    }
}
