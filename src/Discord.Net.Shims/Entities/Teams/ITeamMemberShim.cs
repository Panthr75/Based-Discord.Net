namespace Discord.Shims
{
    /// <summary>
    /// A shim for <see cref="ITeamMember"/>
    /// </summary>
    public interface ITeamMemberShim : ITeamMember
    {
        /// <inheritdoc cref="ITeamMember.MembershipState"/>
        new MembershipState MembershipState { get; set; }
        /// <inheritdoc cref="ITeamMember.Permissions"/>
        new string[] Permissions { get; set; }
        /// <inheritdoc cref="ITeamMember.TeamId"/>
        new ulong TeamId { get; set; }
        /// <inheritdoc cref="ITeamMember.User"/>
        new IUserShim User { get; }
    }
}
