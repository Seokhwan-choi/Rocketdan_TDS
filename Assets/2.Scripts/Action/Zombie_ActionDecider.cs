using UnityEngine;
using System;

namespace TDS
{
    // 대기, 공격, 점프
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

                    // 거리를 전혀 좁히지 못하고 있다면 움직이지 못한다고 판단한다.
                    if (mLastTargetDistance < distance)
                    {
                        mFreezeCount++;

                        // 일정 시간 못 움직이고 있다면
                        if (mFreezeCount >= 30)
                        {
                            // 가장 뒤에 있는 좀비인지 확인하고
                            if (zombie.Manager.IsBackMostZombie(zombie))
                            {
                                // 점프한다.
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
                // ex) 공격속도 1 => 1초에 1번 공격가능
                // 대기시간 = 1초 / 공격속도
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
