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

            // �ѱ� �����ߴ� ���� ��ŭ ���󺹱�
            // -5�� ~5�� ������ ���� ������ �ؼ� ���� źȯ ó�� ���̰� �����Ѵ�.
            float rangeRangle = (angle + 35) + Random.Range(-5, 5);

            // ���� ���͸� ������Ѵ�. Cos Sin �̿�
            Vector2 dir = new Vector2(Mathf.Cos(rangeRangle * Mathf.Deg2Rad), Mathf.Sin(rangeRangle * Mathf.Deg2Rad));

            // �Ѿ� �ѱ� ��ġ��
            transform.localPosition = startPos;

            // �Ѿ��� ����� ���� �ٶ�
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