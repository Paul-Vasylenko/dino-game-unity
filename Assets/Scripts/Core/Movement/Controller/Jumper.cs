using Core.Movement.Data;
using StatsSystem;
using StatsSystem.Enum;
using UnityEngine;

namespace Core.Movement.Controller
{
    public class Jumper
    {
        private readonly JumpData _jumpData;
        private readonly float _minVelocityAfterJump = 0.01f;
        private readonly float _rayHeight = 1.2f;
        private readonly Rigidbody2D _rigidbody;

        public Jumper(Rigidbody2D rigidbody, JumpData jumpData)
        {
            _rigidbody = rigidbody;
            _jumpData = jumpData;
        }

        public bool IsJumping { get; private set; }

        public void Jump(float jumpForce)
        {
            if (IsJumping) return;
            IsJumping = true;
            _rigidbody.AddForce(Vector2.up * jumpForce);
        }

        public void UpdateJump()
        {
            if (_rigidbody.velocity.y < _minVelocityAfterJump && IsGrounded())
            {
                ResetJump();
            }
        }

        private bool IsGrounded()
        {
            return Physics2D.Raycast(_rigidbody.transform.position, Vector2.down, _rayHeight,
                _jumpData.PlatformLayerMask.value);
        }

        private void ResetJump()
        {
            IsJumping = false;
        }
    }
}