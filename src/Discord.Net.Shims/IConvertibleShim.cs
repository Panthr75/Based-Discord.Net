namespace Discord.Shims
{
    /// <summary>
    /// Represents a convertible shim
    /// </summary>
    /// <typeparam name="T">The type of object this shim
    /// is convertible to/from</typeparam>
    public interface IConvertibleShim<T>
    {
        /// <summary>
        /// Applies the given value to this shim.
        /// </summary>
        /// <param name="value">The value to apply.</param>
        void Apply(T value);

        /// <summary>
        /// Unshims this object.
        /// </summary>
        /// <returns>
        /// The unshimmed variant of this object.
        /// </returns>
        T UnShim();
    }
}
