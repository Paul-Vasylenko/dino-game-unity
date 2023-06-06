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
        [SerializeField] private JumpData _jumpData;
        
        private Jumper _jumper;

        private void Update()
        {
            UpdateAnimations();

            if (_jumper.IsJumping) _jumper.UpdateJump();
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

        public void StartKick()
        {
            if (!Animator.PlayAnimation(AnimationType.Kick, true))
                return;

            Animator.ActionRequested += Kick;
            Animator.AnimationEnded += EndKick;
        }

        private void Kick()
        {
            Debug.Log("Kick");
        }
        
        private void EndKick()
        {
            Debug.Log("END!");
            Animator.ActionRequested -= Kick;
            Animator.AnimationEnded -= EndKick;
            Animator.PlayAnimation(AnimationType.Kick, false);
        }
        
        public void StartBite()
        {
            if (!Animator.PlayAnimation(AnimationType.Bite, true))
                return;

            Animator.ActionRequested += Bite;
            Animator.AnimationEnded += EndBite;
        }

        private void Bite()
        {
            Debug.Log("Bite");
        }
        
        private void EndBite()
        {
            Debug.Log("END!");
            Animator.ActionRequested -= Bite;
            Animator.AnimationEnded -= EndBite;
            Animator.PlayAnimation(AnimationType.Bite, false);
        }

        public void Jump(float jumpForce)
        {
            _jumper.Jump(jumpForce);
        }
    }
}