using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="INestedChannel"/>
    /// </summary>
    public interface INestedChannelShim : INestedChannel, IGuildChannelShim
    {
        /// <inheritdoc cref="INestedChannel.CategoryId"/>
        new ulong? CategoryId { get; set; }

        /// <inheritdoc cref="INestedChannel.GetCategoryAsync(CacheMode, RequestOptions?)"/>
        new Task<ICategoryChannelShim?> GetCategoryAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);

        /// <inheritdoc cref="INestedChannel.CreateInviteAsync(int?, int?, bool, bool, RequestOptions?)"/>
        new Task<IInviteMetadataShim> CreateInviteAsync(int? maxAge = 86400, int? maxUses = default(int?), bool isTemporary = false, bool isUnique = false, RequestOptions? options = null);


        /// <inheritdoc cref="INestedChannel.CreateInviteToApplicationAsync(ulong, int?, int?, bool, bool, RequestOptions?)"/>
        new Task<IInviteMetadataShim> CreateInviteToApplicationAsync(ulong applicationId, int? maxAge = 86400, int? maxUses = default(int?), bool isTemporary = false, bool isUnique = false, RequestOptions? options = null);

        /// <inheritdoc cref="INestedChannel.CreateInviteToApplicationAsync(DefaultApplications, int?, int?, bool, bool, RequestOptions?)"/>
        new Task<IInviteMetadataShim> CreateInviteToApplicationAsync(DefaultApplications application, int? maxAge = 86400, int? maxUses = default(int?), bool isTemporary = false, bool isUnique = false, RequestOptions? options = null);

        /// <inheritdoc cref="INestedChannel.CreateInviteToStreamAsync(IUser, int?, int?, bool, bool, RequestOptions?)"/>
        Task<IInviteMetadataShim> CreateInviteToStreamAsync(IUserShim user, int? maxAge = 86400, int? maxUses = default(int?), bool isTemporary = false, bool isUnique = false, RequestOptions? options = null);

        /// <inheritdoc cref="INestedChannel.GetInvitesAsync(RequestOptions?)"/>
        new Task<IReadOnlyCollection<IInviteMetadataShim>> GetInvitesAsync(RequestOptions? options = null);
    }
}
