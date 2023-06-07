using Core.Animation;
using Core.Movement.Controller;
using UnityEngine;
using UnityEngine.Rendering;

namespace NPC.Behaviour
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class BaseEntityBehaviour : MonoBehaviour
    {
        [SerializeField] protected AnimationController Animator;
        [SerializeField] private SortingGroup _sortingGroup;

        protected Rigidbody2D Rigidbody;
        protected HorizontalMover HorizontalMover;

        public virtual void Initialize()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
        }

        public void SetDrawingOrder(int order) => _sortingGroup.sortingOrder = order;
        public void MoveHorizontally(float direction) => HorizontalMover.MoveHorizontally(direction);

        protected virtual void UpdateAnimations()
        {
            Animator.PlayAnimation(AnimationType.Idle, true);
            Animator.PlayAnimation(AnimationType.Run, HorizontalMover.IsMoving);
        }
    }
}