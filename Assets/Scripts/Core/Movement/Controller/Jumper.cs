using Core.Movement.Data;
using UnityEngine;

namespace Core.Movement.Controller
{
    public class Jumper
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly JumpData _jumpData;
        private readonly Collider2D _collider;

        public Jumper(Rigidbody2D rigidbody, JumpData jumpData, Collider2D collider)
        {
            _rigidbody = rigidbody;
            _jumpData = jumpData;
            _collider = collider;
        }
        
        public bool IsJumping { get; private set; }
        public void Jump()
        {
            if (IsJumping) return;
            IsJumping = true;
            _rigidbody.AddForce(Vector2.up * _jumpData.JumpForce);
        }
        
        public void UpdateJump()
        {
            if (_rigidbody.velocity.y < 0 && IsGrounded()) // add is grounded
            {
                ResetJump();
                return;
            }
        }
        
        private bool IsGrounded()
        {
            float extraHeight = 0f;
            RaycastHit2D boxCastHit = Physics2D.CapsuleCast(_collider.bounds.center, _collider.bounds.size, 0, 0f, Vector2.down, extraHeight, _jumpData.PlatformLayerMask);
            return boxCastHit.collider != null;
        }
        
        private void ResetJump()
        {
            IsJumping = false;
        }
    }
}