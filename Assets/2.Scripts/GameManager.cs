using UnityEngine;
using System.Collections;

namespace TDS
{
    class GameManager : MonoBehaviour
    {
        Hero mHero;
        ZombieManager mZombieManager;

        RectTransform mDamageParent;
        public Hero Hero => mHero;
        public RectTransform DamageParent => mDamageParent;
        public void Init()
        {
            mDamageParent = GameObject.Find("DamageTextParent").GetComponent<RectTransform>();

            mZombieManager = new ZombieManager();
            mZombieManager.Init();

            var heroObj = GameObject.Find("Hero");

            mHero = heroObj.GetOrAddComponent<Hero>();
            mHero.Init();

            StartCoroutine(SpawnZombieRoutine());
        }

        private void Update()
        {
            float dt = Time.deltaTime;

            mHero?.OnUpdate(dt);
            mZombieManager?.OnUpdate(dt);
        }

        private void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;

            mZombieManager?.OnFixedUpdate(dt);
        }

        IEnumerator SpawnZombieRoutine()
        {
            int[] lines = new int[] { 1, 2, 3 };

            var waitSeconds = new WaitForSeconds(1.5f);

            while (true)
            {
                // 1 ~ 3 라인 중 무작위 라인에 좀비 한마리를 생성
                int randLine = Util.RandomSelect(lines);

                mZombieManager.SpawnZombie(randLine);

                yield return waitSeconds;
            }
        }

        public Zombie FindClosestZombie(Vector2 refPos)
        {
            return mZombieManager.FindClosestZombie(refPos);
        }
    }
}