using System;
using System.Collections.Generic;

namespace Discord.Shims.Bindables
{
    internal class Bindable<T>
    {
        private T m_value;
        private T m_defaultValue;
        private readonly ValueChangeListenerCollection m_defaultValueChange;
        private readonly ValueChangeListenerCollection m_valueChange;

        public Bindable() : this(default!)
        { }

        public Bindable(T defaultValue)
        {
            this.m_value = defaultValue;
            this.m_defaultValue = defaultValue;
            this.m_defaultValueChange = new();
            this.m_valueChange = new();
        }

        public event ValueChangeDelegate OnValueChange
        {
            add => this.m_valueChange.Add(value);
            remove => this.m_valueChange.Remove(value);
        }

        public event ValueChangeDelegate OnDefaultValueChange
        {
            add => this.m_valueChange.Add(value);
            remove => this.m_valueChange.Remove(value);
        }

        public T DefaultValue
        {
            get => this.m_defaultValue;
            set
            {
                if (EqualityComparer<T>.Default.Equals(this.m_defaultValue, value))
                {
                    return;
                }

                this.SetDefaultValue(value);
            }
        }

        public T Value
        {
            get => this.m_value;
            set
            {
                if (EqualityComparer<T>.Default.Equals(this.m_value, value))
                {
                    return;
                }

                this.SetValue(value);
            }
        }

        public void SetDefault(bool ignoreListeners = false)
        {
            this.SetValue(this.m_defaultValue, ignoreListeners);
        }

        public void SetDefaultValue(T value, bool ignoreListeners = false)
        {
            if (ignoreListeners)
            {
                this.m_defaultValue = value;
                return;
            }

            T oldValue = this.m_defaultValue;
            this.m_defaultValue = value;
            this.m_defaultValueChange.Invoke(oldValue, value);
        }

        public void SetValue(T value, bool ignoreListeners = false)
        {
            if (ignoreListeners)
            {
                this.m_value = value;
                return;
            }

            T oldValue = this.m_value;
            this.m_value = value;
            this.m_valueChange.Invoke(oldValue, value);
        }

        public delegate void ValueChangeDelegate(T oldValue, T newValue);


        private sealed class ValueChangeListenerCollection
        {
            private readonly List<ValueChangeDelegate> m_listeners = new();

            public void Add(ValueChangeDelegate listener)
            {
                if (listener == null)
                {
                    return;
                }
                this.m_listeners.Add(listener);
            }
            public void Remove(ValueChangeDelegate listener)
            {
                if (listener == null)
                {
                    return;
                }
                this.m_listeners.Remove(listener);
            }

            public void Invoke(T oldValue, T newValue)
            {
                List<Exception> exceptions = new();
                foreach (ValueChangeDelegate listener in this.m_listeners)
                {
                    try
                    {
                        listener.Invoke(oldValue, newValue);
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }

                if (exceptions.Count > 0)
                {
                    if (exceptions.Count == 1)
                    {
                        throw exceptions[0];
                    }
                    else
                    {
                        throw new AggregateException(exceptions.ToArray());
                    }
                }
            }

        }
    }
}
