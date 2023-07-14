using System;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="IMessageComponent"/>
    /// </summary>
    public interface IMessageComponentShim : IMessageComponent, IConvertibleShim<IMessageComponent>, IEquatable<IMessageComponentShim>, IEquatable<IMessageComponent>
    { }
}
