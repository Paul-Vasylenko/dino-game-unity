using Core.Enums;
using Core.Movement.Data;
using StatsSystem;
using StatsSystem.Enum;
using UnityEngine;

namespace Core.Movement.Controller
{
    public abstract class HorizontalMover
    {
        protected readonly Rigidbody2D Rigidbody;
        public Direction Direction { get; private set; }

        public HorizontalMover(Rigidbody2D rigidbody)
        {
            Rigidbody = rigidbody;
            Direction = Direction.Right;
        }

        public abstract bool IsMoving { get; }

        public abstract void MoveHorizontally(float horizontalMovement);

        public void SetDirection(Direction direction)
        {
            if (Direction != direction)
                Flip();
        }

        private void Flip()
        {
            Rigidbody.transform.Rotate(0, 180, 0);
            Direction = Direction == Direction.Left ? Direction.Right : Direction.Left;
        }
    }
}