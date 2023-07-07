using System;
using System.Collections.Generic;

namespace Discord;

public class ForumChannelProperties : DiscussionChannelProperties
{
    /// <summary>
    /// Gets or sets the rule used to display posts in a forum channel.
    /// </summary>
    public Optional<ForumLayout> DefaultLayout { get; set; }
}
