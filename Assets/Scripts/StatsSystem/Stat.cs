using System;
using StatsSystem.Enum;
using UnityEngine;

namespace StatsSystem
{
    [Serializable]
    public class Stat
    {
        public Stat(StatType statType, float value)
        {
            Type = statType;
            Value = value;
        }

        [field: SerializeField] public StatType Type { get; private set; }
        [field: SerializeField] public float Value { get; private set; }

        public void SetStatValue(float value)
        {
            Value = value;
        }

        public static implicit operator float(Stat stat)
        {
            return stat?.Value ?? 0;
        }

        public Stat GetCopy()
        {
            return new(Type, Value);
        }
    }
}