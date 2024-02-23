using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Discord
{
    internal static class Preconditions
    {
        #region Objects
        /// <exception cref="ArgumentNullException"><paramref name="obj"/> must not be <see langword="null"/>.</exception>
        public static void NotNull<T>([NotNull] T? obj, string name, string? msg = null) { if (obj == null) throw CreateNotNullException(name, msg); }
        /// <exception cref="ArgumentNullException"><paramref name="obj"/> must not be <see langword="null"/>.</exception>
        public static void NotNull<T>(Optional<T> obj, string name, string? msg = null) { if (obj.IsSpecified && obj.Value == null) throw CreateNotNullException(name, msg); }

        private static ArgumentNullException CreateNotNullException(string name, string? msg)
        {
            if (msg == null)
                return new ArgumentNullException(paramName: name);
            else
                return new ArgumentNullException(paramName: name, message: msg);
        }
        #endregion

        #region Strings
        /// <exception cref="ArgumentException"><paramref name="obj"/> cannot be blank.</exception>
        public static void NotEmpty(string obj, string name, string? msg = null) { if (obj.Length == 0) throw CreateNotEmptyException(name, msg); }
        /// <exception cref="ArgumentException"><paramref name="obj"/> cannot be blank.</exception>
        public static void NotEmpty(Optional<string> obj, string name, string? msg = null) { if (obj.IsSpecified && obj.Value.Length == 0) throw CreateNotEmptyException(name, msg); }
        /// <exception cref="ArgumentException"><paramref name="obj"/> cannot be blank.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="obj"/> must not be <see langword="null"/>.</exception>
        public static void NotNullOrEmpty([NotNull] string? obj, string name, string? msg = null)
        {
            if (obj == null)
                throw CreateNotNullException(name, msg);
            if (obj.Length == 0)
                throw CreateNotEmptyException(name, msg);
        }
        /// <exception cref="ArgumentException"><paramref name="obj"/> cannot be blank.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="obj"/> must not be <see langword="null"/>.</exception>
        public static void NotNullOrEmpty(Optional<string> obj, string name, string? msg = null)
        {
            if (obj.IsSpecified)
            {
                if (obj.Value == null)
                    throw CreateNotNullException(name, msg);
                if (obj.Value.Length == 0)
                    throw CreateNotEmptyException(name, msg);
            }
        }
        /// <exception cref="ArgumentException"><paramref name="obj"/> cannot be blank.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="obj"/> must not be <see langword="null"/>.</exception>
        public static void NotNullOrWhitespace([NotNull] string? obj, string name, string? msg = null)
        {
            if (obj == null)
                throw CreateNotNullException(name, msg);
            if (obj.Trim().Length == 0)
                throw CreateNotEmptyException(name, msg);
        }
        /// <exception cref="ArgumentException"><paramref name="obj"/> cannot be blank.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="obj"/> must not be <see langword="null"/>.</exception>
        public static void NotNullOrWhitespace(Optional<string> obj, string name, string? msg = null)
        {
            if (obj.IsSpecified)
            {
                if (obj.Value == null)
                    throw CreateNotNullException(name, msg);
                if (obj.Value.Trim().Length == 0)
                    throw CreateNotEmptyException(name, msg);
            }
        }

        private static ArgumentException CreateNotEmptyException(string name, string? msg)
            => new ArgumentException(message: msg ?? "Argument cannot be blank.", paramName: name);

        /// <exception cref="ArgumentException"><paramref name="obj"/> must be at least <paramref name="value"/>.</exception>
        public static void LengthAtLeast(string obj, int value, string name, string? msg = null)
        {
            if (obj.Length < value)
                throw CreateLengthAtLeastException(name, msg, value);
        }
        /// <exception cref="ArgumentException"><paramref name="obj"/> must be at least <paramref name="value"/>.</exception>
        public static void LengthAtLeast(Optional<string> obj, int value, string name, string? msg = null)
        {
            if (obj.IsSpecified && obj.Value.Length < value)
                throw CreateLengthAtLeastException(name, msg, value);
        }
        /// <exception cref="ArgumentException"><paramref name="obj"/> must be greater than <paramref name="value"/>.</exception>
        public static void LengthGreaterThan(string obj, int value, string name, string? msg = null)
        {
            if (obj.Length <= value)
                throw CreateLengthGreaterThanException(name, msg, value);
        }
        /// <exception cref="ArgumentException"><paramref name="obj"/> must be greater than <paramref name="value"/>.</exception>
        public static void LengthGreaterThan(Optional<string> obj, int value, string name, string? msg = null)
        {
            if (obj.IsSpecified && obj.Value.Length <= value)
                throw CreateLengthGreaterThanException(name, msg, value);
        }
        /// <exception cref="ArgumentException"><paramref name="obj"/> must be at most <paramref name="value"/>.</exception>
        public static void LengthAtMost(string obj, int value, string name, string? msg = null)
        {
            if (obj.Length > value)
                throw CreateLengthAtMostException(name, msg, value);
        }
        /// <exception cref="ArgumentException"><paramref name="obj"/> must be at most <paramref name="value"/>.</exception>
        public static void LengthAtMost(Optional<string> obj, int value, string name, string? msg = null)
        {
            if (obj.IsSpecified && obj.Value.Length > value)
                throw CreateLengthAtMostException(name, msg, value);
        }
        /// <exception cref="ArgumentException"><paramref name="obj"/> must be less than <paramref name="value"/>.</exception>
        public static void LengthLessThan(string obj, int value, string name, string? msg = null)
        {
            if (obj.Length > value)
                throw CreateLengthLessThanException(name, msg, value);
        }
        /// <exception cref="ArgumentException"><paramref name="obj"/> must be less than <paramref name="value"/>.</exception>
        public static void LengthLessThan(Optional<string> obj, int value, string name, string? msg = null)
        {
            if (obj.IsSpecified && obj.Value.Length > value)
                throw CreateLengthLessThanException(name, msg, value);
        }

        private static ArgumentException CreateLengthAtLeastException(string name, string? msg, int value)
            => new ArgumentException(message: msg ?? $"Length must be at least {value}.", paramName: name);
        private static ArgumentException CreateLengthGreaterThanException(string name, string? msg, int value)
            => new ArgumentException(message: msg ?? $"Length must be greater than {value}.", paramName: name);
        private static ArgumentException CreateLengthAtMostException(string name, string? msg, int value)
            => new ArgumentException(message: msg ?? $"Length must be at most {value}.", paramName: name);
        private static ArgumentException CreateLengthLessThanException(string name, string? msg, int value)
            => new ArgumentException(message: msg ?? $"Length must be less than {value}.", paramName: name);

        #endregion

        #region Message Validation

        public static void WebhookMessageAtLeastOneOf(string? text = null, MessageComponent? components = null, ICollection<IEmbed>? embeds = null,
            IEnumerable<FileAttachment>? attachments = null)
        {
            if (!string.IsNullOrEmpty(text))
                return;

            if (components != null && components.Components.Count != 0)
                return;

            if (attachments != null && attachments.Count() != 0)
                return;

            if (embeds != null && embeds.Count != 0)
                return;

            throw new ArgumentException($"At least one of 'Content', 'Embeds', 'Components' or 'Attachments' must be specified.");
        }

        public static void MessageAtLeastOneOf(string? text = null, MessageComponent? components = null, ICollection<IEmbed>? embeds = null,
                    ICollection<ISticker>? stickers = null, IEnumerable<FileAttachment>? attachments = null)
        {
            if (!string.IsNullOrEmpty(text))
                return;

            if (components != null && components.Components.Count != 0)
                return;

            if (stickers != null && stickers.Count != 0)
                return;

            if (attachments != null && attachments.Count() != 0)
                return;

            if (embeds != null && embeds.Count != 0)
                return;

            throw new ArgumentException($"At least one of 'Content', 'Embeds', 'Components', 'Stickers' or 'Attachments' must be specified.");
        }

        #endregion

        #region Numerics
#if NET7_0_OR_GREATER
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual<T>(T? obj, T? value, string name, string? msg = null)
            where T : System.Numerics.IEqualityOperators<T, T, bool>
        {
            if (obj == value)
                throw CreateNotEqualException(name, msg, value);
        }
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual<T>(T? obj, T value, string name, string? msg = null)
            where T : struct, System.Numerics.IEqualityOperators<T, T, bool>
        {
            if (obj == value)
                throw CreateNotEqualException(name, msg, value);
        }
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual<T>(Optional<T> obj, T value, string name, string? msg = null)
            where T : System.Numerics.IEqualityOperators<T, T, bool>
        {
            if (obj.IsSpecified && obj.Value == value)
                throw CreateNotEqualException(name, msg, value);
        }
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual<T>(Optional<T?> obj, T value, string name, string? msg = null)
            where T : struct, System.Numerics.IEqualityOperators<T, T, bool>
        {
            if (obj.IsSpecified && obj.Value == value)
                throw CreateNotEqualException(name, msg, value);
        }
#else
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual(sbyte obj, sbyte value, string name, string? msg = null) { if (obj == value) throw CreateNotEqualException(name, msg, value); }
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual(byte obj, byte value, string name, string? msg = null) { if (obj == value) throw CreateNotEqualException(name, msg, value); }
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual(short obj, short value, string name, string? msg = null) { if (obj == value) throw CreateNotEqualException(name, msg, value); }
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual(ushort obj, ushort value, string name, string? msg = null) { if (obj == value) throw CreateNotEqualException(name, msg, value); }
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual(int obj, int value, string name, string? msg = null) { if (obj == value) throw CreateNotEqualException(name, msg, value); }
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual(uint obj, uint value, string name, string? msg = null) { if (obj == value) throw CreateNotEqualException(name, msg, value); }
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual(long obj, long value, string name, string? msg = null) { if (obj == value) throw CreateNotEqualException(name, msg, value); }
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual(ulong obj, ulong value, string name, string? msg = null) { if (obj == value) throw CreateNotEqualException(name, msg, value); }
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual(Optional<sbyte> obj, sbyte value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value == value) throw CreateNotEqualException(name, msg, value); }
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual(Optional<byte> obj, byte value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value == value) throw CreateNotEqualException(name, msg, value); }
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual(Optional<short> obj, short value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value == value) throw CreateNotEqualException(name, msg, value); }
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual(Optional<ushort> obj, ushort value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value == value) throw CreateNotEqualException(name, msg, value); }
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual(Optional<int> obj, int value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value == value) throw CreateNotEqualException(name, msg, value); }
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual(Optional<uint> obj, uint value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value == value) throw CreateNotEqualException(name, msg, value); }
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual(Optional<long> obj, long value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value == value) throw CreateNotEqualException(name, msg, value); }
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual(Optional<ulong> obj, ulong value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value == value) throw CreateNotEqualException(name, msg, value); }
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual(sbyte? obj, sbyte value, string name, string? msg = null) { if (obj == value) throw CreateNotEqualException(name, msg, value); }
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual(byte? obj, byte value, string name, string? msg = null) { if (obj == value) throw CreateNotEqualException(name, msg, value); }
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual(short? obj, short value, string name, string? msg = null) { if (obj == value) throw CreateNotEqualException(name, msg, value); }
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual(ushort? obj, ushort value, string name, string? msg = null) { if (obj == value) throw CreateNotEqualException(name, msg, value); }
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual(int? obj, int value, string name, string? msg = null) { if (obj == value) throw CreateNotEqualException(name, msg, value); }
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual(uint? obj, uint value, string name, string? msg = null) { if (obj == value) throw CreateNotEqualException(name, msg, value); }
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual(long? obj, long value, string name, string? msg = null) { if (obj == value) throw CreateNotEqualException(name, msg, value); }
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual(ulong? obj, ulong value, string name, string? msg = null) { if (obj == value) throw CreateNotEqualException(name, msg, value); }
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual(Optional<sbyte?> obj, sbyte value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value == value) throw CreateNotEqualException(name, msg, value); }
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual(Optional<byte?> obj, byte value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value == value) throw CreateNotEqualException(name, msg, value); }
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual(Optional<short?> obj, short value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value == value) throw CreateNotEqualException(name, msg, value); }
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual(Optional<ushort?> obj, ushort value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value == value) throw CreateNotEqualException(name, msg, value); }
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual(Optional<int?> obj, int value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value == value) throw CreateNotEqualException(name, msg, value); }
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual(Optional<uint?> obj, uint value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value == value) throw CreateNotEqualException(name, msg, value); }
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual(Optional<long?> obj, long value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value == value) throw CreateNotEqualException(name, msg, value); }
        /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
        public static void NotEqual(Optional<ulong?> obj, ulong value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value == value) throw CreateNotEqualException(name, msg, value); }
#endif

        private static ArgumentException CreateNotEqualException<T>(string name, string? msg, T value)
            => new ArgumentException(message: msg ?? $"Value may not be equal to {value}.", paramName: name);

#if NET7_0_OR_GREATER
        /// <exception cref="ArgumentException">Value must be at least <paramref name="value"/>.</exception>
        public static void AtLeast<T>(T obj, T value, string name, string? msg = null)
            where T : System.Numerics.IComparisonOperators<T, T, bool>
        {
            if (obj < value)
                throw CreateAtLeastException(name, msg, value);
        }
        /// <exception cref="ArgumentException">Value must be at least <paramref name="value"/>.</exception>
        public static void AtLeast<T>(T? obj, T value, string name, string? msg = null)
            where T : struct, System.Numerics.IComparisonOperators<T, T, bool>
        {
            if (obj < value)
                throw CreateAtLeastException(name, msg, value);
        }
        /// <exception cref="ArgumentException">Value must be at least <paramref name="value"/>.</exception>
        public static void AtLeast<T>(Optional<T> obj, T value, string name, string? msg = null)
            where T : System.Numerics.IComparisonOperators<T, T, bool>
        {
            if (obj.IsSpecified && obj.Value < value)
                throw CreateAtLeastException(name, msg, value);
        }
        /// <exception cref="ArgumentException">Value must be at least <paramref name="value"/>.</exception>
        public static void AtLeast<T>(Optional<T?> obj, T value, string name, string? msg = null)
            where T : struct, System.Numerics.IComparisonOperators<T, T, bool>
        {
            if (obj.IsSpecified && obj.Value < value)
                throw CreateAtLeastException(name, msg, value);
        }
#else
        /// <exception cref="ArgumentException">Value must be at least <paramref name="value"/>.</exception>
        public static void AtLeast(sbyte obj, sbyte value, string name, string? msg = null) { if (obj < value) throw CreateAtLeastException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be at least <paramref name="value"/>.</exception>
        public static void AtLeast(byte obj, byte value, string name, string? msg = null) { if (obj < value) throw CreateAtLeastException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be at least <paramref name="value"/>.</exception>
        public static void AtLeast(short obj, short value, string name, string? msg = null) { if (obj < value) throw CreateAtLeastException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be at least <paramref name="value"/>.</exception>
        public static void AtLeast(ushort obj, ushort value, string name, string? msg = null) { if (obj < value) throw CreateAtLeastException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be at least <paramref name="value"/>.</exception>
        public static void AtLeast(int obj, int value, string name, string? msg = null) { if (obj < value) throw CreateAtLeastException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be at least <paramref name="value"/>.</exception>
        public static void AtLeast(uint obj, uint value, string name, string? msg = null) { if (obj < value) throw CreateAtLeastException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be at least <paramref name="value"/>.</exception>
        public static void AtLeast(long obj, long value, string name, string? msg = null) { if (obj < value) throw CreateAtLeastException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be at least <paramref name="value"/>.</exception>
        public static void AtLeast(ulong obj, ulong value, string name, string? msg = null) { if (obj < value) throw CreateAtLeastException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be at least <paramref name="value"/>.</exception>
        public static void AtLeast(Optional<sbyte> obj, sbyte value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value < value) throw CreateAtLeastException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be at least <paramref name="value"/>.</exception>
        public static void AtLeast(Optional<byte> obj, byte value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value < value) throw CreateAtLeastException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be at least <paramref name="value"/>.</exception>
        public static void AtLeast(Optional<short> obj, short value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value < value) throw CreateAtLeastException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be at least <paramref name="value"/>.</exception>
        public static void AtLeast(Optional<ushort> obj, ushort value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value < value) throw CreateAtLeastException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be at least <paramref name="value"/>.</exception>
        public static void AtLeast(Optional<int> obj, int value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value < value) throw CreateAtLeastException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be at least <paramref name="value"/>.</exception>
        public static void AtLeast(Optional<uint> obj, uint value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value < value) throw CreateAtLeastException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be at least <paramref name="value"/>.</exception>
        public static void AtLeast(Optional<long> obj, long value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value < value) throw CreateAtLeastException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be at least <paramref name="value"/>.</exception>
        public static void AtLeast(Optional<ulong> obj, ulong value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value < value) throw CreateAtLeastException(name, msg, value); }
#endif

        private static ArgumentException CreateAtLeastException<T>(string name, string? msg, T value)
            => new ArgumentException(message: msg ?? $"Value must be at least {value}.", paramName: name);

#if NET7_0_OR_GREATER
        /// <exception cref="ArgumentException">Value must be greater than <paramref name="value"/>.</exception>
        public static void GreaterThan<T>(T obj, T value, string name, string? msg = null)
            where T : System.Numerics.IComparisonOperators<T, T, bool>
        {
            if (obj <= value)
                throw CreateGreaterThanException(name, msg, value);
        }
        /// <exception cref="ArgumentException">Value must be greater than <paramref name="value"/>.</exception>
        public static void GreaterThan<T>(T? obj, T value, string name, string? msg = null)
            where T : struct, System.Numerics.IComparisonOperators<T, T, bool>
        {
            if (obj <= value)
                throw CreateGreaterThanException(name, msg, value);
        }
        /// <exception cref="ArgumentException">Value must be greater than <paramref name="value"/>.</exception>
        public static void GreaterThan<T>(Optional<T> obj, T value, string name, string? msg = null)
            where T : System.Numerics.IComparisonOperators<T, T, bool>
        {
            if (obj.IsSpecified && obj.Value <= value)
                throw CreateGreaterThanException(name, msg, value);
        }
        /// <exception cref="ArgumentException">Value must be greater than <paramref name="value"/>.</exception>
        public static void GreaterThan<T>(Optional<T?> obj, T value, string name, string? msg = null)
            where T : struct, System.Numerics.IComparisonOperators<T, T, bool>
        {
            if (obj.IsSpecified && obj.Value <= value)
                throw CreateGreaterThanException(name, msg, value);
        }
#else
        /// <exception cref="ArgumentException">Value must be greater than <paramref name="value"/>.</exception>
        public static void GreaterThan(sbyte obj, sbyte value, string name, string? msg = null) { if (obj <= value) throw CreateGreaterThanException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be greater than <paramref name="value"/>.</exception>
        public static void GreaterThan(byte obj, byte value, string name, string? msg = null) { if (obj <= value) throw CreateGreaterThanException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be greater than <paramref name="value"/>.</exception>
        public static void GreaterThan(short obj, short value, string name, string? msg = null) { if (obj <= value) throw CreateGreaterThanException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be greater than <paramref name="value"/>.</exception>
        public static void GreaterThan(ushort obj, ushort value, string name, string? msg = null) { if (obj <= value) throw CreateGreaterThanException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be greater than <paramref name="value"/>.</exception>
        public static void GreaterThan(int obj, int value, string name, string? msg = null) { if (obj <= value) throw CreateGreaterThanException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be greater than <paramref name="value"/>.</exception>
        public static void GreaterThan(uint obj, uint value, string name, string? msg = null) { if (obj <= value) throw CreateGreaterThanException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be greater than <paramref name="value"/>.</exception>
        public static void GreaterThan(long obj, long value, string name, string? msg = null) { if (obj <= value) throw CreateGreaterThanException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be greater than <paramref name="value"/>.</exception>
        public static void GreaterThan(ulong obj, ulong value, string name, string? msg = null) { if (obj <= value) throw CreateGreaterThanException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be greater than <paramref name="value"/>.</exception>
        public static void GreaterThan(Optional<sbyte> obj, sbyte value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value <= value) throw CreateGreaterThanException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be greater than <paramref name="value"/>.</exception>
        public static void GreaterThan(Optional<byte> obj, byte value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value <= value) throw CreateGreaterThanException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be greater than <paramref name="value"/>.</exception>
        public static void GreaterThan(Optional<short> obj, short value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value <= value) throw CreateGreaterThanException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be greater than <paramref name="value"/>.</exception>
        public static void GreaterThan(Optional<ushort> obj, ushort value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value <= value) throw CreateGreaterThanException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be greater than <paramref name="value"/>.</exception>
        public static void GreaterThan(Optional<int> obj, int value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value <= value) throw CreateGreaterThanException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be greater than <paramref name="value"/>.</exception>
        public static void GreaterThan(Optional<uint> obj, uint value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value <= value) throw CreateGreaterThanException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be greater than <paramref name="value"/>.</exception>
        public static void GreaterThan(Optional<long> obj, long value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value <= value) throw CreateGreaterThanException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be greater than <paramref name="value"/>.</exception>
        public static void GreaterThan(Optional<ulong> obj, ulong value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value <= value) throw CreateGreaterThanException(name, msg, value); }
#endif

        private static ArgumentException CreateGreaterThanException<T>(string name, string? msg, T value)
            => new ArgumentException(message: msg ?? $"Value must be greater than {value}.", paramName: name);

#if NET7_0_OR_GREATER
        /// <exception cref="ArgumentException">Value must be at most <paramref name="value"/>.</exception>
        public static void AtMost<T>(T obj, T value, string name, string? msg = null)
            where T : System.Numerics.IComparisonOperators<T, T, bool>
        {
            if (obj > value)
                throw CreateAtMostException(name, msg, value);
        }
        /// <exception cref="ArgumentException">Value must be at most <paramref name="value"/>.</exception>
        public static void AtMost<T>(T? obj, T value, string name, string? msg = null)
            where T : struct, System.Numerics.IComparisonOperators<T, T, bool>
        {
            if (obj > value)
                throw CreateAtMostException(name, msg, value);
        }
        /// <exception cref="ArgumentException">Value must be at most <paramref name="value"/>.</exception>
        public static void AtMost<T>(Optional<T> obj, T value, string name, string? msg = null)
            where T : System.Numerics.IComparisonOperators<T, T, bool>
        {
            if (obj.IsSpecified && obj.Value > value)
                throw CreateAtMostException(name, msg, value);
        }
        /// <exception cref="ArgumentException">Value must be at most <paramref name="value"/>.</exception>
        public static void AtMost<T>(Optional<T?> obj, T value, string name, string? msg = null)
            where T : struct, System.Numerics.IComparisonOperators<T, T, bool>
        {
            if (obj.IsSpecified && obj.Value > value)
                throw CreateAtMostException(name, msg, value);
        }
#else
        /// <exception cref="ArgumentException">Value must be at most <paramref name="value"/>.</exception>
        public static void AtMost(sbyte obj, sbyte value, string name, string? msg = null) { if (obj > value) throw CreateAtMostException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be at most <paramref name="value"/>.</exception>
        public static void AtMost(byte obj, byte value, string name, string? msg = null) { if (obj > value) throw CreateAtMostException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be at most <paramref name="value"/>.</exception>
        public static void AtMost(short obj, short value, string name, string? msg = null) { if (obj > value) throw CreateAtMostException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be at most <paramref name="value"/>.</exception>
        public static void AtMost(ushort obj, ushort value, string name, string? msg = null) { if (obj > value) throw CreateAtMostException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be at most <paramref name="value"/>.</exception>
        public static void AtMost(int obj, int value, string name, string? msg = null) { if (obj > value) throw CreateAtMostException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be at most <paramref name="value"/>.</exception>
        public static void AtMost(uint obj, uint value, string name, string? msg = null) { if (obj > value) throw CreateAtMostException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be at most <paramref name="value"/>.</exception>
        public static void AtMost(long obj, long value, string name, string? msg = null) { if (obj > value) throw CreateAtMostException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be at most <paramref name="value"/>.</exception>
        public static void AtMost(ulong obj, ulong value, string name, string? msg = null) { if (obj > value) throw CreateAtMostException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be at most <paramref name="value"/>.</exception>
        public static void AtMost(Optional<sbyte> obj, sbyte value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value > value) throw CreateAtMostException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be at most <paramref name="value"/>.</exception>
        public static void AtMost(Optional<byte> obj, byte value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value > value) throw CreateAtMostException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be at most <paramref name="value"/>.</exception>
        public static void AtMost(Optional<short> obj, short value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value > value) throw CreateAtMostException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be at most <paramref name="value"/>.</exception>
        public static void AtMost(Optional<ushort> obj, ushort value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value > value) throw CreateAtMostException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be at most <paramref name="value"/>.</exception>
        public static void AtMost(Optional<int> obj, int value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value > value) throw CreateAtMostException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be at most <paramref name="value"/>.</exception>
        public static void AtMost(Optional<uint> obj, uint value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value > value) throw CreateAtMostException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be at most <paramref name="value"/>.</exception>
        public static void AtMost(Optional<long> obj, long value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value > value) throw CreateAtMostException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be at most <paramref name="value"/>.</exception>
        public static void AtMost(Optional<ulong> obj, ulong value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value > value) throw CreateAtMostException(name, msg, value); }
#endif

        private static ArgumentException CreateAtMostException<T>(string name, string? msg, T value)
            => new ArgumentException(message: msg ?? $"Value must be at most {value}.", paramName: name);

#if NET7_0_OR_GREATER

        /// <exception cref="ArgumentException">Value must be less than <paramref name="value"/>.</exception>
        public static void LessThan<T>(T obj, T value, string name, string? msg = null)
            where T : System.Numerics.IComparisonOperators<T, T, bool>
        {
            if (obj >= value)
                throw CreateLessThanException(name, msg, value);
        }
        /// <exception cref="ArgumentException">Value must be less than <paramref name="value"/>.</exception>
        public static void LessThan<T>(T? obj, T value, string name, string? msg = null)
            where T : struct, System.Numerics.IComparisonOperators<T, T, bool>
        {
            if (obj >= value)
                throw CreateLessThanException(name, msg, value);
        }
        /// <exception cref="ArgumentException">Value must be less than <paramref name="value"/>.</exception>
        public static void LessThan<T>(Optional<T> obj, T value, string name, string? msg = null)
            where T : System.Numerics.IComparisonOperators<T, T, bool>
        {
            if (obj.IsSpecified && obj.Value >= value)
                throw CreateLessThanException(name, msg, value);
        }
        /// <exception cref="ArgumentException">Value must be less than <paramref name="value"/>.</exception>
        public static void LessThan<T>(Optional<T?> obj, T value, string name, string? msg = null)
            where T : struct, System.Numerics.IComparisonOperators<T, T, bool>
        {
            if (obj.IsSpecified && obj.Value >= value)
                throw CreateLessThanException(name, msg, value);
        }
#else
        /// <exception cref="ArgumentException">Value must be less than <paramref name="value"/>.</exception>
        public static void LessThan(sbyte obj, sbyte value, string name, string? msg = null) { if (obj >= value) throw CreateLessThanException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be less than <paramref name="value"/>.</exception>
        public static void LessThan(byte obj, byte value, string name, string? msg = null) { if (obj >= value) throw CreateLessThanException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be less than <paramref name="value"/>.</exception>
        public static void LessThan(short obj, short value, string name, string? msg = null) { if (obj >= value) throw CreateLessThanException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be less than <paramref name="value"/>.</exception>
        public static void LessThan(ushort obj, ushort value, string name, string? msg = null) { if (obj >= value) throw CreateLessThanException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be less than <paramref name="value"/>.</exception>
        public static void LessThan(int obj, int value, string name, string? msg = null) { if (obj >= value) throw CreateLessThanException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be less than <paramref name="value"/>.</exception>
        public static void LessThan(uint obj, uint value, string name, string? msg = null) { if (obj >= value) throw CreateLessThanException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be less than <paramref name="value"/>.</exception>
        public static void LessThan(long obj, long value, string name, string? msg = null) { if (obj >= value) throw CreateLessThanException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be less than <paramref name="value"/>.</exception>
        public static void LessThan(ulong obj, ulong value, string name, string? msg = null) { if (obj >= value) throw CreateLessThanException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be less than <paramref name="value"/>.</exception>
        public static void LessThan(Optional<sbyte> obj, sbyte value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value >= value) throw CreateLessThanException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be less than <paramref name="value"/>.</exception>
        public static void LessThan(Optional<byte> obj, byte value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value >= value) throw CreateLessThanException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be less than <paramref name="value"/>.</exception>
        public static void LessThan(Optional<short> obj, short value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value >= value) throw CreateLessThanException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be less than <paramref name="value"/>.</exception>
        public static void LessThan(Optional<ushort> obj, ushort value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value >= value) throw CreateLessThanException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be less than <paramref name="value"/>.</exception>
        public static void LessThan(Optional<int> obj, int value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value >= value) throw CreateLessThanException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be less than <paramref name="value"/>.</exception>
        public static void LessThan(Optional<uint> obj, uint value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value >= value) throw CreateLessThanException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be less than <paramref name="value"/>.</exception>
        public static void LessThan(Optional<long> obj, long value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value >= value) throw CreateLessThanException(name, msg, value); }
        /// <exception cref="ArgumentException">Value must be less than <paramref name="value"/>.</exception>
        public static void LessThan(Optional<ulong> obj, ulong value, string name, string? msg = null) { if (obj.IsSpecified && obj.Value >= value) throw CreateLessThanException(name, msg, value); }
#endif

        private static ArgumentException CreateLessThanException<T>(string name, string? msg, T value)
            => new ArgumentException(message: msg ?? $"Value must be less than {value}.", paramName: name);
#endregion

        #region Bulk Delete
        /// <exception cref="ArgumentOutOfRangeException">Messages are younger than 2 weeks.</exception>
        public static void YoungerThanTwoWeeks(ulong[] collection, string name)
        {
            var minimum = SnowflakeUtils.ToSnowflake(DateTimeOffset.UtcNow.Subtract(TimeSpan.FromDays(14)));
            for (var i = 0; i < collection.Length; i++)
            {
                if (collection[i] == 0)
                    continue;
                if (collection[i] <= minimum)
                    throw new ArgumentOutOfRangeException(name, "Messages must be younger than two weeks old.");
            }
        }
        /// <exception cref="ArgumentException">The everyone role cannot be assigned to a user.</exception>
        public static void NotEveryoneRole(ulong[] roles, ulong guildId, string name)
        {
            for (var i = 0; i < roles.Length; i++)
            {
                if (roles[i] == guildId)
                    throw new ArgumentException(message: "The everyone role cannot be assigned to a user.", paramName: name);
            }
        }
        #endregion

        #region SlashCommandOptions

        /// <exception cref="ArgumentNullException"><paramref name="description"/> or <paramref name="name"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="description"/> or <paramref name="name"/> are either empty or their length exceed limits.</exception>
        public static void Options([NotNull] string? name, [NotNull] string? description)
        {
            // Make sure the name matches the requirements from discord
            NotNullOrEmpty(name, nameof(name));
            NotNullOrEmpty(description, nameof(description));
            AtLeast(name.Length, 1, nameof(name));
            AtMost(name.Length, SlashCommandBuilder.MaxNameLength, nameof(name));
            AtLeast(description.Length, 1, nameof(description));
            AtMost(description.Length, SlashCommandBuilder.MaxDescriptionLength, nameof(description));
        }

        #endregion
    }
}
