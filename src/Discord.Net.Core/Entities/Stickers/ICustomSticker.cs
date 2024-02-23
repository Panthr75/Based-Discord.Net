using System;
using System.Threading.Tasks;

namespace Discord
{
    /// <summary>
    ///     Represents a custom sticker within a guild.
    /// </summary>
    public interface ICustomSticker : ISticker, IDeletable
    {
        /// <summary>
        ///     Gets the users id who uploaded the sticker.
        /// </summary>
        /// <remarks>
        ///     In order to get the author id, the bot needs the MANAGE_EMOJIS_AND_STICKERS permission.
        /// </remarks>
        ulong? AuthorId { get; }

        /// <summary>
        ///     Gets the guild that this custom sticker is in, or <see langword="null"/>
        ///     if it wasn't cached.
        /// </summary>
        IGuild? Guild { get; }

        /// <summary>
        ///     The ID of the guild this sticker is in.
        /// </summary>
        ulong GuildId { get; }

        /// <summary>
        ///     Modifies this sticker.
        /// </summary>
        /// <remarks>
        ///     This method modifies this sticker with the specified properties. To see an example of this
        ///     method and what properties are available, please refer to <see cref="StickerProperties"/>.
        ///     <note>
        ///         The bot needs the <see cref="GuildPermission.ManageEmojisAndStickers">MANAGE_GUILD_EXPRESSIONS</see>
        ///         permission inside the guild in order to modify stickers, unless they were created by the bot,
        ///         in which case only the <see cref="GuildPermission.CreateGuildExpressions">CREATE_GUILD_EXPRESSIONS</see> permission is required.
        ///     </note>
        /// </remarks>
        /// <example>
        ///     <para>The following example replaces the name of the sticker with <c>kekw</c>.</para>
        ///     <code language="cs">
        ///     await sticker.ModifyAsync(x =&gt; x.Name = "kekw");
        ///     </code>
        /// </example>
        /// <param name="func">A delegate containing the properties to modify the sticker with.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous modification operation.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
        Task ModifyAsync(Action<StickerProperties> func, RequestOptions? options = null);

        /// <summary>
        ///     Deletes the current sticker.
        /// </summary>
        /// <remarks>
        ///     <note>
        ///         The bot needs the <see cref="GuildPermission.ManageEmojisAndStickers">MANAGE_GUILD_EXPRESSIONS</see>
        ///         permission inside the guild in order to delete stickers, unless they were created by the bot,
        ///         in which case only the <see cref="GuildPermission.CreateGuildExpressions">CREATE_GUILD_EXPRESSIONS</see> permission is required.
        ///     </note>
        /// </remarks>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///      A task that represents the asynchronous deletion operation.
        /// </returns>
        new Task DeleteAsync(RequestOptions? options = null);
    }
}
