using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RingCentral.Http
{
    class HttpException : Exception
    {

        protected Response Response;

        public HttpException()
        {
            
        }

        public HttpException(string message) : base(message)
        {
            
        }

        public HttpException(string message, Exception innerException) : base(message ,innerException)
        {
            
        }

        public Response GetResponse()
        {
            return Response;
        }

    }
}
