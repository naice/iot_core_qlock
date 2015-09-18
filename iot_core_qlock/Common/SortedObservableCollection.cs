using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Collections;

namespace iot_core_qlock.Common
{
    public enum SortedObservableCollectionSortDirection : int
    {
        /// <summary>
        /// Ascended sorting.
        /// </summary>
        ASC = 1,
        /// <summary>
        /// Descended sorting.
        /// </summary>
        DESC = -1
    }
    public class SortedObservableCollection<T> : IEnumerable<T>, IEnumerable, IList, ICollection, INotifyCollectionChanged, INotifyPropertyChanged
    {
        public int Count { get { return data.Count; } }
        public delegate long GetSortIdDelegate(T item);
        public SortedObservableCollectionSortDirection SortDirection { get { return (data.Comparer as DuplicateKeyComparer).SortDirection; } }
        
        class DuplicateKeyComparer : IComparer<long> 
        {
            public SortedObservableCollectionSortDirection SortDirection { get; private set; }
            public DuplicateKeyComparer(SortedObservableCollectionSortDirection SortDirection)
            {
                this.SortDirection = SortDirection;
            }
            public int Compare(long x, long y)
            {
                int result = ((int)this.SortDirection) * x.CompareTo(y);
                if (result == 0)
                    return (int)this.SortDirection;
                else
                    return result;
            }
        }
        #region Notify implementations
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            if (CollectionChanged != null)
                CollectionChanged.Invoke(this, args);
        }
        protected void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        #endregion

        private SortedList<long, T> data = null;
        private GetSortIdDelegate GetSortID = null;

        public SortedObservableCollection(GetSortIdDelegate GetSortID)
            : this(GetSortID, SortedObservableCollectionSortDirection.ASC)
        {
                
        }
        public SortedObservableCollection(GetSortIdDelegate GetSortID, SortedObservableCollectionSortDirection direction)
        {
            this.GetSortID = GetSortID;
            data = new SortedList<long, T>(new DuplicateKeyComparer(direction));
        }

        public void Add(T item)
        {
            var iSort = GetSortID(item);
            data.Add(iSort, item);
            int index = data.IndexOfValue(item);

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
            OnPropertyChanged("Count");
        }

        public IEnumerator<T> GetEnumerator()
        {
            return data.Values.GetEnumerator();
        }


        public T this[int idx]
        {
            get
            {
                return data.Values[idx];
            }
            set
            {
                throw new InvalidOperationException("SortedObservableCollection does not expect Insert");
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return data.Values.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            throw new InvalidOperationException("SortedObservableCollection does not expect Insert");
        }

        public void RemoveAt(int index)
        {
            data.RemoveAt(index);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, index));
        }

        public void Clear()
        {
            data.Clear();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Contains(T item)
        {
            return data.ContainsValue(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new InvalidOperationException("SortedObservableCollection does not expect CopyTo");
        }

        public bool Remove(T item)
        {
            int idx = IndexOf(item);

            if (idx >= 0)
            {
                data.RemoveAt(IndexOf(item));
                return true;
            }

            return false;
        }

        public void CopyTo(Array array, int index)
        {
            throw new InvalidOperationException("SortedObservableCollection does not expect CopyTo");
        }

        #region IList & ICollection
        bool IList.IsFixedSize
        {
            get;
        } = false;

        bool IList.IsReadOnly
        {
            get;
        } = false;

        int ICollection.Count
        {
            get
            {
                return this.Count;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }

            set
            {
                throw new InvalidOperationException("SortedObservableCollection does not expect Insert");
            }
        }
        int IList.Add(object value)
        {
            throw new NotImplementedException();
        }

        void IList.Clear()
        {
            throw new NotImplementedException();
        }

        bool IList.Contains(object value)
        {
            throw new NotImplementedException();
        }

        int IList.IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        void IList.Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        void IList.Remove(object value)
        {
            throw new NotImplementedException();
        }

        void IList.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

