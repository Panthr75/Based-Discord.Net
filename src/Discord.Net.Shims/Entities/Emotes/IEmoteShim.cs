using System;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="IEmote"/>
    /// </summary>
    public interface IEmoteShim : IEmote, IEquatable<IEmoteShim>, IEquatable<IEmote>, IConvertibleShim<IEmote>
    {
        /// <inheritdoc cref="IEmote.Name"/>
        new string Name { get; set; }
    }
}
