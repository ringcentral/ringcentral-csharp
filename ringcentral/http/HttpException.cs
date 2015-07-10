using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ringcentral.http
{
    class HttpException : Exception
    {

        protected Request Request;
        protected Response Response;

        public HttpException(Request request, Response response, Exception previous) : base(response.GetError() ,previous)
        {

        }

        public Response GetResponse()
        {
            return Response;
        }

        public Request GetRequest()
        {
            return Request;
        }
    }
}
