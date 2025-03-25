using UnityEngine;

namespace TDS
{
    class Action_Move : ActionInst
    {
        public override void OnStart(Character parent)
        {
            base.OnStart(parent);

            mDuration = 999f;

            mParent.Anim?.PlayMove();
        }

        public override void OnUpdate(float dt)
        {
            
        }

        public override void OnFixedUpdate(float dt)
        {
            if (mParent.IsAlive)
            {
                // ������ �������θ� �̵�
                mParent.MoveToLeft();

                // ������ �� �ִ� ����� �ִٸ� �����.
                bool anyInAttackRange = mParent.AnyInAttackRangeOpponent();

                if (anyInAttackRange)
                {
                    mDuration = 0f;
                }
            }
        }
    }
}