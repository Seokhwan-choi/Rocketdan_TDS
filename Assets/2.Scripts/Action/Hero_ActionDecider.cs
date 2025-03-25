using UnityEngine;

namespace TDS
{
    // 대기, 공격만 가능
    class Hero_ActionDecider : ActionDecider
    {
        float mAttackCoolTime;
        public override void OnUpdate(float dt)
        {
            if (mParent.Action.IsIdle && mAttackCoolTime > 0f)
            {
                mAttackCoolTime -= dt;
            }
        }

        public override ActionInst Decide()
        {
            bool anyInAttackRange = mParent.AnyInAttackRangeOpponent();
            if (anyInAttackRange && mAttackCoolTime <= 0)
            {
                // ex) 공격속도 1 => 1초에 1번 공격가능
                // 대기시간 = 1초 / 공격속도
                mAttackCoolTime = 1 / mParent.Stat.AtkSpeed;

                return new Action_Attack();
            }
            else
            {
                return new Action_Idle();
            }
        }
    }
}
