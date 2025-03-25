using UnityEngine;

namespace TDS
{
    class GameInitializer : MonoBehaviour
    {
        private void Awake()
        {
            TDS.Init(gameObject);
        }
    }

    class TDS
    {
        public static ObjectPool ObjectPool;
        public static GameManager GameManager;

        public static void Init(GameObject go)
        {
            ObjectPool = new ObjectPool();

            GameManager = go.AddComponent<GameManager>();
            GameManager.Init();
        }
    }
}