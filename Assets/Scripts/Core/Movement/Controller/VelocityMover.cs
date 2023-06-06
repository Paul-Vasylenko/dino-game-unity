using UnityEngine;
using Core.Enums;

namespace Core.Movement.Controller
{
    public class VelocityMover : HorizontalMover
    {
        private Vector2 _movement;
        public override bool IsMoving => _movement.magnitude > 0;

        public VelocityMover(Rigidbody2D rigidbody) : base(rigidbody) { }

        public override void MoveHorizontally(float horizontalMovement)
        {
            _movement.x = horizontalMovement;
            Vector2 velocity = Rigidbody.velocity;
            velocity.x = horizontalMovement;
            Rigidbody.velocity = velocity;
            
            if (horizontalMovement == 0)
                return;

            SetDirection(horizontalMovement > 0 ? Direction.Right : Direction.Left);
        }
    }
}
