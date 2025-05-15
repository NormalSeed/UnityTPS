using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPattern
{
    public class ObjectPool
    {
        private Stack<PooledObject> _stack;
        private PooledObject _targetPrefab;
        private GameObject _poolObject;

        public ObjectPool(Transform parent, PooledObject targetPrefab, int initsize = 5) => Init(parent, targetPrefab, initsize);

        private void Init(Transform parent, PooledObject targetPrefab, int initsize)
        {
            _stack = new Stack<PooledObject>(initsize);
            _targetPrefab = targetPrefab;
            _poolObject = new GameObject($"{targetPrefab.name} Pool");
            _poolObject.transform.parent = parent;

            for (int i = 0; i < initsize; i++)
            {
                CreatePoolObject();
            }
        }

        public PooledObject PopPool()
        {
            if (_stack.Count == 0)
            {
                CreatePoolObject();
            }

            PooledObject pooledObject = _stack.Pop();
            pooledObject.gameObject.SetActive(true);
            return pooledObject;
        }

        public void PushPool(PooledObject target)
        {
            target.transform.parent = _poolObject.transform;
            target.gameObject.SetActive(false);
            _stack.Push(target);
        }

        private void CreatePoolObject()
        {
            PooledObject obj = MonoBehaviour.Instantiate(_targetPrefab);
            obj.PooledInit(this);
            PushPool(obj);
        }
    }
}

