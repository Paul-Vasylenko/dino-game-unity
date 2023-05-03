using System;
using UnityEngine;

namespace Core.Animation
{
    public abstract class AnimationController : MonoBehaviour
    {
        private AnimationType _currentAnimationType;
        
        private Action _animationAction;
        private Action _animationEndAction;

        public bool PlayAnimation(AnimationType animationType, bool active, Action animationAction = null, Action endAnimationAction = null)
        {
            if (!active)
            {
                if (_currentAnimationType == AnimationType.Idle || _currentAnimationType != animationType)
                    return false; // if we try to disable Idle animation or wrong animation

                OnAnimationEnded();
                return false;
            }

            if (_currentAnimationType > animationType)
                return false; // new animation priority is less than the animation playing now

            _animationAction = animationAction;
            _animationEndAction = endAnimationAction;
            SetAnimation(animationType);
            return true;
        }

        protected abstract void PlayAnimation(AnimationType animationType);
        private void SetAnimation(AnimationType animationType)
        {
            _currentAnimationType = animationType;
            PlayAnimation(_currentAnimationType);
        }
        protected void OnAnimationEnded()
        {
            _animationEndAction?.Invoke();
            _animationEndAction = null;
            SetAnimation(AnimationType.Idle);
        } 
    }
}