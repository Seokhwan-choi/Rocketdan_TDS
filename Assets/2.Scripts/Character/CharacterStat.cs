using System;
using UnityEngine;

namespace TDS
{
    // 캐릭터 (영웅, 몬스터) 능력치
    class CharacterStat
    {
        double mAtk;
        double mCurHp;
        double mMaxHp;
        float mAtkSpeed;
        float mAtkRange;
        float mMoveSpeed;
        public bool IsDeath => mCurHp <= 0;
        public double Atk => mAtk;
        public double CurHp => mCurHp;
        public double MaxHp => mMaxHp;
        public float MoveSpeed => mMoveSpeed;
        public float AtkSpeed => mAtkSpeed;
        public float AtkRange => mAtkRange;
        public void InitHero()
        {
            mAtk = 10;
            mMaxHp = mCurHp = 100;
            mAtkRange = 10.0f;
            mAtkSpeed = 1f;
        }

        public void InitMonster()
        {
            mAtk = 10;
            mMaxHp = mCurHp = 100;
            mAtkRange = 2f;
            mMoveSpeed = 3f;
            mAtkSpeed = 1f;
        }

        public void OnDamage(double damage)
        {
            if (mCurHp <= 0)
            {
                return;
            }
            else
            {
                mCurHp -= damage;
                mCurHp = Math.Max(0, mCurHp);
            }
        }
    }
}