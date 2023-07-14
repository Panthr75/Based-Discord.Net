using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="MessageComponent"/>
    /// </summary>
    public class MessageComponentShim
    {
        /// <summary>
        ///     Gets the components to be used in a message.
        /// </summary>
        public IReadOnlyCollection<ActionRowComponent> Components { get; }

        internal MessageComponent(List<ActionRowComponent> components)
        {
            Components = components;
        }

        protected sealed class ActionRowComponentCollection : ICollection<ActionRowComponentShim>, ICollection<ActionRowComponent>, IConvertibleShim<IReadOnlyCollection<ActionRowComponent>>
        {
            private readonly List<ActionRowComponentShim> m_components;
            private readonly MessageComponentShim m_component;

            internal ActionRowComponentCollection(MessageComponentShim component)
            {
                this.m_component = component;
                this.m_components = new();
            }

            public void Apply(IReadOnlyCollection<ActionRowComponent> value)
            {
                if (value is null)
                {
                    return;
                }

                this.m_components.Clear();
                foreach (ActionRowComponent component in value)
                {
                    ActionRowComponentShim shim = this.m_component.ShimComponent(component);
                    if (shim is not null)
                    {
                        this.m_components.Add(shim);
                    }
                }
            }

            public IReadOnlyCollection<ActionRowComponent> UnShim()
            {
                return this.ToImmutableArray<ActionRowComponent>();
            }

            public int Count => this.m_components.Count;

            bool ICollection<ActionRowComponent>.IsReadOnly => false;
            bool ICollection<ActionRowComponentShim>.IsReadOnly => false;

            public void Add(ActionRowComponentShim item)
            {
                Preconditions.NotNull(item, nameof(item));
                if (this.m_components.Count >= ComponentBuilder.MaxActionRowCount)
                {
                    throw new InvalidOperationException($"Cannot add row, as the message component has exceeded the maximum action rows ({ComponentBuilder.MaxActionRowCount})");
                }

                this.m_components.Add(item);
            }
            public void Add(ActionRowComponent item)
            {
                this.Add(this.m_component.ShimComponent(item));
            }
            public void Clear()
            {
                this.m_components.Clear();
            }
            public bool Contains([NotNullWhen(true)] ActionRowComponentShim? item)
            {
                if (item is null)
                {
                    return false;
                }

                return this.m_components.Exists(f => f.Id == item.Id);
            }
            public bool Contains([NotNullWhen(true)] ActionRowComponent? item)
            {
                if (item is null)
                {
                    return false;
                }

                return this.m_components.Exists(f => f.Id == item.Id);
            }
            void ICollection<ActionRowComponentShim>.CopyTo(ActionRowComponentShim[] array, int arrayIndex)
            {
                this.m_components.CopyTo(array, arrayIndex);
            }
            void ICollection<ActionRowComponent>.CopyTo(ActionRowComponent[] array, int arrayIndex)
            {
                ((ICollection)this.m_components).CopyTo(array, arrayIndex);
            }
            public IEnumerator<ActionRowComponentShim> GetEnumerator()
            {
                return new Enumerator(this);
            }
            IEnumerator<ActionRowComponent> IEnumerable<ActionRowComponent>.GetEnumerator()
            {
                return new UnShimmedEnumerator(this);
            }
            public bool Remove([NotNullWhen(true)] ActionRowComponentShim? item)
            {
                if (item is null)
                {
                    return false;
                }

                int index = this.m_components.FindIndex(f => f.Id == item.Id);
                if (index == -1)
                {
                    return false;
                }
                this.m_components.RemoveAt(index);
                return true;
            }
            public bool Remove([NotNullWhen(true)] ActionRowComponent? item)
            {
                if (item is null)
                {
                    return false;
                }


                int index = this.m_components.FindIndex(f => f.Id == item.Id);
                if (index == -1)
                {
                    return false;
                }
                this.m_components.RemoveAt(index);
                return true;
            }
            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            private readonly struct Enumerator : IEnumerator<ActionRowComponentShim>
            {
                private readonly List<ActionRowComponentShim>.Enumerator m_enumerator;
                public Enumerator(ActionRowComponentCollection collection)
                {
                    this.m_enumerator = collection.m_components.GetEnumerator();
                }

                public ActionRowComponentShim Current
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

            private readonly struct UnShimmedEnumerator : IEnumerator<ActionRowComponent>
            {
                private readonly List<ActionRowComponentShim>.Enumerator m_enumerator;

                public UnShimmedEnumerator(ActionRowComponentCollection collection)
                {
                    this.m_enumerator = collection.m_components.GetEnumerator();
                }

                public ActionRowComponent Current
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
