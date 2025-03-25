using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TDS
{
    class ObjectPoolManager
    {
        public class PoolEntry
        {
            public GameObject Prefab;
            public Stack<GameObject> Items = new Stack<GameObject>();
        }

        int mUniqueId;

        Dictionary<string, PoolEntry> mPathToPool;
        Dictionary<GameObject, PoolEntry> mActiveObjects;

        Transform mContainer;

        public ObjectPoolManager(Transform container)
        {
            mUniqueId = 100;
            mContainer = container;

            mPathToPool = new Dictionary<string, PoolEntry>();
            mActiveObjects = new Dictionary<GameObject, PoolEntry>();
        }

        public GameObject Acquire(string prefabPath, Transform parent = null)
        {
            // ������ ����ߴ� pool�� �ִ��� Ȯ��
            mPathToPool.TryGetValue(prefabPath, out PoolEntry pool);
            if (pool == null)
            {
                // ����ߴ� pool�� ������ ���Ӱ� ����
                GameObject prefabObj = Resources.Load(prefabPath) as GameObject;
                if (prefabObj == null)
                    return null;

                pool = new PoolEntry() { Prefab = prefabObj };

                mPathToPool.Add(prefabPath, pool);
            }

            return Acquire(pool, parent);
        }

        public GameObject Acquire(PoolEntry pool, Transform parent = null)
        {
            Debug.Assert(pool != null);

            GameObject go;

            // Ǯ�� ����ִ� ������Ʈ�� �ִٸ�
            if (pool.Items.Count > 0)
            {
                // �ٷ� ������ ���
                go = pool.Items.Pop();
            }
            else
            {
                // Ǯ�� ������ִ� ������Ʈ�� ���ٸ� ���� �����ؼ����
                go = GameObject.Instantiate<GameObject>(pool.Prefab, parent);

                go.name = $"{pool.Prefab.name}_{mUniqueId++}";
            }

            go.transform.SetParent(parent, false);

            go.SetActive(true);

            // ������Ʈ�� Ȱ��ȭ�ϰ� � Ǯ���� ���Դ��� ���
            mActiveObjects[go] = pool;

            return go;
        }

        public void Release(GameObject go)
        {
            mActiveObjects.TryGetValue(go, out PoolEntry pool);
            if (pool != null)
            {
                // �ݳ�
                go.transform.SetParent(mContainer, false);

                // ��Ȱ��ȭ
                go.SetActive(false);

                // Ǯ�� �ٽ� ��� ����
                pool.Items.Push(go);

                // Ȱ��ȭ ��Ͽ��� ����
                mActiveObjects.Remove(go);
            }
            else
            {
                Debug.LogError($"Ȱ��ȭ �ȵ� ������Ʈ�� Release�Ϸ��� �õ���. ({go.name})");
            }
        }

        public void DestroyAllObjects()
        {
            foreach (PoolEntry pool in mPathToPool.Values)
            {
                foreach (var go in pool.Items)
                {
                    if (go != null)
                        GameObject.Destroy(go);
                }

                pool.Items.Clear();
            }

            foreach (var item in mActiveObjects)
            {
                if (item.Key != null)
                    GameObject.Destroy(item.Key);
            }

            mPathToPool.Clear();
            mActiveObjects.Clear();
        }
    }
}