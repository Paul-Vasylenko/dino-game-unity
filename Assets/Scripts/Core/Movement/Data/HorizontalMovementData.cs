using System;
using Core.Enums;
using UnityEngine;

namespace Core.Movement.Data
{
    [Serializable]
    public class HorizontalMovementData
    {
        [field: SerializeField] public float HorizontalSpeed { get; private set;  }
        [field: SerializeField] public Direction Direction { get; private set;  }
    }
}