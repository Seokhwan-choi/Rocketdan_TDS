using UnityEngine;

namespace TDS
{
    // ���, ���ݸ� ����
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
                // ex) ���ݼӵ� 1 => 1�ʿ� 1�� ���ݰ���
                // ���ð� = 1�� / ���ݼӵ�
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
