using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MyTools.Pooling.Base
{
    public class Pool<T> where T : class
    {
        private class PoolNode
        {
            public T item;
            public PoolNode prev = null;
            public PoolNode next = null;
            public PoolNode(T item) { this.item = item; }
        }

        HashSet<T> m_InactiveObjsSet = new HashSet<T>();
        Stack<PoolNode> m_InactiveObjs;
        Dictionary<T, PoolNode> m_ActiveObjsDict;
        PoolNode m_FirstNode = null;
        PoolNode m_LastNode = null;

        public event Action<T> OnActivation = delegate { };
        public event Action<T> OnDeactivation = delegate { };

        public int InactiveCount { get { return m_InactiveObjs.Count; } }
        public int ActiveCount { get { return m_ActiveObjsDict.Count; } }
        public int TotalCount { get { return m_InactiveObjs.Count + m_ActiveObjsDict.Count; } }

        public Pool()
        {
            m_InactiveObjs = new Stack<PoolNode>();
            m_ActiveObjsDict = new Dictionary<T, PoolNode>();
        }
        public Pool(int capacity)
        {
            m_InactiveObjs = new Stack<PoolNode>(capacity);
            m_ActiveObjsDict = new Dictionary<T, PoolNode>(capacity);
        }
        public Pool(IEnumerable<T> collection)
        {
            using (var iter = collection.GetEnumerator())
            {
                m_InactiveObjs = new Stack<PoolNode>();
                T cur;
                while (iter.MoveNext())
                {
                    cur = iter.Current;
                    if (cur == null) continue;
                    m_InactiveObjs.Push(new PoolNode(cur));
                    m_InactiveObjsSet.Add(cur);
                }
                m_ActiveObjsDict = new Dictionary<T, PoolNode>(m_InactiveObjs.Count);
            }
        }

        public T TakeFromInactiveObjs()
        {
            if (m_InactiveObjs.Count < 1) return null;
            var node = m_InactiveObjs.Pop();
            if (node.item == null) throw new NullReferenceException();

            var first = m_FirstNode;
            var last = m_LastNode;

            m_InactiveObjsSet.Remove(node.item);
            m_ActiveObjsDict.Add(node.item, node);
            if (first == null)
                first = node;
            if (last != null)
            {
                last.next = node;
                node.prev = last;
            }
            last = node;

            m_FirstNode = first;
            m_LastNode = last;

            OnActivation(node.item);
            return node.item;
        }
        public T TakeFromActiveObjs()
        {
            var first = m_FirstNode;
            var last = m_LastNode;

            if (first == null) return null;
            var node = first;
            if (node.item == null) throw new NullReferenceException();
            OnDeactivation(node.item);

            if (first == last)
            {
                OnActivation(node.item);
                return node.item;
            }
            node.next.prev = null;
            first = node.next;
            node.next = null;
            last.next = node;
            node.prev = last;
            last = node;

            m_FirstNode = first;
            m_LastNode = last;

            OnActivation(node.item);
            return node.item;
        }
        public bool ReturnToPool(T obj)
        {
            if (obj == null) return false;
            if (!m_ActiveObjsDict.TryGetValue(obj, out var node)) return false;
            if (node.item == null) throw new NullReferenceException();
            OnDeactivation(node.item);

            if (node.prev == null) m_FirstNode = node.next;
            else { node.prev.next = node.next; node.prev = null; }

            if (node.next == null) m_LastNode = node.prev;
            else { node.next.prev = node.prev; node.next = null; }

            m_ActiveObjsDict.Remove(node.item);
            m_InactiveObjsSet.Add(node.item);
            m_InactiveObjs.Push(node);
            return true;
        }

        public void Add(T obj)
        {
            if (obj == null) return;
            if (Contains(obj)) return;
            m_InactiveObjs.Push(new PoolNode(obj));
            m_InactiveObjsSet.Add(obj);
        }

        public bool Contains(T obj)
        {
            if (obj == null) return false;
            return m_InactiveObjsSet.Contains(obj) || m_ActiveObjsDict.ContainsKey(obj);
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var node in m_InactiveObjs)
                yield return node.item;
            PoolNode curNode = m_FirstNode;
            while (curNode != null)
            {
                yield return curNode.item;
                curNode = curNode.next;
            }
        }
    }
}
