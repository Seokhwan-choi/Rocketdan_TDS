using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;


namespace TDS
{
    class CharacterAnim
    {
        protected Animator mAnim;
        public void Init(Character parent)
        {
            mAnim = parent.GetComponentInChildren<Animator>();
        }

        public void PlayDeath()
        {
            mAnim.SetBool("IsDead", true);
            mAnim.SetBool("IsAttacking", false);
            mAnim.SetBool("IsIdle", false);
        }
        public void PlayIdle()
        {
            mAnim.SetBool("IsDead", false);
            mAnim.SetBool("IsAttacking", false);
            mAnim.SetBool("IsIdle", true);
        }

        public void PlayMove()
        {
            mAnim.SetBool("IsDead", false);
            mAnim.SetBool("IsAttacking", false);
            mAnim.SetBool("IsIdle", true);
        }

        public void PlayJump()
        {
            mAnim.SetBool("IsDead", false);
            mAnim.SetBool("IsAttacking", false);
            mAnim.SetBool("IsIdle", true);
        }

        public void PlayAttack()
        {
            mAnim.SetBool("IsDead", false);
            mAnim.SetBool("IsAttacking", true);
            mAnim.SetBool("IsIdle", false);
        }
    }
}