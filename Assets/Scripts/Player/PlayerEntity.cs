using System;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class PlayerEntity : MonoBehaviour
    {
        [Header("Horizontal Movement")]
        [SerializeField] private float _horizontalSpeed;
        [SerializeField] private bool _faceRight;

        [Header("Jumping")] 
        [SerializeField] private float _jumpForce;
        [SerializeField] private LayerMask _platformLayerMask;

        private Rigidbody2D _rigidbody;
        private Collider2D _collider;
        private bool _isJumping;

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<CapsuleCollider2D>();
        }

        private void Update()
        {
            if (_isJumping)
            {
                UpdateJump();
            }
        }

        private void FixedUpdate()
        {
        }

        public void MoveHorizontally(float direction)
        {
            SetDirection(direction);
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
            if ((_faceRight && direction < 0) || (!_faceRight && direction > 0))
            {
                Flip();
            }   
        }

        private void Flip()
        {
            transform.Rotate(0, 180, 0);
            _faceRight = !_faceRight;
        }

        public bool IsGrounded()
        {
            float extraHeight = 5f;
            RaycastHit2D boxCastHit = Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0f, Vector2.down, extraHeight, _platformLayerMask);
            Debug.Log(boxCastHit.collider);
            return boxCastHit.collider != null;
        }
        private void UpdateJump()
        {
            if (_rigidbody.velocity.y < 0) // add is grounded
            {
                ResetJump();
                return;
            }
        }
        
        private void ResetJump()
        {
            _isJumping = false;
        }
    }
}