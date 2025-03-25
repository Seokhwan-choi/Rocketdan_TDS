using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace TDS
{
    class Zombie : Character
    {
        const float FloorHeight = 1f;
        const float RayUpdateInterval = 0.1f;
        float mRayUpdateInterval;

        int mLine;
        int mFloor;
        bool mIsGrounded;   // 바닥에 있음 ( 점프 X )
        bool mIsOverHead;   // 머리에 좀비가 있음
        LayerMask mZombieLayerMask;
        LayerMask mGroundLayerMask;
        ZombieManager mManager;
        public ZombieManager Manager => mManager;
        public int Line => mLine;
        public int Floor => mFloor;
        public bool IsOverHead => mIsOverHead;
        public bool InJumping => mPhysics.InJumping && mIsGrounded == false;
        public bool InSmoothKnockback => mPhysics.InSmoothKnockback;
        public void Init(ZombieManager manager, int line)
        {
            mLine = line;
            mFloor = 0;

            mManager = manager;

            mStat = new CharacterStat();
            mStat.InitMonster();

            mAction = new ActionManager();
            mAction.Init(this);

            mAnim = new CharacterAnim();
            mAnim.Init(this);

            mPhysics = new CharacterPhysics();
            mPhysics.Init(this);

            // 좀비가 생성되는 라인의 레이어를 지정해준다.
            gameObject.layer = LayerMask.NameToLayer($"Zombie_Line_{line:00}");
            mZombieLayerMask = LayerMask.GetMask("Zombie_Line_01", "Zombie_Line_02", "Zombie_Line_03");
            mGroundLayerMask = LayerMask.GetMask(
                "Zombie_Line_01", "Zombie_Line_Ground_01",
                "Zombie_Line_02", "Zombie_Line_Ground_02",
                "Zombie_Line_03", "Zombie_Line_Ground_03");
        }

        
        public override void OnUpdate(float dt)
        {
            mAction.OnUpdate(dt);
            mPhysics.OnUpdate(dt);

            // 층 수 계산 (0층부터 시작한다고 가정)
            mFloor = Mathf.FloorToInt(transform.localPosition.y / FloorHeight);

            UpdateOverHead();
            UpdateGrounded();
        }

        void UpdateOverHead()
        {
            // ContactFilter 설정
            ContactFilter2D filter = new ContactFilter2D();
            filter.SetLayerMask(mZombieLayerMask);

            // 결과 저장 배열
            RaycastHit2D[] results = new RaycastHit2D[10];

            int hitCount = Physics2D.Raycast(transform.position, Vector2.up, filter, results, 0.025f);
            if (hitCount > 0)
            {
                for (int i = 0; i < results.Length; ++i)
                {
                    if (results[i].collider == null)
                        continue;

                    // 나 자신은 무시
                    if (results[i].collider.gameObject != this.gameObject)
                    {
                        mIsOverHead = true;
                        break;
                    }
                }
            }
            else
            {
                mIsOverHead = false;
            }
        }

        void UpdateGrounded()
        {
            // ContactFilter 설정
            ContactFilter2D filter = new ContactFilter2D();
            filter.SetLayerMask(mGroundLayerMask);

            // 결과 저장 배열
            RaycastHit2D[] results = new RaycastHit2D[10];

            int hitCount = Physics2D.Raycast(transform.position, Vector2.down, filter, results, 0.025f);
            if (hitCount > 0)
            {
                for (int i = 0; i < results.Length; ++i)
                {
                    if (results[i].collider == null)
                        continue;

                    // 나 자신은 무시
                    if (results[i].collider.gameObject != this.gameObject)
                    {
                        var zombie = results[i].collider.gameObject.GetComponent<Zombie>();
                        if (zombie != null)
                        {
                            mIsGrounded = zombie.InJumping == false;
                            break;
                        }
                        else
                        {
                            mIsGrounded = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                mIsGrounded = false;
            }
        }

        public override void OnFixedUpdate(float dt)
        {
            mAction.OnFixedUpdate(dt);
            mPhysics.OnFixedUpdate(dt);
        }

        public override void OnDamage(DamageInst inst)
        {
            if (IsDeath)
                return;

            // 데미지 입음
            mStat.OnDamage(inst.Damage);

            ShowDamageText(this, inst.Damage);

            // 데미지를 입어서 죽었다면 게임 매니저한테 알려주자
            if (mStat.IsDeath)
            {
                OnDeath();
            }
        }

        public override void OnAttack()
        {
            base.OnAttack();

            if (mFloor > 0)
            {
                // 근접 공격하고 확률적으로 정렬하기..?
                if (Util.Dice(0.75f))
                {
                    // 좀비 탑이 쌓여있다면
                    if (mManager.HaveFloorZombie(mLine))
                    {
                        // 맨 밑을 밀어주기
                        mManager.SmoothKnockBackZombies(mLine, 0);
                    }
                }
            }
        }

        public override bool AnyInAttackRangeOpponent()
        {
            return mPhysics.IsInRange(TDS.GameManager.Hero.CharacterPhysics, mStat.AtkRange);
        }

        public override void OnDeath()
        {
            mAnim.PlayDeath();

            mManager.RemoveZombie(this);
        }

        void ShowDamageText(Character target, double damage)
        {
            GameObject damageTextObj = TDS.ObjectPool.AcquireUI("DamageTextUI", TDS.GameManager.DamageParent);

            DamageTextUI damageTextUI = damageTextObj.GetOrAddComponent<DamageTextUI>();

            damageTextUI.Init(target, damage);
        }
    }
}