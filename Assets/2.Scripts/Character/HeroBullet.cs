using UnityEngine;

namespace TDS
{
    class HeroBullet : MonoBehaviour
    {
        bool mInDestory;
        float mLifeTime;
        HeroGun mParent;
        public void Init(HeroGun parent, Vector2 startPos, float angle, float bulletSpeed = 15f, float lifeTime = 5f)
        {
            mParent = parent;

            var rigidBody = GetComponent<Rigidbody2D>();

            // 총구 보정했던 각도 만큼 원상복구
            // -5도 ~5도 사이의 각도 조정을 해서 샷건 탄환 처럼 보이게 설정한다.
            float rangeRangle = (angle + 35) + Random.Range(-5, 5);

            // 방향 벡터를 역계산한다. Cos Sin 이용
            Vector2 dir = new Vector2(Mathf.Cos(rangeRangle * Mathf.Deg2Rad), Mathf.Sin(rangeRangle * Mathf.Deg2Rad));

            // 총알 총구 위치로
            transform.localPosition = startPos;

            // 총알이 대상을 향해 바라봄
            transform.rotation = Quaternion.Euler(0, 0, rangeRangle);

            rigidBody.linearVelocity = dir * bulletSpeed;

            mLifeTime = 5f;

            mInDestory = false;
        }

        private void Update()
        {
            float dt = Time.deltaTime;

            if (mLifeTime >= 0f)
            {
                mLifeTime -= dt;
                if (mLifeTime <= 0f && mInDestory == false)
                {
                    TDS.ObjectPool.ReleaseObject(gameObject);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (mInDestory)
                return;

            if (collision.gameObject.CompareTag("Zombie"))
            {
                var zombie = collision.gameObject.GetComponent<Zombie>();
                if (zombie != null && zombie.IsAlive && mLifeTime > 0f)
                {
                    DamageInst damageInst = DamageCalculator.Create(mParent.Parent, zombie);

                    zombie.OnDamage(damageInst);

                    TDS.ObjectPool.ReleaseObject(gameObject);

                    mInDestory = true;
                }
            }
        }
    }
}