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
    /// A shim of <see cref="ActionRowComponent"/>
    /// </summary>
    public class ActionRowComponentShim : IConvertibleShim<ActionRowComponent>, IMessageComponentShim, IEquatable<ActionRowComponentShim>, IEquatable<ActionRowComponent>
    {
        private readonly InnerComponentCollection m_components;

        public ActionRowComponentShim()
        {
            this.m_components = new(this);
        }
        public ActionRowComponentShim(ActionRowComponent component) : this()
        {
            Preconditions.NotNull(component, nameof(component));

            this.Apply(component);
        }

        public ActionRowComponent UnShim()
        {
            return new ActionRowComponent(this.ComponentCollection.UnShim());
        }

        IMessageComponent IConvertibleShim<IMessageComponent>.UnShim()
        {
            return this.UnShim();
        }

        void IConvertibleShim<IMessageComponent>.Apply(IMessageComponent value)
        {
            if (value is not ActionRowComponent component)
            {
                return;
            }
            this.Apply(component);
        }

        public void Apply(ActionRowComponent value)
        {
            if (value is null)
            {
                return;
            }

            this.m_components.Apply(value.Components);
        }

        protected InnerComponentCollection ComponentCollection => this.m_components;

        public virtual ICollection<IMessageComponentShim> Components => this.ComponentCollection;

        bool IEquatable<IMessageComponentShim>.Equals(IMessageComponentShim? other)
        {
            return other is ActionRowComponentShim component && this.Equals(component);
        }

        bool IEquatable<IMessageComponent>.Equals(IMessageComponent? other)
        {
            return other is ActionRowComponent component && this.Equals(component);
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is ActionRowComponentShim shim)
            {
                return this.Equals(shim);
            }
            else if (obj is ActionRowComponent other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public override int GetHashCode() => base.GetHashCode();

        public bool Equals([NotNullWhen(true)] ActionRowComponent? other)
        {
            if (other is null)
            {
                return false;
            }

            return this.ComponentCollection.Equals(other.Components);
        }
        public bool Equals([NotNullWhen(true)] ActionRowComponentShim? other)
        {
            if (other is null)
            {
                return false;
            }

            return this.ComponentCollection.Equals(other.Components);
        }
        private IMessageComponentShim? ShimComponent(IMessageComponent? component)
        {
            if (component is ButtonComponent button)
            {
                return this.ShimButton(button);
            }
            else if (component is SelectMenuComponent selectMenu)
            {
                return this.ShimSelectMenu(selectMenu);
            }
            else if (component is TextInputComponent textInput)
            {
                return this.ShimTextInput(textInput);
            }
            else
            {
                return null;
            }
        }

        [return: NotNullIfNotNull(nameof(component))]
        protected virtual ButtonComponentShim? ShimButton(ButtonComponent? component)
        {
            if (component == null)
            {
                return null;
            }

            return new ButtonComponentShim(component);
        }

        [return: NotNullIfNotNull(nameof(component))]
        protected virtual SelectMenuComponentShim? ShimSelectMenu(SelectMenuComponent? component)
        {
            if (component == null)
            {
                return null;
            }

            return new SelectMenuComponentShim(component);
        }

        [return: NotNullIfNotNull(nameof(component))]
        protected virtual TextInputComponentShim? ShimTextInput(TextInputComponent? component)
        {
            if (component == null)
            {
                return null;
            }

            return new TextInputComponentShim(component);
        }

        ComponentType IMessageComponent.Type => ComponentType.ActionRow;
        string? IMessageComponent.CustomId => null;

        protected sealed class InnerComponentCollection : ICollection<IMessageComponentShim>, ICollection<IMessageComponent>, IConvertibleShim<List<IMessageComponent>>, IEquatable<IReadOnlyCollection<IMessageComponent>>, IEquatable<IReadOnlyCollection<IMessageComponentShim>>
        {
            private readonly List<IMessageComponentShim> m_components;
            private readonly ActionRowComponentShim m_component;

            internal InnerComponentCollection(ActionRowComponentShim component)
            {
                this.m_component = component;
                this.m_components = new();
            }

            public override bool Equals([NotNullWhen(true)] object? obj)
            {
                if (obj is IReadOnlyCollection<IMessageComponentShim> shim)
                {
                    return this.Equals(shim);
                }
                else if (obj is IReadOnlyCollection<IMessageComponent> other)
                {
                    return this.Equals(other);
                }
                return false;
            }
            public override int GetHashCode() => base.GetHashCode();

            public bool Equals([NotNullWhen(true)] IReadOnlyCollection<IMessageComponentShim>? other)
            {
                if (other is null || this.Count != other.Count)
                {
                    return false;
                }

                IEnumerator<IMessageComponentShim> otherEnumerator = other.GetEnumerator();
                foreach (IMessageComponentShim component in this)
                {
                    if (!otherEnumerator.MoveNext() ||
                        !component.Equals(otherEnumerator.Current))
                    {
                        otherEnumerator.Dispose();
                        return false;
                    }
                }
                otherEnumerator.Dispose();
                return true;
            }

            public bool Equals([NotNullWhen(true)] IReadOnlyCollection<IMessageComponent>? other)
            {
                if (other is null || this.Count != other.Count)
                {
                    return false;
                }

                IEnumerator<IMessageComponent> otherEnumerator = other.GetEnumerator();
                foreach (IMessageComponentShim component in this)
                {
                    if (!otherEnumerator.MoveNext() ||
                        !component.Equals(otherEnumerator.Current))
                    {
                        otherEnumerator.Dispose();
                        return false;
                    }
                }
                otherEnumerator.Dispose();
                return true;
            }

            public void Apply(List<IMessageComponent> value)
            {
                if (value is null)
                {
                    return;
                }

                this.Apply((IReadOnlyCollection<IMessageComponent>)value);
            }

            public void Apply(IReadOnlyCollection<IMessageComponent> value)
            {
                if (value is null)
                {
                    return;
                }

                this.m_components.Clear();
                foreach (IMessageComponent component in value)
                {
                    IMessageComponentShim? shim = this.m_component.ShimComponent(component);

                    if (shim != null)
                    {
                        this.m_components.Add(shim);
                    }
                }
            }

            public List<IMessageComponent> UnShim()
            {
                return this.ToList<IMessageComponent>();
            }

            public int Count => this.m_components.Count;

            bool ICollection<IMessageComponent>.IsReadOnly => false;
            bool ICollection<IMessageComponentShim>.IsReadOnly => false;

            public void Add(IMessageComponentShim item)
            {
                Preconditions.NotNull(item, nameof(item));
                if (item is ActionRowComponentShim)
                {
                    throw new ArgumentException("Cannot add action row to action rows", nameof(item));
                }

                if (this.m_components.Count >= ActionRowBuilder.MaxChildCount)
                {
                    throw new InvalidOperationException($"Cannot add component, as the action row has exceeded the maximum components ({ActionRowBuilder.MaxChildCount})");
                }

                this.m_components.Add(item);
            }
            public void Add(IMessageComponent item)
            {
                Preconditions.NotNull(item, nameof(item));
                if (item is ActionRowComponent)
                {
                    throw new ArgumentException("Cannot add action row to action rows", nameof(item));
                }

                IMessageComponentShim? shim = this.m_component.ShimComponent(item);
                if (shim == null)
                {
                    throw new ArgumentException($"Cannot shim component of type {item.GetType()}", nameof(item));
                }

                this.Add(shim);
            }
            public void Clear()
            {
                this.m_components.Clear();
            }
            public bool Contains([NotNullWhen(true)] IMessageComponentShim? item)
            {
                if (item is null)
                {
                    return false;
                }

                return this.m_components.Exists(f => f.Equals(item));
            }
            public bool Contains([NotNullWhen(true)] IMessageComponent? item)
            {
                if (item is null)
                {
                    return false;
                }

                return this.m_components.Exists(f => f.Equals(item));
            }
            void ICollection<IMessageComponentShim>.CopyTo(IMessageComponentShim[] array, int arrayIndex)
            {
                this.m_components.CopyTo(array, arrayIndex);
            }
            void ICollection<IMessageComponent>.CopyTo(IMessageComponent[] array, int arrayIndex)
            {
                ((ICollection)this.m_components).CopyTo(array, arrayIndex);
            }
            public IEnumerator<IMessageComponentShim> GetEnumerator()
            {
                return new Enumerator(this);
            }
            IEnumerator<IMessageComponent> IEnumerable<IMessageComponent>.GetEnumerator()
            {
                return new UnShimmedEnumerator(this);
            }
            public bool Remove([NotNullWhen(true)] IMessageComponentShim? item)
            {
                if (item is null)
                {
                    return false;
                }

                int index = this.m_components.FindIndex(f => f.Equals(item));
                if (index == -1)
                {
                    return false;
                }
                this.m_components.RemoveAt(index);
                return true;
            }
            public bool Remove([NotNullWhen(true)] IMessageComponent? item)
            {
                if (item is null)
                {
                    return false;
                }


                int index = this.m_components.FindIndex(f => f.Equals(item));
                if (index == -1)
                {
                    return false;
                }
                this.m_components.RemoveAt(index);
                return true;
            }
            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            private readonly struct Enumerator : IEnumerator<IMessageComponentShim>
            {
                private readonly List<IMessageComponentShim>.Enumerator m_enumerator;
                public Enumerator(InnerComponentCollection collection)
                {
                    this.m_enumerator = collection.m_components.GetEnumerator();
                }

                public IMessageComponentShim Current
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

            private readonly struct UnShimmedEnumerator : IEnumerator<IMessageComponent>
            {
                private readonly List<IMessageComponentShim>.Enumerator m_enumerator;

                public UnShimmedEnumerator(InnerComponentCollection collection)
                {
                    this.m_enumerator = collection.m_components.GetEnumerator();
                }

                public IMessageComponent Current
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
