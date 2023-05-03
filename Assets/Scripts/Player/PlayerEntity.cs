using Core.Animation;
using Core.Movement.Controller;
using Core.Movement.Data;
using Core.Tools;
using StatsSystem;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class PlayerEntity : MonoBehaviour
    {
        [Header("Animation")] [SerializeField] private AnimationController _animator;

        [SerializeField] private HorizontalMovementData _horizontalMovementData;
        [SerializeField] private JumpData _jumpData;
        [SerializeField] private DirectionalCameraPair _cameras;
        private Collider2D _collider;

        private HorizontalMover _horizontalMover;
        private Jumper _jumper;

        private Rigidbody2D _rigidbody;
        

        private void Update()
        {
            UpdateAnimations();
            UpdateCameras();

            if (_jumper.IsJumping) _jumper.UpdateJump();
        }

        public void Initialize(IStatValueGiver statValueGiver)
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<CapsuleCollider2D>();
            _horizontalMover = new HorizontalMover(_rigidbody, _horizontalMovementData, statValueGiver);
            _jumper = new Jumper(_rigidbody, _jumpData, _collider, statValueGiver);
        }

        private void UpdateCameras()
        {
            foreach (var cameraPair in _cameras.DirectionalCameras)
                cameraPair.Value.enabled = cameraPair.Key != _horizontalMover.Direction;
        }

        private void UpdateAnimations()
        {
            _animator.PlayAnimation(AnimationType.Idle, true);
            _animator.PlayAnimation(AnimationType.Run, _horizontalMover.IsMoving);
            _animator.PlayAnimation(AnimationType.Jump, _jumper.IsJumping);
        }
        
        public void StartKick()
        {
            if (!_animator.PlayAnimation(AnimationType.Kick, true))
                return;

            _animator.ActionRequested += Kick;
            _animator.AnimationEnded += EndKick;
        }

        private void Kick()
        {
            Debug.Log("Kick");
        }
        
        private void EndKick()
        {
            Debug.Log("END!");
            _animator.ActionRequested -= Kick;
            _animator.AnimationEnded -= EndKick;
            _animator.PlayAnimation(AnimationType.Kick, false);
        }
        
        public void StartBite()
        {
            if (!_animator.PlayAnimation(AnimationType.Bite, true))
                return;

            _animator.ActionRequested += Bite;
            _animator.AnimationEnded += EndBite;
        }

        private void Bite()
        {
            Debug.Log("Bite");
        }
        
        private void EndBite()
        {
            Debug.Log("END!");
            _animator.ActionRequested -= Bite;
            _animator.AnimationEnded -= EndBite;
            _animator.PlayAnimation(AnimationType.Bite, false);
        }

        public void MoveHorizontally(float direction)
        {
            _horizontalMover.MoveHorizontally(direction);
        }

        public void Jump()
        {
            _jumper.Jump();
        }
    }
}