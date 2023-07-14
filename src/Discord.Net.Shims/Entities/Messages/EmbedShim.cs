using Discord.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="Embed"/>
    /// </summary>
    [DebuggerDisplay(@"{DebuggerDisplay,nq}")]
    public class EmbedShim : IConvertibleShim<Embed>, IEquatable<Embed>, IEquatable<EmbedShim>, IEmbedShim
    {
        private EmbedType m_type;
        private string? m_description;
        private string? m_title;
        private string? m_url;
        private DateTimeOffset? m_timestamp;
        private Color? m_color;
        private EmbedImageShim? m_image;
        private EmbedVideoShim? m_video;
        private EmbedAuthorShim? m_author;
        private EmbedFooterShim? m_footer;
        private EmbedProviderShim? m_provider;
        private EmbedThumbnailShim? m_thumbnail;
        private readonly EmbedFieldsCollection m_fields;

        protected EmbedFieldsCollection FieldCollection => this.m_fields;

        public EmbedShim()
        {
            this.m_fields = new(this);
        }

        public EmbedShim(Embed embed) : this()
        {
            this.Apply(embed);
        }

        public Embed UnShim()
        {
            return new Embed(
                type: this.Type,
                title: this.Title,
                description: this.Description,
                url: this.Url,
                timestamp: this.Timestamp,
                color: this.Color,
                image: this.Image?.UnShim(),
                video: this.Video?.UnShim(),
                author: this.Author?.UnShim(),
                footer: this.Footer?.UnShim(),
                provider: this.Provider?.UnShim(),
                thumbnail: this.Thumbnail?.UnShim(),
                fields: this.m_fields.UnShim());
        }

        protected virtual EmbedFieldShim ShimField(EmbedField field)
        {
            return new EmbedFieldShim(field);
        }

        [return: NotNullIfNotNull(nameof(author))]
        protected virtual EmbedAuthorShim? ShimAuthor(EmbedAuthor? author)
        {
            if (!author.HasValue)
            {
                return null;
            }
            else
            {
                return new EmbedAuthorShim(author.Value);
            }
        }

        [return: NotNullIfNotNull(nameof(footer))]
        protected virtual EmbedFooterShim? ShimFooter(EmbedFooter? footer)
        {
            if (!footer.HasValue)
            {
                return null;
            }
            else
            {
                return new EmbedFooterShim(footer.Value);
            }
        }

        [return: NotNullIfNotNull(nameof(image))]
        protected virtual EmbedImageShim? ShimImage(EmbedImage? image)
        {
            if (!image.HasValue)
            {
                return null;
            }
            else
            {
                return new EmbedImageShim(image.Value);
            }
        }

        [return: NotNullIfNotNull(nameof(provider))]
        protected virtual EmbedProviderShim? ShimProvider(EmbedProvider? provider)
        {
            if (!provider.HasValue)
            {
                return null;
            }
            else
            {
                return new EmbedProviderShim(provider.Value);
            }
        }

        [return: NotNullIfNotNull(nameof(thumbnail))]
        protected virtual EmbedThumbnailShim? ShimThumbnail(EmbedThumbnail? thumbnail)
        {
            if (!thumbnail.HasValue)
            {
                return null;
            }
            else
            {
                return new EmbedThumbnailShim(thumbnail.Value);
            }
        }

        [return: NotNullIfNotNull(nameof(video))]
        protected virtual EmbedVideoShim? ShimVideo(EmbedVideo? video)
        {
            if (!video.HasValue)
            {
                return null;
            }
            else
            {
                return new EmbedVideoShim(video.Value);
            }
        }

        public void Apply(Embed value)
        {
            this.m_author = this.ShimAuthor(value.Author);
            this.m_color = value.Color;
            this.m_description = value.Description;
            this.m_fields.Apply(value.Fields);
            this.m_footer = this.ShimFooter(value.Footer);
            this.m_image = this.ShimImage(value.Image);
            this.m_provider = this.ShimProvider(value.Provider);
            this.m_thumbnail = this.ShimThumbnail(value.Thumbnail);
            this.m_timestamp = value.Timestamp;
            this.m_title = value.Title;
            this.m_type = value.Type;
            this.m_url = value.Url;
            this.m_video = this.ShimVideo(value.Video);
        }

        /// <inheritdoc/>
        public virtual EmbedType Type
        {
            get => this.m_type;
            set => this.m_type = value;
        }

        /// <inheritdoc/>
        public virtual string? Description
        {
            get => this.m_description;
            set
            {
                if (value != null)
                {
                    value = value.Trim();
                    value = value.Substring(0, Math.Min(value.Length, EmbedBuilder.MaxDescriptionLength));
                }
                this.m_description = value;
            }
        }
        /// <inheritdoc/>
        public virtual string? Url
        {
            get => this.m_url;
            set
            {
                UrlValidation.Validate(value, true);
                this.m_url = value;
            }
        }
        /// <inheritdoc/>
        public virtual string? Title
        {
            get => this.m_title;
            set
            {
                if (value != null)
                {
                    value = value.Trim();
                    value = value.Substring(0, Math.Min(value.Length, EmbedBuilder.MaxTitleLength));
                }
                this.m_title = value;
            }
        }
        /// <inheritdoc/>
        public virtual DateTimeOffset? Timestamp
        {
            get => this.m_timestamp;
            set => this.m_timestamp = value;
        }
        /// <inheritdoc/>
        public virtual Color? Color
        {
            get => this.m_color;
            set => this.m_color = value;
        }
        /// <inheritdoc/>
        public virtual EmbedImageShim? Image
        {
            get => this.m_image;
            set => this.m_image = value;
        }
        EmbedImage? IEmbed.Image => this.Image?.UnShim();
        /// <inheritdoc/>
        public virtual EmbedVideoShim? Video
        {
            get => this.m_video;
            set => this.m_video = value;
        }
        EmbedVideo? IEmbed.Video => this.Video?.UnShim();
        /// <inheritdoc/>
        public virtual EmbedAuthorShim? Author
        {
            get => this.m_author;
            set => this.m_author = value;
        }
        EmbedAuthor? IEmbed.Author => this.Author?.UnShim();
        /// <inheritdoc/>
        public virtual EmbedFooterShim? Footer
        {
            get => this.m_footer;
            set => this.m_footer = value;
        }
        EmbedFooter? IEmbed.Footer => this.Footer?.UnShim();
        /// <inheritdoc/>
        public virtual EmbedProviderShim? Provider
        {
            get => this.m_provider;
            set => this.m_provider = value;
        }
        EmbedProvider? IEmbed.Provider => this.Provider?.UnShim();
        /// <inheritdoc/>
        public virtual EmbedThumbnailShim? Thumbnail
        {
            get => this.m_thumbnail;
            set => this.m_thumbnail = value;
        }
        EmbedThumbnail? IEmbed.Thumbnail => this.Thumbnail?.UnShim();
        /// <inheritdoc/>
        public virtual ICollection<EmbedFieldShim> Fields
        {
            get => this.FieldCollection;
        }

        protected virtual ImmutableArray<EmbedField> UnShimFields()
        {
            return this.FieldCollection.UnShim();
        }
        ImmutableArray<EmbedField> IEmbed.Fields => this.UnShimFields();

        /// <inheritdoc cref="Embed.Length"/>
        public int Length
        {
            get
            {
                int titleLength = this.Title?.Length ?? 0;
                int authorLength = this.Author?.Name?.Length ?? 0;
                int descriptionLength = this.Description?.Length ?? 0;
                int footerLength = this.Footer?.Text?.Length ?? 0;
                int fieldSum = this.Fields.Sum(f => f.Name?.Length + f.Value?.ToString().Length) ?? 0;
                return titleLength + authorLength + descriptionLength + footerLength + fieldSum;
            }
        }

        /// <inheritdoc cref="Embed.ToString()"/>
        public override string? ToString() => this.Title;
        private string DebuggerDisplay => $"{this.Title} ({this.Type})";

        public static implicit operator Embed(EmbedShim v)
        {
            return v.UnShim();
        }

        public static bool operator ==(EmbedShim? left, EmbedShim? right)
        => left is null ? right is null
                : left.Equals(right);

        public static bool operator !=(EmbedShim? left, EmbedShim? right)
            => !(left == right);

        /// <inheritdoc cref="Embed.Equals(object?)"/>
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is EmbedShim shim)
            {
                return this.Equals(shim);
            }
            else if (obj is Embed other)
            {
                return this.Equals(other);
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc cref="Embed.Equals(Embed?)"/>
        public bool Equals([NotNullWhen(true)] Embed? embed)
        {
            if (embed is null ||
                this.Type != embed.Type ||
                this.Title != embed.Title ||
                this.Description != embed.Description ||
                this.Timestamp != embed.Timestamp ||
                this.Color != embed.Color)
            {
                return false;
            }

            if (this.Image is not null)
            {
                if (!this.Image.Equals(embed.Image))
                {
                    return false;
                }
            }
            else if (embed.Image.HasValue)
            {
                return false;
            }

            if (this.Video is not null)
            {
                if (!this.Video.Equals(embed.Video))
                {
                    return false;
                }
            }
            else if (embed.Video.HasValue)
            {
                return false;
            }


            if (this.Author is not null)
            {
                if (!this.Author.Equals(embed.Author))
                {
                    return false;
                }
            }
            else if (embed.Author.HasValue)
            {
                return false;
            }

            if (this.Footer is not null)
            {
                if (!this.Footer.Equals(embed.Footer))
                {
                    return false;
                }
            }
            else if (embed.Footer.HasValue)
            {
                return false;
            }

            if (this.Provider is not null)
            {
                if (!this.Provider.Equals(embed.Provider))
                {
                    return false;
                }
            }
            else if (embed.Provider.HasValue)
            {
                return false;
            }

            if (this.Thumbnail is not null)
            {
                if (!this.Thumbnail.Equals(embed.Thumbnail))
                {
                    return false;
                }
            }
            else if (embed.Thumbnail.HasValue)
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc cref="Embed.Equals(Embed?)"/>
        public bool Equals([NotNullWhen(true)] EmbedShim? embed)
        {
            return embed is not null &&
                this.Type == embed.Type &&
                this.Title == embed.Title &&
                this.Description == embed.Description &&
                this.Timestamp == embed.Timestamp &&
                this.Color == embed.Color &&
                this.Image == embed.Image &&
                this.Video == embed.Video &&
                this.Author == embed.Author &&
                this.Footer == embed.Footer &&
                this.Provider == embed.Provider &&
                this.Thumbnail == embed.Thumbnail;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = (hash * 23) + (Type, Title, Description, Timestamp, Color, Image, Video, Author, Footer, Provider, Thumbnail).GetHashCode();
                foreach (var field in Fields)
                    hash = (hash * 23) + field.GetHashCode();
                return hash;
            }
        }

        protected sealed class EmbedFieldsCollection : ICollection<EmbedFieldShim>, ICollection<EmbedField>, IConvertibleShim<ImmutableArray<EmbedField>>
        {
            private readonly List<EmbedFieldShim> m_fields;
            private readonly EmbedShim m_embed;

            internal EmbedFieldsCollection(EmbedShim embed)
            {
                this.m_embed = embed;
                this.m_fields = new();
            }

            public void Apply(ImmutableArray<EmbedField> value)
            {
                this.m_fields.Clear();
                foreach (EmbedField field in value)
                {
                    EmbedFieldShim shim = this.m_embed.ShimField(field);
                    if (shim is not null)
                    {
                        this.m_fields.Add(shim);
                    }
                }
            }

            public ImmutableArray<EmbedField> UnShim()
            {
                return this.ToImmutableArray<EmbedField>();
            }

            public int Count => this.m_fields.Count;

            bool ICollection<EmbedField>.IsReadOnly => false;
            bool ICollection<EmbedFieldShim>.IsReadOnly => false;

            public void Add(EmbedFieldShim item)
            {
                Preconditions.NotNull(item, nameof(item));
                if (this.m_fields.Count >= EmbedBuilder.MaxFieldCount)
                {
                    throw new InvalidOperationException($"Cannot add field, as the embed has reached the maximum number of fields ({EmbedBuilder.MaxFieldCount})");
                }

                this.m_fields.Add(item);
            }
            public void Add(EmbedField item)
            {
                this.Add(this.m_embed.ShimField(item));
            }
            public void Clear()
            {
                this.m_fields.Clear();
            }
            public bool Contains(EmbedFieldShim item)
            {
                return this.m_fields.Exists(f => f.Equals(item));
            }
            public bool Contains(EmbedField item)
            {
                return this.m_fields.Exists(f => f.Equals(item));
            }
            void ICollection<EmbedFieldShim>.CopyTo(EmbedFieldShim[] array, int arrayIndex)
            {
                this.m_fields.CopyTo(array, arrayIndex);
            }
            void ICollection<EmbedField>.CopyTo(EmbedField[] array, int arrayIndex)
            {
                ((ICollection)this.m_fields).CopyTo(array, arrayIndex);
            }
            public IEnumerator<EmbedFieldShim> GetEnumerator()
            {
                return new Enumerator(this);
            }
            IEnumerator<EmbedField> IEnumerable<EmbedField>.GetEnumerator()
            {
                return new UnShimmedEnumerator(this);
            }
            public bool Remove(EmbedFieldShim item)
            {
                if (item is null)
                {
                    return false;
                }

                return this.m_fields.Remove(item);
            }
            public bool Remove(EmbedField item)
            {
                int index = this.m_fields.FindIndex(f => f.Equals(item));
                if (index == -1)
                {
                    return false;
                }
                this.m_fields.RemoveAt(index);
                return true;
            }
            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            private readonly struct Enumerator : IEnumerator<EmbedFieldShim>
            {
                private readonly List<EmbedFieldShim>.Enumerator m_enumerator;
                public Enumerator(EmbedFieldsCollection collection)
                {
                    this.m_enumerator = collection.m_fields.GetEnumerator();
                }

                public EmbedFieldShim Current
                {
                    get => this.m_enumerator.Current;
                }

                object? IEnumerator.Current
                {
                    get => this.Current;
                }

                public void Dispose()
                {
                    this.m_enumerator.Dispose();
                }

                public bool MoveNext()
                {
                    return this.m_enumerator.MoveNext();
                }

                void IEnumerator.Reset()
                {
                    ((IEnumerator)this.m_enumerator).Reset();
                }
            }

            private readonly struct UnShimmedEnumerator : IEnumerator<EmbedField>
            {
                private readonly List<EmbedFieldShim>.Enumerator m_enumerator;

                public UnShimmedEnumerator(EmbedFieldsCollection collection)
                {
                    this.m_enumerator = collection.m_fields.GetEnumerator();
                }

                public EmbedField Current
                {
                    get => this.m_enumerator.Current.UnShim();
                }

                object IEnumerator.Current
                {
                    get => this.Current;
                }

                public void Dispose()
                {
                    this.m_enumerator.Dispose();
                }
                public bool MoveNext()
                {
                    return this.m_enumerator.MoveNext();
                }
                void IEnumerator.Reset()
                {
                    ((IEnumerator)this.m_enumerator).Reset();
                }
            }
        }
    }
}
