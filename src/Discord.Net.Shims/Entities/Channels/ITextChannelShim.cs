using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="ITextChannel"/>
    /// </summary>
    public interface ITextChannelShim : ITextChannel, IMessageChannelShim, INestedChannelShim, IIntegrationChannelShim
    {
        /// <inheritdoc cref="ITextChannel.IsNsfw"/>
        new bool IsNsfw { get; set; }

        /// <inheritdoc cref="ITextChannel.Topic"/>
        new string? Topic { get; set; }

        /// <inheritdoc cref="ITextChannel.SlowModeInterval"/>
        new int SlowModeInterval { get; set; }

        /// <inheritdoc cref="ITextChannel.DefaultSlowModeInterval"/>
        new int DefaultSlowModeInterval { get; set; }

        /// <inheritdoc cref="ITextChannel.DefaultArchiveDuration"/>
        new ThreadArchiveDuration DefaultArchiveDuration { get; set; }

        /// <inheritdoc cref="ITextChannel.DeleteMessagesAsync(IEnumerable{IMessage}, RequestOptions?)"/>
        Task DeleteMessagesAsync(IEnumerable<IMessageShim> messages, RequestOptions? options = null);

        /// <inheritdoc cref="ITextChannel.CreateThreadAsync(string, ThreadType, ThreadArchiveDuration, IMessage?, bool?, int?, RequestOptions?)"/>
        new Task<IThreadChannelShim> CreateThreadAsync(string name, ThreadType type = ThreadType.PublicThread, ThreadArchiveDuration autoArchiveDuration = ThreadArchiveDuration.OneDay,
            IMessage? message = null, bool? invitable = null, int? slowmode = null, RequestOptions? options = null);

        /// <inheritdoc cref="ITextChannel.GetActiveThreadsAsync(RequestOptions?)"/>
        new Task<IReadOnlyCollection<IThreadChannelShim>> GetActiveThreadsAsync(RequestOptions? options = null);
    }
}
