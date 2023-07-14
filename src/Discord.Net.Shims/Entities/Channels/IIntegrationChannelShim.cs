using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="IIntegrationChannel"/>
    /// </summary>
    public interface IIntegrationChannelShim : IIntegrationChannel, IGuildChannelShim
    {
        /// <inheritdoc cref="IIntegrationChannel.CreateWebhookAsync(string, System.IO.Stream?, RequestOptions?)"/>
        new Task<IWebhookShim> CreateWebhookAsync(string name, Stream? avatar = null, RequestOptions? options = null);

        /// <inheritdoc cref="IIntegrationChannel.GetWebhookAsync(ulong, RequestOptions?)"/>
        new Task<IWebhookShim?> GetWebhookAsync(ulong id, RequestOptions? options = null);

        /// <inheritdoc cref="IIntegrationChannel.GetWebhooksAsync(RequestOptions?)"/>
        new Task<IReadOnlyCollection<IWebhookShim>> GetWebhooksAsync(RequestOptions? options = null);
    }
}
