using UnityEngine;
using System;

namespace TDS
{
    // ���, ����, ����
    class Zombie_ActionDecider : ActionDecider 
    {
        float mAttackCoolTime;
        public override void OnUpdate(float dt)
        {
            if (mParent is Zombie zombie)
            {
                if (zombie.Action.IsMove && zombie.InJumping == false)
                {
                    float distance = Vector2.Distance(TDS.GameManager.Hero.GetPosition(), zombie.GetPosition());

                    // �Ÿ��� ���� ������ ���ϰ� �ִٸ� �������� ���Ѵٰ� �Ǵ��Ѵ�.
                    if (mLastTargetDistance < distance)
                    {
                        mFreezeCount++;

                        // ���� �ð� �� �����̰� �ִٸ�
                        if (mFreezeCount >= 30)
                        {
                            // ���� �ڿ� �ִ� �������� Ȯ���ϰ�
                            if (zombie.Manager.IsBackMostZombie(zombie))
                            {
                                // �����Ѵ�.
                                zombie.CharacterPhysics.Jump();

                                mFreezeCount = 0;
                            }
                        }
                    }
                    else
                    {
                        mFreezeCount = 0;
                    }

                    mLastTargetDistance = distance;
                }

                if ((zombie.Action.IsIdle || zombie.Action.IsMove) && mAttackCoolTime > 0f)
                {
                    mAttackCoolTime -= dt;
                }
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
                return new Action_Move();
            }
        }
    }
}
