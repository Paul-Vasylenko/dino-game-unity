using System;
using Battle;
using Core.Animation;
using Core.Movement.Controller;
using UnityEngine;
using UnityEngine.Rendering;

namespace NPC.Behaviour
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class BaseEntityBehaviour : MonoBehaviour, IDamageable
    {
        [SerializeField] protected AnimationController Animator;
        [SerializeField] private SortingGroup _sortingGroup;
        [SerializeField] protected Transform _attackPoint;
        [SerializeField] protected float _attackRange;

        
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
        
        public event Action<float> DamageTaken;

        public void TakeDamage(float damage) => DamageTaken?.Invoke(damage);

        public void DestroyObject()
        {
            Animator.PlayAnimation(AnimationType.Death, true);
            Destroy(gameObject, 1.4f);
        }
        
        public void PlayDeathAnimation()
        {
            Animator.PlayAnimation(AnimationType.Death, true);
        }
    }
}