using UnityEngine;


namespace TDS
{
    class HeroGun : MonoBehaviour
    {
        Hero mParent;
        GameObject mMuzzlePointObj;
        public Hero Parent => mParent;
        public void Init(Hero parent)
        {
            mParent = parent;

            mMuzzlePointObj = gameObject.FindGameObject("MuzzlePoint");
        }

        public void OnUpdate(float dt)
        {
            // ���콺�� ������ ������ ���콺 ��ǥ�� ���� ȸ������
            if (Input.GetMouseButton(0))
            {
                Vector3 screenTouchPos = Input.mousePosition;
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenTouchPos);

                RotateGunMuzzle(mParent.GetPosition(), new Vector2(worldPos.x, worldPos.y));
            }
            else
            {
                RotateGunMuzzle(mParent.GetPosition(), mParent.Target.GetPosition());
            }
        }

        void RotateGunMuzzle(Vector2 refPos, Vector2 targetPos)
        {
            if (mParent.Target == null) 
                return;

            if (mParent.Target != null && mParent.Target.IsDeath)
                return;

            // ���� ����
            Vector2 dir = targetPos - refPos;

            // ���� ���͸� ������ ��ȯ (���� �� ��)
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            // �ѱ� ���� �̹����� �ణ ������ �ֱ� ������ �ణ ����
            transform.rotation = Quaternion.Euler(0, 0, angle - 35f);
        }

        public void ShotGun()
        {
            for (int i = 0; i < 5; ++i)
            {
                GameObject bulletObj = TDS.ObjectPool.AcquireObject("Hero/HeroBullet");

                var bullet = bulletObj.GetOrAddComponent<HeroBullet>();

                bullet.Init(this, mMuzzlePointObj.transform.position, transform.eulerAngles.z);
            }
        }
    }
}