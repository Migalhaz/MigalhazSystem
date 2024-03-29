using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MigalhaSystem.Animation
{
    public abstract class AnimationController : MonoBehaviour
    {
        [Header("Animator")]
        [SerializeField] Animator m_animator;
        
        public Animator GetAnimator()
        {
            if (m_animator is null) TryGetComponent(out m_animator);
            return m_animator;
        }

        public void PlayAnimation(AnimationSO animation)
        {
            animation.Play(GetAnimator());
        }

        public void PlayAnimation(AnimationSO animation, float normalizedTime)
        {
            animation.Play(GetAnimator(), normalizedTime);
        }

        public void CrossFadeAnimation(AnimationSO animation)
        {
            animation.CrossFade(GetAnimator());
        }

        public void CrossFadeAnimation(AnimationSO animation, float transitionDuration)
        {
            animation.CrossFade(GetAnimator(), transitionDuration);
        }
    }
}