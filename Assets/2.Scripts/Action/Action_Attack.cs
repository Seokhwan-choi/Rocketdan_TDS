using UnityEngine;

namespace TDS
{
    class Action_Attack : ActionInst
    {
        public override void OnStart(Character parent)
        {
            base.OnStart(parent);

            mDuration = 0.5f * (1f / parent.Stat.AtkSpeed);

            parent.Anim?.PlayAttack();
            
            parent.OnAttack();
        }

        public override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);

            mDuration -= dt;
        }
    }
}