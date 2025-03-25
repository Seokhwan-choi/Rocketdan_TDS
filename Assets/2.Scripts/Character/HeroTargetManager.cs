using UnityEngine;

namespace TDS
{
    class HeroTargetManager
    {
        Hero mParent;
        Zombie mTarget;
        public Zombie Target => mTarget;
        public void Init(Hero hero)
        {
            mParent = hero;
        }

        public void OnUpdate(float dt)
        {
            if (mTarget != null && mTarget.IsDeath)
            {
                FindTarget();
            }
            else
            {
                FindTarget();
            }
        }

        void FindTarget()
        {
            mTarget = TDS.GameManager.FindClosestZombie(mParent.GetPosition());
        }
    }
}