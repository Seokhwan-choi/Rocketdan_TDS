using UnityEngine;
using TMPro;

namespace TDS
{
    class DamageTextUI : MonoBehaviour
    {
        const float Duration = 0.5f;

        float mDuration;
        Character mTarget;
        Bounds mBounds;
        TextMeshProUGUI mTextDamage;
        public void Init(Character target, double damageText)
        {
            mTarget = target;
            mBounds = mTarget.GetBounds();

            if (mTextDamage == null)
                mTextDamage = gameObject.FindComponent<TextMeshProUGUI>("Text_Damage");

            mTextDamage.text = damageText.ToNumberString();
            mTextDamage.alpha = 1f;

            mDuration = 0.5f;

            SetPos();
        }

        void SetPos()
        {
            Vector3 uiPos = Util.WorldToScreenPoint(new Vector3(mBounds.center.x, mBounds.min.y + 1f));

            transform.position = uiPos;
        }

        private void Update()
        {
            if (mDuration > 0f)
            {
                float dt = Time.deltaTime;

                mDuration -= dt;

                mTextDamage.alpha = mDuration / Duration;

                if (mDuration <= 0f)
                {
                    TDS.ObjectPool.ReleaseUI(gameObject);
                }
            }
        }
    }
}