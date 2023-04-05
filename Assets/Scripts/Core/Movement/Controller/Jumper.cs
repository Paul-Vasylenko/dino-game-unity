using Core.Movement.Data;
using StatsSystem;
using StatsSystem.Enum;
using UnityEngine;

namespace Core.Movement.Controller
{
    public class Jumper
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly JumpData _jumpData;
        private readonly Collider2D _collider;
        private readonly IStatValueGiver _statValueGiver;
        private readonly float _rayHeight = 1.2f;
        private readonly float _minVelocityAfterJump = 0.01f;

        public Jumper(Rigidbody2D rigidbody, JumpData jumpData, Collider2D collider, IStatValueGiver statValueGiver)
        {
            _rigidbody = rigidbody;
            _jumpData = jumpData;
            _collider = collider;
            _statValueGiver = statValueGiver;
        }

        public bool IsJumping { get; private set; }
        public void Jump()
        {
            if (IsJumping) return;
            IsJumping = true;
            _rigidbody.AddForce(Vector2.up * _statValueGiver.GetStatValue(StatType.JumpForce));
        }

        public void UpdateJump()
        {
            if (_rigidbody.velocity.y < _minVelocityAfterJump && IsGrounded())
            {
                ResetJump();
                return;
            }
        }

        private bool IsGrounded() => Physics2D.Raycast(_rigidbody.transform.position, Vector2.down, _rayHeight, _jumpData.PlatformLayerMask.value);

        private void ResetJump()
        {
            IsJumping = false;
        }
    }
}