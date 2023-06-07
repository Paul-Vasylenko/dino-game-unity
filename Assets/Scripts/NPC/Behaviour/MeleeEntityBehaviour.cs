using System;
using Battle;
using Core.Animation;
using Core.Enums;
using Core.Movement.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace NPC.Behaviour
{
    public class MeleeEntityBehaviour : BaseEntityBehaviour
    {
        [SerializeField] private float _afterAttackDelay;
        [SerializeField] private Collider2D _collider2D;
        [field: SerializeField] public Slider HpBar { get; private set; }

        [field: SerializeField] public Vector2 SearchBox { get; private set; }
        [field: SerializeField] public LayerMask Targets { get; private set; }

        public Vector2 Size => _collider2D.bounds.size;

        public event Action AttackSequenceEnded;
        private float _damage;

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, SearchBox);
        }

        public override void Initialize()
        {
            base.Initialize();
            HorizontalMover = new PositionMover(Rigidbody);
        }
        
        private void Update() => UpdateAnimations();

        public void StartAttack(float damage)
        {
            _damage = damage;
            Animator.PlayAnimation(AnimationType.Kick, true, Attack, EndAttack);
        }

        public void SetDirection(Direction direction) => HorizontalMover.SetDirection(direction);

        private void Attack()
        {
            Collider2D hitPlayer = Physics2D.OverlapCircle(_attackPoint.position, _attackRange, Targets);
            
            Debug.Log(hitPlayer);
            if (hitPlayer == null) return;

            if (hitPlayer.TryGetComponent(out IDamageable damageable)) damageable.TakeDamage(_damage);
        }

        private void EndAttack()
        {
            Animator.PlayAnimation(AnimationType.Kick, false);
            Invoke(nameof(EndAttackSequence), _afterAttackDelay);
        }

        private void EndAttackSequence()
        {
            AttackSequenceEnded?.Invoke();
        }
    }
}
