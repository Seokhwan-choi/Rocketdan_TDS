using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace TDS
{
    class CharacterPhysics
    {
        bool mInKnockback;
        bool mInSmoothKnockback;
        float mKnockbackTime;
        Transform mTm;
        float mBodySize;
        Rigidbody2D mRigidBody;
        public bool InJumping => mRigidBody.linearVelocityY > 0;
        public bool InSmoothKnockback => mInSmoothKnockback;
        public float BodySize => mBodySize;
        public void Init(Character parent)
        {
            mBodySize = 0.5f;
            mTm = parent.transform;
            mTm.rotation = Quaternion.Euler(Vector3.zero);

            mRigidBody = parent.GetComponent<Rigidbody2D>();
        }

        public void OnUpdate(float dt)
        {
            if (mInKnockback)
            {
                mKnockbackTime -= dt;
                if (mKnockbackTime <= 0f)
                {
                    mInKnockback = false;
                }
            }
        }

        public void OnFixedUpdate(float dt)
        {
            if (mInSmoothKnockback)
            {
                mKnockbackTime -= dt;
                if (mKnockbackTime <= 0f)
                {
                    mInSmoothKnockback = false;
                }
                else
                {
                    mRigidBody.linearVelocity = new Vector2(1.5f, mRigidBody.linearVelocity.y);
                }
            }
        }

        public bool IsInRange(CharacterPhysics physics, float range)
        {
            float distance = Mathf.Abs(GetPosition().x - physics.GetPosition().x);

            bool isInRange = distance <= (range + (physics.BodySize * 0.5f));

            return isInRange;
        }

        public void SetPosition(Vector2 pos)
        {
            mTm.position = pos;
        }

        public void SetLocalPosition(Vector2 pos)
        {
            mTm.localPosition = pos;
        }

        public Vector2 GetPosition()
        {
            return mTm?.position ?? Vector2.zero;
        }

        public void Jump(float jumpForce = 6f)
        {
            mRigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        public void SmoothKnockback(float knockbackTime = 0.5f)
        {
            mInSmoothKnockback = true;
            mKnockbackTime = knockbackTime;
        }

        public void Knockback(float knockbackForce = 3f, float knockbackTime = 0.5f)
        {
            mInKnockback = true;

            mKnockbackTime = knockbackTime;

            mRigidBody.AddForce(Vector2.right * knockbackForce, ForceMode2D.Impulse);
        }

        // 무조건 좌측으로만 이동
        public void MoveToLeft(float moveSpeed)
        {
            if (mInKnockback == false && mInSmoothKnockback == false)
                mRigidBody.linearVelocity = new Vector2(-moveSpeed, mRigidBody.linearVelocity.y);
        }

        public void OnRelease()
        {
            mTm = null;
        }
    }
}