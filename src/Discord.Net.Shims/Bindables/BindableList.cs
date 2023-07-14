using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Discord.Shims.Bindables
{
    public class BindableList<T> : IList<T>, IList, IReadOnlyList<T>
    {
        private readonly List<T> m_list;
        private readonly ListModifyListenerCollection m_onAdd;
        private readonly ListModifyListenerCollection m_onRemove;
        private readonly EntryModifyListenerCollection m_onModify;
        private readonly ListClearListenerCollection m_onClear;

        public BindableList()
        {
            this.m_list = new();
            this.m_onAdd = new();
            this.m_onRemove = new();
            this.m_onModify = new();
            this.m_onClear = new();
        }

        public event ListModifyListenerDelegate OnItemAdd
        {
            add => this.m_onAdd.Add(value);
            remove => this.m_onAdd.Remove(value);
        }

        public event ListModifyListenerDelegate OnItemRemove
        {
            add => this.m_onRemove.Add(value);
            remove => this.m_onRemove.Remove(value);
        }

        public event EntryModifyListenerDelegate OnItemModified
        {
            add => this.m_onModify.Add(value);
            remove => this.m_onModify.Remove(value);
        }
        public event ListClearListenerDelegate OnCleared
        {
            add => this.m_onClear.Add(value);
            remove => this.m_onClear.Remove(value);
        }

        public readonly struct ItemInfo
        {
            public ItemInfo(T item, int index)
            {
                this.Item = item;
                this.Index = index;
            }

            public T Item { get; }
            public int Index { get; }
        }

        public delegate void ListModifyListenerDelegate(IEnumerable<ItemInfo> items);
        public delegate void EntryModifyListenerDelegate(T oldItem, T newItem, int index);
        public delegate void ListClearListenerDelegate(IEnumerable<T> items);

        private abstract class ListenerCollection<TDelegate>
            where TDelegate : Delegate
        {
            protected readonly List<TDelegate> m_listeners = new();

            public void Add(TDelegate listener)
            {
                if (listener == null)
                {
                    return;
                }
                this.m_listeners.Add(listener);
            }
            public void Remove(TDelegate listener)
            {
                if (listener == null)
                {
                    return;
                }
                this.m_listeners.Remove(listener);
            }

        }

        private sealed class ListModifyListenerCollection : ListenerCollection<ListModifyListenerDelegate>
        {
            public void Invoke(T item, int index) => this.Invoke(new ItemInfo(item, index));
            public void Invoke(ItemInfo item) => this.Invoke(ImmutableArray.Create(item));
            public void Invoke(IEnumerable<ItemInfo> items)
            {
                List<Exception> exceptions = new();
                foreach (ListModifyListenerDelegate listener in this.m_listeners)
                {
                    try
                    {
                        listener.Invoke(items);
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

        private sealed class EntryModifyListenerCollection : ListenerCollection<EntryModifyListenerDelegate>
        {
            public void Invoke(T oldItem, T newItem, int index)
            {
                List<Exception> exceptions = new();
                foreach (EntryModifyListenerDelegate listener in this.m_listeners)
                {
                    try
                    {
                        listener.Invoke(oldItem, newItem, index);
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

        private sealed class ListClearListenerCollection : ListenerCollection<ListClearListenerDelegate>
        {
            public void Invoke(IEnumerable<T> items)
            {
                List<Exception> exceptions = new();
                foreach (ListClearListenerDelegate listener in this.m_listeners)
                {
                    try
                    {
                        listener.Invoke(items);
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

        public T this[int index]
        {
            get => this.m_list[index];
            set
            {
                T old = this.m_list[index];
                this.m_list[index] = value;

                this.m_onModify.Invoke(old, value, index);
            }
        }

        object? IList.this[int index]
        {
            get => this[index];
            set
            {
                if (value == null && default(T) != null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                try
                {
                    this[index] = (T)value!;
                }
                catch (InvalidCastException)
                {
                    throw new ArgumentException($"Argument type {value!.GetType()} cannot be converted to {typeof(T)}", nameof(value));
                }
            }
        }

        public int Count
        {
            get => this.m_list.Count;
        }

        bool IList.IsFixedSize => false;

        bool IList.IsReadOnly => false;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => true;

        bool ICollection<T>.IsReadOnly => false;

        public void Add(T item)
        {
            this.m_list.Add(item);
            this.m_onAdd.Invoke(item, this.Count - 1);
        }
        public void AddRange(IEnumerable<T> items)
        {
            if (items is null)
            {
                return;
            }

            int index = this.Count;
            this.m_list.AddRange(items);
            ImmutableArray<ItemInfo>.Builder builder = ImmutableArray.CreateBuilder<ItemInfo>();
            foreach (T item in items)
            {
                builder.Add(new ItemInfo(item, index));
                index++;
            }
            this.m_onAdd.Invoke(builder.ToImmutable());
        }

        int IList.Add(object? value)
        {
            if (value == null && default(T) != null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            try
            {
                this.Add((T)value!);
                return this.Count - 1;
            }
            catch (InvalidCastException)
            {
                throw new ArgumentException($"Argument type {value!.GetType()} cannot be converted to {typeof(T)}", nameof(value));
            }
        }
        public void Clear()
        {
            ImmutableArray<T> items = this.m_list.ToImmutableArray();
            this.m_list.Clear();
            this.m_onClear.Invoke(items);
        }

        public bool Contains(T item)
        {
            return this.m_list.Contains(item);
        }
        bool IList.Contains(object? value)
        {
            if (value is not T tValue)
            {
                return false;
            }

            return this.Contains(tValue);
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.m_list.CopyTo(array, arrayIndex);
        }
        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)this.m_list).CopyTo(array, index);
        }

        public bool Exists(Predicate<T> predicate)
        {
            return this.m_list.Exists(predicate);
        }
        public T? Find(Predicate<T> predicate)
        {
            return this.m_list.Find(predicate);
        }
        public IReadOnlyCollection<T> FindAll(Predicate<T> predicate)
        {
            return this.m_list.FindAll(predicate).AsReadOnly();

        }
        public int FindIndex(Predicate<T> predicate)
        {
            return this.m_list.FindIndex(predicate);
        }
        public int FindIndex(int startIndex, Predicate<T> predicate)
        {
            return this.m_list.FindIndex(startIndex, predicate);
        }
        public int FindIndex(int startIndex, int count, Predicate<T> predicate)
        {
            return this.m_list.FindIndex(startIndex, count, predicate);
        }
        public T? FindLast(Predicate<T> predicate)
        {
            return this.m_list.FindLast(predicate);
        }
        public int FindLastIndex(Predicate<T> predicate)
        {
            return this.m_list.FindLastIndex(predicate);
        }
        public int FindLastIndex(int startIndex, Predicate<T> predicate)
        {
            return this.m_list.FindLastIndex(startIndex, predicate);
        }
        public int FindLastIndex(int startIndex, int count, Predicate<T> predicate)
        {
            return this.m_list.FindLastIndex(startIndex, count, predicate);
        }
        public IEnumerator<T> GetEnumerator()
        {
            return this.m_list.GetEnumerator();
        }
        public int IndexOf(T item)
        {
            return this.m_list.IndexOf(item);
        }
        int IList.IndexOf(object? value)
        {
            if (value is not T tValue)
            {
                return -1;
            }
            return this.IndexOf(tValue);
        }
        public void Insert(int index, T item)
        {
            this.m_list.Insert(index, item);
            this.m_onAdd.Invoke(item, index);
        }
        public void InsertRange(int index, IEnumerable<T> items)
        {
            if (items is null)
            {
                return;
            }

            this.m_list.InsertRange(index, items);
            ImmutableArray<ItemInfo>.Builder builder = ImmutableArray.CreateBuilder<ItemInfo>();
            foreach (T item in items)
            {
                builder.Add(new ItemInfo(item, index));
                index++;
            }
            this.m_onAdd.Invoke(builder.ToImmutable());
        }
        void IList.Insert(int index, object? value)
        {
            if (value == null && default(T) != null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            try
            {
                this.Insert(index, (T)value!);
            }
            catch (InvalidCastException)
            {
                throw new ArgumentException($"Argument type {value!.GetType()} cannot be converted to {typeof(T)}", nameof(value));
            }
        }
        public bool Remove(T item)
        {
            int index = this.IndexOf(item);
            if (index < 0)
            {
                return false;
            }
            this.m_list.RemoveAt(index);
            this.m_onRemove.Invoke(item, index);
            return true;
        }
        void IList.Remove(object? value)
        {
            if (value == null)
            {
                if (default(T) != null)
                {
                    return;
                }
                this.Remove(default(T)!);
            }
            else if (value is T tValue)
            {
                this.Remove(tValue);
            }
        }
        public void RemoveAt(int index)
        {
            T item = this[index];
            this.m_list.RemoveAt(index);
            this.m_onRemove.Invoke(item, index);
        }
        public void RemoveRange(int index, int count)
        {
            if (count <= 0)
            {
                return;
            }
            else if (index + count >= this.Count)
            {
                throw new ArgumentException("Given index & count exceeds the bounds of this list");
            }
            int relativeIndex = index;
            ImmutableArray<ItemInfo>.Builder builder = ImmutableArray.CreateBuilder<ItemInfo>();
            while (count > 0)
            {
                T item = this.m_list[index];
                this.m_list.RemoveAt(index);
                builder.Add(new ItemInfo(item, relativeIndex));
                relativeIndex++;
            }
            this.m_onRemove.Invoke(builder.ToImmutable());
        }
        public void RemoveAll(Predicate<T> predicate)
        {
            ImmutableArray<ItemInfo>.Builder builder = ImmutableArray.CreateBuilder<ItemInfo>();
            int index = 0;
            int relativeIndex = 0;
            while (index < this.Count)
            {
                T item = this.m_list[index];
                if (predicate.Invoke(item))
                {
                    this.m_list.RemoveAt(index);
                    builder.Add(new ItemInfo(item, relativeIndex));
                }
                else
                {
                    index++;
                }
                relativeIndex++;
            }

            this.m_onRemove.Invoke(builder.ToImmutable());
        }
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
