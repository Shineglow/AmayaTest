using System;
using System.Collections.Generic;

namespace AmayaTest.Scripts.General
{
    public class ObjectsPool<T> where T : UnityEngine.Object
    {
        private readonly Stack<T> _free;
        private readonly HashSet<T> _inUse;
        private readonly Func<T> _makeObjectFunc;
        private readonly Action<T> _onFreeFunc;

        public ObjectsPool(int defaultObjectsCount, Func<T> makeObjectFunc, Action<T> onFreeFunc)
        {
            _free = new Stack<T>(defaultObjectsCount);
            _inUse = new HashSet<T>();
            _makeObjectFunc = makeObjectFunc;
            _onFreeFunc = onFreeFunc;
            
            for (int i = 0; i < defaultObjectsCount; i++)
            {
                _free.Push(SafeMakeItem());
            }
        }
        
        public T GetObject()
        {
            T item = _free.Count == 0 ? SafeMakeItem() : _free.Pop();
            _inUse.Add(item);
            return item;
        }

        private T SafeMakeItem()
        {
            T obj;
            
            obj = _makeObjectFunc?.Invoke();
            if (obj == null)
            {
                throw new InvalidOperationException("The function that creates instances returns invalid objects");
            }

            return obj;
        }

        public List<T> GetObjects(int count)
        {
            List<T> result = new List<T>(count);

            while (count > _free.Count)
            {
                _free.Push(SafeMakeItem());
            }

            while (_free.Count > 0)
            {
                T item = _free.Pop();
                _inUse.Add(item);
                result.Add(item);
            }

            return result;
        }

        public void FreeItem(T item)
        {
            _inUse.Remove(item);
            _free.Push(item);
        }

        public void FreeAllForce()
        {
            foreach (var item in _inUse)
            {
                _onFreeFunc(item);
                _free.Push(item);
            }
            
            _inUse.Clear();
        }
    }
}