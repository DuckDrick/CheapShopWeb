using System;
using System.Collections.Concurrent;
using System.Linq;

namespace CheapShopWeb.Scrapers
{
    public class CustomList<T>
    {
        private readonly ConcurrentQueue<T> _searchQueue;

        public CustomList()
        {
            _searchQueue = new ConcurrentQueue<T>();
        }

        public void Add(T item)
        {
            var length = _searchQueue.Count;
            if (!_searchQueue.Contains(item))
                _searchQueue.Enqueue(item);

            if (length == 0 && _searchQueue.Count > 0)
            {
                OnBecomingNonEmpty(EventArgs.Empty);
            }
        }

        public void Remove()
        {
            var tries = 0;
            for (; tries < 5 && !_searchQueue.TryDequeue(out _); tries++) {}
            if(_searchQueue.Count == 0) OnBecomingEmpty(EventArgs.Empty);
        }

        public T Peek()
        {
            T item = default;
            while (!_searchQueue.TryPeek(out item) && !_searchQueue.IsEmpty) { }
            return item;
        }

        public int Size()
        {
            return _searchQueue.Count;
        }
        public bool IsEmpty()
        {
            return _searchQueue.IsEmpty;
        }

        protected virtual void OnBecomingNonEmpty(EventArgs e)
        {
            BecameNonEmpty?.Invoke(this, e);
        }

        protected virtual void OnBecomingEmpty(EventArgs e)
        {
            BecameEmpty?.Invoke(this, e);
        }

        public event EventHandler BecameNonEmpty;
        public event EventHandler BecameEmpty;
    }
}