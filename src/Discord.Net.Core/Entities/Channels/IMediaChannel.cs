using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord
{
    /// <summary>
    ///     Represents a media channel in a guild that can create posts.
    /// </summary>
    public interface IMediaChannel : IDiscussionChannel
    {

        /// <summary>
        ///     Modifies this discussion channel.
        /// </summary>
        /// <remarks>
        ///     This method modifies the current discussion channel with the specified properties. To see an example of this
        ///     method and what properties are available, please refer to <see cref="MediaChannelProperties"/>.
        /// </remarks>
        /// <param name="func">The delegate containing the properties to modify the channel with.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous modification operation.
        /// </returns>
        Task ModifyAsync(Action<MediaChannelProperties> func, RequestOptions? options = null);

        /// <summary>
        ///     Gets a collection of active threads within this media channel.
        /// </summary>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents an asynchronous get operation for retrieving the threads. The task result contains
        ///     a collection of active threads.
        /// </returns>
        Task<IReadOnlyCollection<IThreadChannel>> GetActiveThreadsAsync(RequestOptions? options = null);

        /// <summary>
        ///     Gets a collection of publicly archived threads within this media channel.
        /// </summary>
        /// <param name="limit">The optional limit of how many to get.</param>
        /// <param name="before">The optional date to return threads created before this timestamp.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents an asynchronous get operation for retrieving the threads. The task result contains
        ///     a collection of publicly archived threads.
        /// </returns>
        Task<IReadOnlyCollection<IThreadChannel>> GetPublicArchivedThreadsAsync(int? limit = null, DateTimeOffset? before = null, RequestOptions? options = null);

        /// <summary>
        ///     Gets a collection of privately archived threads within this media channel.
        /// </summary>
        /// <remarks>
        ///     The bot requires the <see cref="GuildPermission.ManageThreads"/> permission in order to execute this request.
        /// </remarks>
        /// <param name="limit">The optional limit of how many to get.</param>
        /// <param name="before">The optional date to return threads created before this timestamp.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents an asynchronous get operation for retrieving the threads. The task result contains
        ///     a collection of privately archived threads.
        /// </returns>
        Task<IReadOnlyCollection<IThreadChannel>> GetPrivateArchivedThreadsAsync(int? limit = null, DateTimeOffset? before = null, RequestOptions? options = null);

        /// <summary>
        ///     Gets a collection of privately archived threads that the current bot has joined within this media channel.
        /// </summary>
        /// <param name="limit">The optional limit of how many to get.</param>
        /// <param name="before">The optional date to return threads created before this timestamp.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents an asynchronous get operation for retrieving the threads. The task result contains
        ///     a collection of privately archived threads.
        /// </returns>
        Task<IReadOnlyCollection<IThreadChannel>> GetJoinedPrivateArchivedThreadsAsync(int? limit = null, DateTimeOffset? before = null, RequestOptions? options = null);
    }
}
