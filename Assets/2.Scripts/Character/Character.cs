using UnityEngine;

namespace TDS
{
    class Character : MonoBehaviour
    {
        protected ActionManager mAction;
        protected CharacterAnim mAnim;
        protected CharacterStat mStat;
        protected CharacterPhysics mPhysics;
        public ActionManager Action => mAction;
        public CharacterStat Stat => mStat;
        public CharacterAnim Anim => mAnim;
        public CharacterPhysics CharacterPhysics => mPhysics;
        public bool IsDeath => mStat.IsDeath;
        public bool IsAlive => !IsDeath;
        public Vector2 GetPosition() { return mPhysics.GetPosition(); }
        public void SetPosition(Vector2 pos) { mPhysics.SetPosition(pos); }
        public void SetLocalPosition(Vector2 pos) { mPhysics.SetLocalPosition(pos); }
        public void MoveToLeft() { mPhysics.MoveToLeft(mStat.MoveSpeed); }
        public Bounds GetBounds() { return GetComponent<CapsuleCollider2D>().bounds; }
        public virtual bool AnyInAttackRangeOpponent() { return false; }
        public virtual void OnDamage(DamageInst inst) { }
        public virtual void OnAttack() { }
        public virtual void OnUpdate(float dt) { }
        public virtual void OnFixedUpdate(float dt) { }
        public virtual void OnRelease() { }
        public virtual void OnDeath() { }
    }
}