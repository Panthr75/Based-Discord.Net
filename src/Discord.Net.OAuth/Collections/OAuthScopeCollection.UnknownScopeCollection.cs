using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Discord.OAuth;

/// <summary>
/// The representation of an unknown oauth scope collection.
/// </summary>
public interface IOAuthUnknownScopeCollection :
    ICollection<string>,
    ICollection,
    IReadOnlyCollection<string>,
    IEnumerable<string>,
    IEnumerable
{
    /// <summary>
    /// Returns whether or not this unknown oauth scope
    /// collection contains the specified scope.
    /// </summary>
    /// <remarks>
    /// Comparisons are made with
    /// <see cref="StringComparison.OrdinalIgnoreCase"/>.
    /// </remarks>
    /// <param name="scope">
    /// The scope to check.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if this unknown oauth scope
    /// collection contains <paramref name="scope"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    new bool Contains([NotNullWhen(true)] string? scope);

    /// <summary>
    /// Returns whether or not this unknown oauth scope
    /// collection contains the specified scope.
    /// </summary>
    /// <remarks>
    /// Comparisons are made with
    /// <see cref="StringComparison.OrdinalIgnoreCase"/>.
    /// </remarks>
    /// <param name="scope">
    /// The scope to check.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if this unknown oauth scope
    /// collection contains <paramref name="scope"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    bool Contains(ReadOnlySpan<char> scope);
}

partial class OAuthScopeCollection
{
    /// <summary>
    /// An immutable representation of unknown oauth scopes.
    /// </summary>
    public sealed class UnknownScopeCollection : IOAuthUnknownScopeCollection
    {
        private readonly string stringValue;
        private readonly ScopeStringSegment[] segments;

        internal UnknownScopeCollection(string stringValue, ScopeStringSegment[] segments)
        {
            this.stringValue = stringValue;
            this.segments = segments;
        }

        /// <summary>
        /// Gets the number of unknown scopes in this
        /// collection.
        /// </summary>
        public int Count => this.segments.Length;

        /// <inheritdoc cref="IOAuthUnknownScopeCollection.Contains(string?)"/>
        public bool Contains([NotNullWhen(true)] string? scope)
        {
            if (scope is null)
                return false;
            if (this.segments.Length == 0)
                return false;

            ReadOnlySpan<char> scopeSpan = scope.AsSpan().Trim();
            if (scopeSpan.Length == 0)
                return false;

            ReadOnlySpan<char> stringSpan = this.stringValue.AsSpan();
            foreach (ScopeStringSegment segment in this.segments)
            {
                if (stringSpan.Slice(segment.startIndex, segment.length).Equals(scopeSpan, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc cref="IOAuthUnknownScopeCollection.Contains(ReadOnlySpan{char})"/>
        public bool Contains(ReadOnlySpan<char> scope)
        {
            if (this.segments.Length == 0)
                return false;

            scope = scope.Trim();

            if (scope.IsEmpty)
                return false;

            ReadOnlySpan<char> stringSpan = this.stringValue.AsSpan();
            foreach (ScopeStringSegment segment in this.segments)
            {
                if (stringSpan.Slice(segment.startIndex, segment.length).Equals(scope, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Copies the elements of this unknown scope
        /// collection to the specified array.
        /// </summary>
        /// <param name="array">
        /// The zero-index-based one-dimensional string array
        /// to copy the unknown scopes to.
        /// </param>
        /// <param name="arrayIndex">
        /// The zero-based index in <paramref name="array"/> at
        /// which copying begins.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="arrayIndex"/> is less than 0.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// There's not enough space in
        /// <paramref name="array"/> starting at
        /// <paramref name="arrayIndex"/> to fit this unknown
        /// scope collection.
        /// </exception>
        public void CopyTo(string[] array, int arrayIndex)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (arrayIndex + this.segments.Length > array.Length)
                throw new ArgumentException("Not enough space to copy items into array.", nameof(array));

            for (int i = 0; i < this.segments.Length; i++)
            {
                ScopeStringSegment segment = this.segments[i];
                string stringValue;
                if (segment.stringValue is null)
                {
                    stringValue = this.stringValue.Substring(segment.startIndex, segment.length);
                    this.segments[i] = segment.WithStringValue(stringValue);
                }
                else
                {
                    stringValue = segment.stringValue;
                }

                array[arrayIndex++] = stringValue;
            }
        }

        /// <summary>
        /// Gets an enumerator that returns
        /// ReadOnlySpan&lt;<see cref="char"/>&gt;
        /// instead of <see cref="string"/>, reducing
        /// allocations from substringing the underlying
        /// scope string value.
        /// </summary>
        /// <returns>
        /// A span enumerator.
        /// </returns>
        public SpanEnumerator GetSpanEnumerator()
        {
            return new SpanEnumerator(this);
        }

        /// <summary>
        /// Gets an enumerator that returns
        /// <see cref="string"/>. Each enumeration requires
        /// substringing the underlying scope string value. 
        /// </summary>
        /// <returns>
        /// A string enumerator.
        /// </returns>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        internal SlowEnumerator GetSlowEnumerator()
        {
            return new SlowEnumerator(this);
        }

        bool ICollection<string>.IsReadOnly => true;
        bool ICollection.IsSynchronized => true;
        object ICollection.SyncRoot => this;

        void ICollection<string>.Add(string item)
            => throw new NotSupportedException();

        void ICollection<string>.Clear()
            => throw new NotSupportedException();

        void ICollection.CopyTo(Array array, int index)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (array.Rank != 1)
                throw new ArgumentException("Multi-Dimensional arrays aren't supported", nameof(array));
            if (index + this.segments.Length > array.Length)
                throw new ArgumentException("Not enough space to copy items into array.", nameof(array));

            for (int i = 0; i < this.segments.Length; i++)
            {
                ScopeStringSegment segment = this.segments[i];
                string stringValue;
                if (segment.stringValue is null)
                {
                    stringValue = this.stringValue.Substring(segment.startIndex, segment.length);
                    this.segments[i] = segment.WithStringValue(stringValue);
                }
                else
                {
                    stringValue = segment.stringValue;
                }

                array.SetValue(stringValue, index++);
            }
        }

        bool ICollection<string>.Remove(string item)
            => throw new NotSupportedException();

        IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            return new SlowEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new SlowEnumerator(this);
        }

        /// <summary>
        /// An empty Unknown Scope Collection.
        /// </summary>
        public static readonly UnknownScopeCollection Empty = new(string.Empty, Array.Empty<ScopeStringSegment>());

        /// <summary>
        /// An enumerator that enumerates through a collection
        /// of unknown oauth scopes that returns spans instead
        /// of strings, reducing allocations as an element may
        /// need to be allocated by substringing the underlying
        /// string.
        /// </summary>
        public ref struct SpanEnumerator
        {
            private readonly ReadOnlySpan<char> fullSpan;
            private readonly ScopeStringSegment[] segments;
            private int index;
            private ReadOnlySpan<char> current;

            internal SpanEnumerator(UnknownScopeCollection collection)
            {
                this.fullSpan = collection.stringValue.AsSpan();
                this.segments = collection.segments;
                this.index = -1;
            }

            /// <summary>
            /// The current scope in the enumerator.
            /// </summary>
            public readonly ReadOnlySpan<char> Current => this.current;

            /// <summary>
            /// Advances to the next scope in the unknown scope
            /// collection.
            /// </summary>
            /// <returns>
            /// <see langword="true"/> if another scope exists
            /// in the unknown scope collection; otherwise,
            /// <see langword="false"/>.
            /// </returns>
            public bool MoveNext()
            {
                if (this.index >= this.segments.Length)
                    return false;

                ScopeStringSegment segment = this.segments[++this.index];
                this.current = this.fullSpan.Slice(segment.startIndex, segment.length);
                return true;
            }
        }

        /// <summary>
        /// An enumerator that enumerates through a collection
        /// of unknown oauth scopes, but may allocate a new
        /// string for each unknown scope if one wasn't
        /// allocated already by substringing the underlying
        /// string.
        /// </summary>
        public struct Enumerator
        {
            private readonly string fullString;
            private readonly ScopeStringSegment[] segments;
            private int index;
            private string current = null!;

            internal Enumerator(UnknownScopeCollection collection)
            {
                this.fullString = collection.stringValue;
                this.segments = collection.segments;
                this.index = -1;
            }

            /// <summary>
            /// The current scope in the enumerator.
            /// </summary>
            public readonly string Current => this.current;

            /// <summary>
            /// Advances to the next scope in the unknown scope
            /// collection.
            /// </summary>
            /// <returns>
            /// <see langword="true"/> if another scope exists
            /// in the unknown scope collection; otherwise,
            /// <see langword="false"/>.
            /// </returns>
            public bool MoveNext()
            {
                if (this.index >= this.segments.Length)
                    return false;

                ScopeStringSegment segment = this.segments[++this.index];
                if (segment.stringValue is null)
                {
                    this.current = this.fullString.Substring(segment.startIndex, segment.length);
                    this.segments[this.index] = segment.WithStringValue(this.current);
                }
                else
                {
                    this.current = segment.stringValue;
                }
                 
                return true;
            }
        }

        internal sealed class SlowEnumerator : IEnumerator<string>
        {
            private readonly string fullString;
            private readonly ScopeStringSegment[] segments;
            private int index;

            private string current = null!;

            internal SlowEnumerator(UnknownScopeCollection collection)
            {
                this.fullString = collection.stringValue;
                this.segments = collection.segments;
                this.index = -1;
            }

            public string Current => this.current;
            object IEnumerator.Current => this.Current;

            public void Dispose()
            { }

            public bool MoveNext()
            {
                if (this.index >= this.segments.Length)
                    return false;

                ScopeStringSegment segment = this.segments[++this.index];
                if (segment.stringValue is null)
                {
                    this.current = this.fullString.Substring(segment.startIndex, segment.length);
                    this.segments[this.index] = segment.WithStringValue(this.current);
                }
                else
                {
                    this.current = segment.stringValue;
                }

                return true;
            }

            public void Reset()
            {
                this.current = null!;
                this.index = -1;
            }
        }
    }
}
