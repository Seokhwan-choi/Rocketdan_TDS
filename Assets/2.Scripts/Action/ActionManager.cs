using System.Collections.Generic;
using UnityEngine;

namespace TDS
{
    enum ActionType
    {
        Attack,
        Idle,
        Move,

        Count
    }

    class ActionManager
    {
        Character mParent;
        List<ActionInst> mActionList;
        ActionInst mCurrAction;
        ActionDecider mActionDecider;
        public ActionInst CurrAction => mCurrAction;
        public bool IsActiveMove => mActionDecider.IsActiveMove;
        public bool IsIdle => mCurrAction is Action_Idle;
        public bool IsMove => mCurrAction is Action_Move;
        public void Init(Character parent)
        {
            mParent = parent;
            mActionList = new List<ActionInst>();

            mActionDecider = parent is Hero ? new Hero_ActionDecider() : new Zombie_ActionDecider();
            mActionDecider.Init(parent);
        }

        public void OnUpdate(float dt)
        {
            if (mCurrAction == null)
            {
                mCurrAction = PopAction();
                mCurrAction?.OnStart(mParent);
            }
            else
            {
                if (mCurrAction.IsFinish)
                {
                    mCurrAction.OnFinish();
                    mCurrAction = null;
                    mCurrAction = PopAction();
                    mCurrAction?.OnStart(mParent);
                }
            }

            mCurrAction?.OnUpdate(dt);
            mActionDecider.OnUpdate(dt);
        }

        public void OnFixedUpdate(float dt)
        {
            mCurrAction?.OnFixedUpdate(dt);
        }

        public void OnRelease()
        {
            mActionList.Clear();
            mActionList = null;
        }

        public void PlayMoveAction(bool forced = false)
        {
            var moveAction = new Action_Move();

            PlayAction(moveAction, forced);
        }

        void PlayAction(ActionInst action, bool forced = false)
        {
            if (action != null)
            {
                if (forced)
                {
                    mCurrAction?.OnFinish();
                    mCurrAction = null;

                    mCurrAction = action;
                    mCurrAction.OnStart(mParent);
                }
                else
                {
                    mActionList.Add(action);
                }
            }
        }

        ActionInst PopAction()
        {
            if (mParent.IsDeath)
                return null;

            if (mActionList.Count > 0)
            {
                var firstAction = mActionList[0];
                mActionList.RemoveAt(0);

                return firstAction;
            }
            else
            {
                // 모든 액션을 마무리 했으면 지금 기준으로
                // 새로운 액션을 만들어주자
                return mActionDecider.Decide();
            }
        }
    }
}