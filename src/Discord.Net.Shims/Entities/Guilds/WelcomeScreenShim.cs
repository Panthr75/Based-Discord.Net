using System;
using System.Collections.Immutable;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Collections;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="WelcomeScreen"/>
    /// </summary>
    public class WelcomeScreenShim : IConvertibleShim<WelcomeScreen>
    {
        private string? m_description;
        private readonly WelcomeScreenChannelsCollection m_channels;

        public WelcomeScreenShim()
        {
            this.m_channels = new(this);
        }

        public WelcomeScreenShim(WelcomeScreen welcomeScreen) : this()
        {
            Preconditions.NotNull(welcomeScreen, nameof(welcomeScreen));

            this.Apply(welcomeScreen);
        }

        protected WelcomeScreenChannelsCollection ChannelCollection => this.m_channels;

        /// <inheritdoc cref="WelcomeScreen.Description"/>
        public virtual string? Description
        {
            get => this.m_description;
            set => this.m_description = value;
        }

        /// <inheritdoc cref="WelcomeScreen.Channels"/>
        public virtual ICollection<WelcomeScreenChannelShim> Channels => this.ChannelCollection;


        [return: NotNullIfNotNull(nameof(channel))]
        protected virtual WelcomeScreenChannelShim? ShimChannel(WelcomeScreenChannel? channel)
        {
            if (channel is null)
            {
                return null;
            }
            return new WelcomeScreenChannelShim(channel);
        }

        public WelcomeScreen UnShim()
        {
            return new WelcomeScreen(this.Description, this.ChannelCollection.UnShim());
        }

        public void Apply(WelcomeScreen value)
        {
            if (value is null)
            {
                return;
            }

            this.m_description = value.Description;
            this.m_channels.Apply(value.Channels);
        }

        public static implicit operator WelcomeScreen(WelcomeScreenShim v)
        {
            return v.UnShim();
        }

        protected sealed class WelcomeScreenChannelsCollection : ICollection<WelcomeScreenChannelShim>, ICollection<WelcomeScreenChannel>, IConvertibleShim<IReadOnlyCollection<WelcomeScreenChannel>>
        {
            private readonly List<WelcomeScreenChannelShim> m_channels;
            private readonly WelcomeScreenShim m_welcomeScreen;

            internal WelcomeScreenChannelsCollection(WelcomeScreenShim welcomeScreen)
            {
                this.m_welcomeScreen = welcomeScreen;
                this.m_channels = new();
            }

            public void Apply(IReadOnlyCollection<WelcomeScreenChannel> value)
            {
                if (value is null)
                {
                    return;
                }

                this.m_channels.Clear();
                foreach (WelcomeScreenChannel channel in value)
                {
                    WelcomeScreenChannelShim shim = this.m_welcomeScreen.ShimChannel(channel);
                    if (shim is not null)
                    {
                        this.m_channels.Add(shim);
                    }
                }
            }

            public IReadOnlyCollection<WelcomeScreenChannel> UnShim()
            {
                return this.ToImmutableArray<WelcomeScreenChannel>();
            }

            public int Count => this.m_channels.Count;

            bool ICollection<WelcomeScreenChannel>.IsReadOnly => false;
            bool ICollection<WelcomeScreenChannelShim>.IsReadOnly => false;

            public void Add(WelcomeScreenChannelShim item)
            {
                Preconditions.NotNull(item, nameof(item));
                if (this.m_channels.Count >= 5)
                {
                    throw new InvalidOperationException("Cannot add channel, as the welcome screen has exceeded the maximum channels (5)");
                }

                this.m_channels.Add(item);
            }
            public void Add(WelcomeScreenChannel item)
            {
                this.Add(this.m_welcomeScreen.ShimChannel(item));
            }
            public void Clear()
            {
                this.m_channels.Clear();
            }
            public bool Contains([NotNullWhen(true)] WelcomeScreenChannelShim? item)
            {
                if (item is null)
                {
                    return false;
                }

                return this.m_channels.Exists(f => f.Id == item.Id);
            }
            public bool Contains([NotNullWhen(true)] WelcomeScreenChannel? item)
            {
                if (item is null)
                {
                    return false;
                }

                return this.m_channels.Exists(f => f.Id == item.Id);
            }
            void ICollection<WelcomeScreenChannelShim>.CopyTo(WelcomeScreenChannelShim[] array, int arrayIndex)
            {
                this.m_channels.CopyTo(array, arrayIndex);
            }
            void ICollection<WelcomeScreenChannel>.CopyTo(WelcomeScreenChannel[] array, int arrayIndex)
            {
                ((ICollection)this.m_channels).CopyTo(array, arrayIndex);
            }
            public IEnumerator<WelcomeScreenChannelShim> GetEnumerator()
            {
                return new Enumerator(this);
            }
            IEnumerator<WelcomeScreenChannel> IEnumerable<WelcomeScreenChannel>.GetEnumerator()
            {
                return new UnShimmedEnumerator(this);
            }
            public bool Remove([NotNullWhen(true)] WelcomeScreenChannelShim? item)
            {
                if (item is null)
                {
                    return false;
                }

                int index = this.m_channels.FindIndex(f => f.Id == item.Id);
                if (index == -1)
                {
                    return false;
                }
                this.m_channels.RemoveAt(index);
                return true;
            }
            public bool Remove([NotNullWhen(true)] WelcomeScreenChannel? item)
            {
                if (item is null)
                {
                    return false;
                }


                int index = this.m_channels.FindIndex(f => f.Id == item.Id);
                if (index == -1)
                {
                    return false;
                }
                this.m_channels.RemoveAt(index);
                return true;
            }
            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            private readonly struct Enumerator : IEnumerator<WelcomeScreenChannelShim>
            {
                private readonly List<WelcomeScreenChannelShim>.Enumerator m_enumerator;
                public Enumerator(WelcomeScreenChannelsCollection collection)
                {
                    this.m_enumerator = collection.m_channels.GetEnumerator();
                }

                public WelcomeScreenChannelShim Current
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

            private readonly struct UnShimmedEnumerator : IEnumerator<WelcomeScreenChannel>
            {
                private readonly List<WelcomeScreenChannelShim>.Enumerator m_enumerator;

                public UnShimmedEnumerator(WelcomeScreenChannelsCollection collection)
                {
                    this.m_enumerator = collection.m_channels.GetEnumerator();
                }

                public WelcomeScreenChannel Current
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
