using System;
using UnityEngine;

namespace Core.Animation
{
    public abstract class AnimationController : MonoBehaviour
    {
        private AnimationType _currentAnimationType;

        private Action _animationAction;
        private Action _animationEndAction;

        public event Action ActionRequested;
        public event Action AnimationEnded;

        public bool PlayAnimation(AnimationType animationType, bool active, Action animationAction = null, Action endAnimationAction = null)
        {
            if (!active)
            {
                if (_currentAnimationType == AnimationType.Idle || _currentAnimationType != animationType)
                    return false;

                _animationAction = null;
                _animationEndAction = null;
                OnAnimationEnded();
                return false;
            }

            if (_currentAnimationType >= animationType)
                return false;

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

        protected void OnActionRequested() => _animationAction?.Invoke();

        protected void OnAnimationEnded()
        {
            _animationEndAction?.Invoke();
            SetAnimation(AnimationType.Idle);
        }
    }
}