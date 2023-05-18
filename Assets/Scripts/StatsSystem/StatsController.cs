using System;
using System.Collections.Generic;
using System.Linq;
using Core.Services.Updater;
using StatsSystem.Enum;
using UnityEngine;

namespace StatsSystem
{
    public class StatsController : IDisposable, IStatValueGiver
    {
        private readonly List<StatModificator> _activeModificators;
        private readonly List<Stat> _currentStats;

        public StatsController(List<Stat> currentStats)
        {
            _currentStats = currentStats;
            _activeModificators = new List<StatModificator>();
            ProjectUpdater.Instance.UpdateCalled += OnUpdate;
        }

        public void Dispose()
        {
            ProjectUpdater.Instance.UpdateCalled -= OnUpdate;
        }

        public float GetStatValue(StatType statType)
        {
            return _currentStats.Find(stat => stat.Type == statType);
        }

        public void ProcessModificator(StatModificator modificator)
        {
            var statToChange = _currentStats.Find(stat => stat.Type == modificator.Stat.Type);
            var newValue = modificator.Type == StatModificatorType.Additive ? 
                statToChange + modificator.Stat : statToChange * modificator.Stat;

            var addedValue = newValue - statToChange;
            statToChange.SetStatValue(statToChange + addedValue);
            
            if (modificator.Duration < 0) return;

            if (_activeModificators.Contains(modificator))
            {
                _activeModificators.Remove(modificator);
            }
            else
            {
                var addedStat = new Stat(modificator.Stat.Type, -addedValue);
                var tempModificator = new StatModificator(addedStat, StatModificatorType.Additive, modificator.Duration,
                    Time.time);

                _activeModificators.Add(tempModificator);
            }
        }

        private void OnUpdate()
        {
            if (_activeModificators.Count == 0) return;

            var expiredModificators =
                _activeModificators.Where(modificator => modificator.StartTime + modificator.Duration <= Time.time);

            foreach (var expiredModificator in expiredModificators) ProcessModificator(expiredModificator);
        }
    }
}