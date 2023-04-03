using System;
using Core.Enums;
using UnityEngine;
using Core.Tools;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class PlayerEntity : MonoBehaviour
    {
        [Header("Animation")] [SerializeField] private Animator _animator;
        
        [Header("Horizontal Movement")]
        [SerializeField] private float _horizontalSpeed;
        [SerializeField] private Direction _direction;

        [Header("Jumping")] 
        [SerializeField] private float _jumpForce;
        [SerializeField] private LayerMask _platformLayerMask;

        [SerializeField] private DirectionalCameraPair _cameras;
        
        private Rigidbody2D _rigidbody;
        private Collider2D _collider;
        private bool _isJumping;
        private Vector2 _movement;
        private AnimationType _currentAnimationType;

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<CapsuleCollider2D>();
        }

        private void Update()
        {
            UpdateAnimations();

            if (_isJumping)
            {
                UpdateJump();
            }

        }

        private void UpdateAnimations()
        {
            PlayAnimation(AnimationType.Idle, true);
            PlayAnimation(AnimationType.Run, _movement.magnitude > 0);
            PlayAnimation(AnimationType.Jump, _isJumping);
        }

        public void MoveHorizontally(float direction)
        {
            SetDirection(direction);
            _movement.x = direction;
            Vector2 velocity = _rigidbody.velocity;
            velocity.x = direction * _horizontalSpeed;
            _rigidbody.velocity = velocity;
        }
        
        public void Jump()
        {
            if (_isJumping) return;
            _isJumping = true;
            _rigidbody.AddForce(Vector2.up * _jumpForce);
        }
        private void SetDirection(float direction)
        {
            if (
                (_direction == Direction.Right && direction < 0)
                || (_direction == Direction.Left && direction > 0)
                )
            {
                Flip();
            }   
        }

        private void Flip()
        {
            transform.Rotate(0, 180, 0);
            _direction = _direction == Direction.Left ? Direction.Right : Direction.Left;
            foreach (var cameraPair in _cameras.DirectionalCameras)
            {
                cameraPair.Value.enabled = cameraPair.Key != _direction; // if player goes right, we want to use left camera
            }
        }

        public bool IsGrounded()
        {
            float extraHeight = 0f;
            RaycastHit2D boxCastHit = Physics2D.CapsuleCast(_collider.bounds.center, _collider.bounds.size, 0, 0f, Vector2.down, extraHeight, _platformLayerMask);
            return boxCastHit.collider != null;
        }
        private void UpdateJump()
        {
            if (_rigidbody.velocity.y < 0 && IsGrounded()) // add is grounded
            {
                ResetJump();
                return;
            }
        }
        
        private void ResetJump()
        {
            _isJumping = false;            

        }

        private void PlayAnimation(AnimationType animationType, bool active)
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

        private void PlayAnimation(AnimationType animationType)
        {
            _animator.SetInteger(nameof(AnimationType), (int)animationType);
        }
    }
}