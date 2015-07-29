using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RingCentral.Test
{
    public class MockHttpClient : DelegatingHandler
    {
        private readonly Dictionary<Uri, HttpResponseMessage>
        _GetMockResponses = new Dictionary<Uri, HttpResponseMessage>();
        private readonly Dictionary<Uri, HttpResponseMessage>
        _PostMockResponses = new Dictionary<Uri, HttpResponseMessage>();
        private readonly Dictionary<Uri, HttpResponseMessage>
        _DeleteMockResponses = new Dictionary<Uri, HttpResponseMessage>();
        private readonly Dictionary<Uri, HttpResponseMessage>
        _PutMockResponses = new Dictionary<Uri, HttpResponseMessage>();

        public void AddGetMockResponse(Uri uri, HttpResponseMessage responseMessage)
        {
            _GetMockResponses.Add(uri, responseMessage);
        }
        public void AddDeleteMockResponse(Uri uri, HttpResponseMessage responseMessage)
        {
            _DeleteMockResponses.Add(uri, responseMessage);
        }
        public void AddPostMockResponse(Uri uri, HttpResponseMessage responseMessage)
        {
            _PostMockResponses.Add(uri, responseMessage);
        }
        public void AddPutMockResponse(Uri uri, HttpResponseMessage responseMessage)
        {
            _PutMockResponses.Add(uri, responseMessage);
        }



        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            if(request.Method.Equals(HttpMethod.Get)){
                if((_GetMockResponses.ContainsKey(request.RequestUri))) return TaskEx.FromResult(_GetMockResponses[request.RequestUri]);
                else return TaskEx.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound) { RequestMessage = request });
            }
            if(request.Method.Equals(HttpMethod.Post)){
                 if((_PostMockResponses.ContainsKey(request.RequestUri))) return TaskEx.FromResult(_PostMockResponses[request.RequestUri]);
                else return TaskEx.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound) { RequestMessage = request });
            }
            if (request.Method.Equals(HttpMethod.Delete))
            {
                if ((_DeleteMockResponses.ContainsKey(request.RequestUri))) return TaskEx.FromResult(_DeleteMockResponses[request.RequestUri]);
                else return TaskEx.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound) { RequestMessage = request });
            }
            if (request.Method.Equals(HttpMethod.Put))
            {
                if ((_PutMockResponses.ContainsKey(request.RequestUri))) return TaskEx.FromResult(_PutMockResponses[request.RequestUri]);
                else return TaskEx.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound) { RequestMessage = request });
            }
            else
            {
                return TaskEx.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound) { RequestMessage = request });
            }
        }

        
    }
}
