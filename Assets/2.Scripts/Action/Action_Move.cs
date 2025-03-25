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
                // 무조건 좌측으로만 이동
                mParent.MoveToLeft();

                // 공격할 수 있는 대상이 있다면 멈춘다.
                bool anyInAttackRange = mParent.AnyInAttackRangeOpponent();

                if (anyInAttackRange)
                {
                    mDuration = 0f;
                }
            }
        }
    }
}