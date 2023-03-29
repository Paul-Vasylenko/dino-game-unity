using System;
using UnityEngine;

namespace Core.Movement.Data
{
    [Serializable]
    public class JumpData
    {
        [field: SerializeField] public float JumpForce { get; private set; }
        [field: SerializeField] public LayerMask PlatformLayerMask { get; private set; }
    }
}