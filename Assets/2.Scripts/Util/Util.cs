using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TDS
{
    // 작업 편의성을 위한 간단한 유틸
    static class Util
    {
        public static GameObject Find(this GameObject go, string name, bool includeinactive = false)
        {
            if (go != null)
            {
                var tmList = go.GetComponentsInChildren<Transform>(includeinactive);
                foreach (var tm in tmList)
                {
                    if (tm.name == name)
                        return tm.gameObject;
                }
            }

            return null;
        }

        public static GameObject FindGameObject(this GameObject go, string name, bool ignoreAssert = false)
        {
            GameObject find = go.Find(name, true);

            if (ignoreAssert == false)
                Debug.Assert(find != null, $"{go.name}의 {name} GameObject가 존재하지 않음");

            return find;
        }

        public static T FindComponent<T>(this GameObject go, string name, bool ignoreAssert = false)
        {
            GameObject componentObj = go.FindGameObject(name, ignoreAssert);

            T component = componentObj.GetComponent<T>();

            if (ignoreAssert == false)
            {
                Debug.Assert(component != null, $"{go.name}의 {typeof(T).Name} {name}가 존재하지 않음");
            }

            return component;
        }

        public static T GetOrAddComponent<T>(this Component comp) where T : Component
        {
            T getComp = comp.GetComponent<T>();
            if (getComp == null)
            {
                getComp = comp.gameObject.AddComponent<T>();
            }

            return getComp;
        }

        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            T comp = go.GetComponent<T>();
            if (comp == null)
            {
                comp = go.AddComponent<T>();
            }

            return comp;
        }

        public static TValue TryGet<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key)
        {
            TValue value;
            dic.TryGetValue(key, out value);

            return value;
        }

        public static Vector3 WorldToScreenPoint(Vector3 worldPos)
        {
            return Camera.main.WorldToScreenPoint(worldPos);
        }

        public static bool Dice(float prob = 0.5f)
        {
            var probs = new float[2] { prob, 1 - prob };

            int idx = RandomChoose(probs);

            return idx == 0;
        }

        public static int RandomChoose(float[] probs)
        {
            float total = 0;

            foreach (float elem in probs)
            {
                total += elem;
            }

            float randomPoint = UnityEngine.Random.value * total;

            for (int i = 0; i < probs.Length; i++)
            {
                if (randomPoint < probs[i])
                {
                    return i;
                }
                else
                {
                    randomPoint -= probs[i];
                }
            }
            return probs.Length - 1;
        }

        public static T RandomSelect<T>(T[] values)
        {
            return values[UnityEngine.Random.Range(0, values.Length)]; ;
        }
    }
}