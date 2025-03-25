using UnityEngine;

namespace TDS
{
    class Hero : Character
    {
        HeroGun mGun;
        HeroTargetManager mTargetManager;
        public Zombie Target => mTargetManager.Target;
        public void Init()
        {
            mTargetManager = new HeroTargetManager();
            mTargetManager.Init(this);

            var gunObj = gameObject.FindGameObject("ChaGun_0");

            mGun = gunObj.GetOrAddComponent<HeroGun>();
            mGun.Init(this);

            mStat = new CharacterStat();
            mStat.InitHero();

            mPhysics = new CharacterPhysics();
            mPhysics.Init(this);

            mAction = new ActionManager();
            mAction.Init(this);
        }

        public override void OnAttack()
        {
            mGun.ShotGun();
        }

        public override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);

            mTargetManager.OnUpdate(dt);
            mAction.OnUpdate(dt);
            mGun.OnUpdate(dt);
        }

        public override bool AnyInAttackRangeOpponent()
        {
            return mTargetManager.Target != null;
        }
    }
}