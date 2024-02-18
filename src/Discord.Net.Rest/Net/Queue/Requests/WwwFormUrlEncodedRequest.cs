using Discord.Net.Rest;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord.Net.Queue
{
    internal class WwwFormUrlEncodedRequest : RestRequest
    {
        public IEnumerable<KeyValuePair<string, string>> NameValueCollection { get; }

        public WwwFormUrlEncodedRequest(IRestClient client, string method, string endpoint, IEnumerable<KeyValuePair<string, string>> nameValueCollection, RequestOptions options)
            : base(client, method, endpoint, options)
        {
            NameValueCollection = nameValueCollection;
        }

        public override Task<RestResponse> SendAsync()
            => Client.SendAsync(Method, Endpoint, NameValueCollection, Options.CancelToken, Options.HeaderOnly, Options.AuditLogReason);
    }
}
