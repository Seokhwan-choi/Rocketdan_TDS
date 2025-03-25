
namespace TDS
{
    class ActionDecider
    {
        protected Character mParent;
        protected int mFreezeCount;
        protected float mLastTargetDistance;
        public virtual void Init(Character parent) { mParent = parent; }
        public virtual void OnUpdate(float dt) { }
        public virtual ActionInst Decide() { return new Action_Idle(); }
        public bool IsActiveMove => mFreezeCount <= 0;
    }
}