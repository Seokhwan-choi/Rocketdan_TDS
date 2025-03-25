using UnityEngine;


namespace TDS
{
    class Action_Idle : ActionInst
    {
        public override void OnStart(Character parent)
        {
            base.OnStart(parent);

            mDuration = 999f;

            parent.Anim?.PlayIdle();
        }

        public override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);

            bool anyInAttackRange = mParent.AnyInAttackRangeOpponent();

            if (anyInAttackRange)
            {
                mDuration = 0f;
            }
        }
    }
}

