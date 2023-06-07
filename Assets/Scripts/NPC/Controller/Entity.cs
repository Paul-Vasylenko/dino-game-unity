using System;
using Drawing;
using NPC.Behaviour;
using StatsSystem;
using StatsSystem.Enum;
using UnityEngine;

namespace NPC.Controller
{
    public abstract class Entity : ILevelGraphicElement
    {
        private readonly BaseEntityBehaviour _entityBehaviour;
        protected readonly StatsController StatsController;

        public event Action<Entity> Died;

        public event Action<ILevelGraphicElement> VerticalPositionChanged;
        private float _currentHp;

        protected Entity(BaseEntityBehaviour entityBehaviour, StatsController statsController)
        {
            _entityBehaviour = entityBehaviour;
            _entityBehaviour.Initialize();
            StatsController = statsController;
            _currentHp = StatsController.GetStatValue(StatType.Health);
            SubscribeOnEvents();
        }

        public void SetDrawingOrder(int order) => _entityBehaviour.SetDrawingOrder(order);
        public abstract void VisualiseHp(float currentHp);

        private void OnDamageTaken(float damage)
        {
            _currentHp = Mathf.Clamp(_currentHp - damage, 0, _currentHp);
            VisualiseHp(_currentHp);
            if (_currentHp <= 0)
                Died?.Invoke(this);
        }
        
        private void SubscribeOnEvents() =>
            _entityBehaviour.DamageTaken += OnDamageTaken;
        
        private void UnsubscribeFromEvents() =>
            _entityBehaviour.DamageTaken -= OnDamageTaken;
        
        public void Dispose()
        {
            StatsController.Dispose();
            UnsubscribeFromEvents();
        }
    }
}