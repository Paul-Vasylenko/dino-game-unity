using UnityEngine;
using Core.Enums;
using Core.Movement.Controller;

namespace Core.Movement.Controllers
{
    public class PositionMover : HorizontalMover
    {
        private Vector2 _destination;
        public override bool IsMoving => _destination.x != Rigidbody.position.x;

        public PositionMover(Rigidbody2D rigidbody) : base(rigidbody) { }

        public override void MoveHorizontally(float horizontalMovement)
        {
            _destination.x = horizontalMovement;
            var newPosition = new Vector2(horizontalMovement, Rigidbody.position.y);
            Rigidbody.MovePosition(newPosition);
            
            if (_destination.x != Rigidbody.position.x)
                SetDirection(_destination.x > Rigidbody.position.x ? Direction.Right : Direction.Left);
        }
    }
}