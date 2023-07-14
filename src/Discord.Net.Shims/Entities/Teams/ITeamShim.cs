using System.Collections.Generic;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="ITeam"/>
    /// </summary>
    public interface ITeamShim : ITeam
    {
        /// <inheritdoc cref="ITeam.Id"/>
        new ulong Id { get; set; }
        /// <inheritdoc cref="ITeam.TeamMembers"/>
        new IList<ITeamMemberShim> TeamMembers { get; }
        /// <inheritdoc cref="ITeam.TeamMembers"/>
        new string Name { get; set; }
        /// <inheritdoc cref="ITeam.OwnerUserId"/>
        new ulong OwnerUserId { get; set; }
    }
}
