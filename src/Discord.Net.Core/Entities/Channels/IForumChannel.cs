using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord
{
    /// <summary>
    ///     Represents a forum channel in a guild that can create posts.
    /// </summary>
    public interface IForumChannel : IDiscussionChannel, IIntegrationChannel
    {
        /// <summary>
        ///     Gets a value that indicates whether the channel is NSFW.
        /// </summary>
        /// <returns>
        ///     <see langword="true" /> if the channel has the NSFW flag enabled; otherwise <see langword="false" />.
        /// </returns>
        bool IsNsfw { get; }

        /// <summary>
        /// Gets the rule used to display posts in a forum channel.
        /// </summary>
        ForumLayout DefaultLayout { get; }

        /// <summary>
        ///     Modifies this forum channel.
        /// </summary>
        /// <remarks>
        ///     This method modifies the current forum channel with the specified properties. To see an example of this
        ///     method and what properties are available, please refer to <see cref="ForumChannelProperties"/>.
        /// </remarks>
        /// <param name="func">The delegate containing the properties to modify the channel with.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous modification operation.
        /// </returns>
        Task ModifyAsync(Action<ForumChannelProperties> func, RequestOptions? options = null);

        /// <summary>
        ///     Gets a collection of active threads within this forum channel.
        /// </summary>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents an asynchronous get operation for retrieving the threads. The task result contains
        ///     a collection of active threads.
        /// </returns>
        Task<IReadOnlyCollection<IThreadChannel>> GetActiveThreadsAsync(RequestOptions? options = null);

        /// <summary>
        ///     Gets a collection of publicly archived threads within this forum channel.
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
        ///     Gets a collection of privately archived threads within this forum channel.
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
        ///     Gets a collection of privately archived threads that the current bot has joined within this forum channel.
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
