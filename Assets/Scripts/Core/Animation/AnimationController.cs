using UnityEngine;

namespace Core.Animation
{
    public abstract class AnimationController : MonoBehaviour
    {
        private AnimationType _currentAnimationType;

        public void PlayAnimation(AnimationType animationType, bool active)
        {
            if (!active)
            {
                if (_currentAnimationType == AnimationType.Idle || _currentAnimationType != animationType)
                    return; // if we try to disable Idle animation or wrong animation

                _currentAnimationType = AnimationType.Idle;
                PlayAnimation(_currentAnimationType);
                return;
            }
            if (_currentAnimationType > animationType)
                return; // new animation priority is less than the animation playing now

            _currentAnimationType = animationType;
            PlayAnimation(_currentAnimationType);
        }

        protected abstract void PlayAnimation(AnimationType animationType);
    }
}