using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace PingerLibrary
{
    public class PingsList<T> : IList<T>
    {
        private volatile List<Tuple<DateTime, T>> _collection = new List<Tuple<DateTime, T>>();

        public T this[int index]
        {
            get { return _collection[index].Item2; }
            set { _collection[index] = new Tuple<DateTime, T>(DateTime.Now, value); }
        }

        private Timer timer;

        public double Interval
        {
            get { return timer.Interval; }
            set { timer.Interval = value; }
        }

        private TimeSpan _expiration;

        public TimeSpan Expiration
        {
            get { return _expiration; }
            set { _expiration = value; }
        }

        public PingsList(double _interval, TimeSpan expiration)
        {
            timer = new Timer();
            timer.Interval = _interval;
            timer.Elapsed += Tick;
            timer.Start();

            _expiration = expiration;
        }

        private void Tick(object sender, EventArgs e)
        {
            for (int i = _collection.Count - 1; i >= 0; i--)
                if ((DateTime.Now - _collection[i].Item1) >= _expiration)
                    _collection.RemoveAt(i);
        }

        public int Count => _collection.Count();

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            _collection.Add(new Tuple<DateTime, T>(DateTime.Now, item));
        }

        public void Clear()
        {
            _collection.Clear();
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < _collection.Count; i++)
                if ((object)_collection[i].Item2 == (object)item)
                    return true;

            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            for (int i = 0; i < _collection.Count; i++)
                array[i + arrayIndex] = _collection[i].Item2;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _collection.Select(x => x.Item2).GetEnumerator();
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < _collection.Count; i++)
            {
                if ((object)_collection[i].Item2 == (object)item)
                    return i;
            }

            return -1;
        }

        public void Insert(int index, T item)
        {
            _collection.Insert(index, new Tuple<DateTime, T>(DateTime.Now, item));
        }

        public bool Remove(T item)
        {
            bool contained = Contains(item);
            for (int i = _collection.Count - 1; i >= 0; i--)
            {
                if ((object)_collection[i].Item2 == (object)item)
                    _collection.RemoveAt(i);
            }
            return contained;
        }

        public void RemoveAt(int index)
        {
            _collection.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _collection.Select(x => x.Item2).GetEnumerator();
        }
    }
}
