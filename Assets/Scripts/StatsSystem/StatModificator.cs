using System;
using StatsSystem.Enum;
using UnityEngine;

namespace StatsSystem
{
    [Serializable]
    public class StatModificator
    {
        public StatModificator(Stat stat, StatModificatorType type, float duration)
        {
            Stat = stat;
            Type = type;
            Duration = duration;
        }

        public StatModificator(Stat stat, StatModificatorType statModificatorType, float duration, float startTime)
        {
            Stat = stat;
            Type = statModificatorType;
            Duration = duration;
            StartTime = startTime;
        }

        [field: SerializeField] public Stat Stat { get; private set; }
        [field: SerializeField] public StatModificatorType Type { get; private set; }
        [field: SerializeField] public float Duration { get; private set; }

        public float StartTime { get; }

        public StatModificator GetReverseModificator()
        {
            var reverseStat = new Stat(Stat.Type, Type == StatModificatorType.Additive ? -Stat : 1 / Stat);
            return new StatModificator(reverseStat, Type, Duration);
        }
    }
}