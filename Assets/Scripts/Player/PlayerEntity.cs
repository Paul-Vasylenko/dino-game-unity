using System;
using Battle;
using Core.Animation;
using Core.Movement.Controller;
using Core.Movement.Data;
using Drawing;
using NPC.Behaviour;
using StatsSystem;
using UnityEngine;
using UnityEngine.Rendering;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class PlayerEntity : BaseEntityBehaviour, ILevelGraphicElement
    {
        [field: SerializeField] public PlayerStatsUIView StatsUIView { get; private set; }
        [SerializeField] private JumpData _jumpData;

        [SerializeField] private Transform _attackPoint;
        [SerializeField] private Transform _bitePoint;
        [SerializeField] private float _attackRange;
        [SerializeField] private LayerMask _targets;
        
        private Jumper _jumper;
        private float _damage;

        private void Update()
        {
            UpdateAnimations();

            if (_jumper.IsJumping) _jumper.UpdateJump();
        }
        
        private void OnDrawGizmosSelected()
        {
            if (_attackPoint == null)
                return;
            Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
            if (_bitePoint == null)
                return;
            Gizmos.DrawWireSphere(_bitePoint.position, _attackRange);
        }

        public override void Initialize()
        {
            base.Initialize();
            Rigidbody = GetComponent<Rigidbody2D>();
            HorizontalMover = new VelocityMover(Rigidbody);
            _jumper = new Jumper(Rigidbody, _jumpData);
        }

        protected override void UpdateAnimations()
        {
            base.UpdateAnimations();
            Animator.PlayAnimation(AnimationType.Jump, _jumper.IsJumping);
        }

        public void StartKick(float damage)
        {
            _damage = damage;
            if (!Animator.PlayAnimation(AnimationType.Kick, true, Kick, EndKick))
                return;
        }

        private void Kick()
        {
            Collider2D hitEnemy = Physics2D.OverlapCircle(_attackPoint.position, _attackRange, _targets);
            
            if (hitEnemy == null) return;

            if (hitEnemy.TryGetComponent(out IDamageable damageable)) damageable.TakeDamage(_damage);
        }

        private void EndKick()
        {
            Animator.PlayAnimation(AnimationType.Kick, false, Kick, EndKick);
        }
        
        public void StartBite(float damage)
        {
            _damage = damage * 1.3f;
            if (!Animator.PlayAnimation(AnimationType.Bite, true, Bite, EndBite))
                return;
        }

        private void Bite()
        {
            Collider2D hitEnemy = Physics2D.OverlapCircle(_bitePoint.position, _attackRange, _targets);
            
            if (hitEnemy == null) return;

            if (hitEnemy.TryGetComponent(out IDamageable damageable)) damageable.TakeDamage(_damage);
        }
        
        private void EndBite()
        {
            Animator.PlayAnimation(AnimationType.Bite, false, Bite, EndBite);
        }

        public void Jump(float jumpForce)
        {
            _jumper.Jump(jumpForce);
        }
    }
}