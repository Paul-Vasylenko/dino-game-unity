using System;
using UnityEngine;

namespace Core.Animation
{
    public abstract class AnimationController : MonoBehaviour
    {
        private AnimationType _currentAnimationType;
        
        public event Action ActionRequested;
        public event Action AnimationEnded;

        public bool PlayAnimation(AnimationType animationType, bool active)
        {
            if (!active)
            {
                if (_currentAnimationType == AnimationType.Idle || _currentAnimationType != animationType)
                    return false; // if we try to disable Idle animation or wrong animation

                SetAnimation(AnimationType.Idle);
                return false;
            }

            if (_currentAnimationType > animationType)
                return false; // new animation priority is less than the animation playing now

            SetAnimation(animationType);
            return true;
        }

        protected abstract void PlayAnimation(AnimationType animationType);
        private void SetAnimation(AnimationType animationType)
        {
            _currentAnimationType = animationType;
            PlayAnimation(_currentAnimationType);
        }
        protected void OnActionRequested() => ActionRequested?.Invoke();
        protected void OnAnimationEnded() => AnimationEnded?.Invoke();
    }
}