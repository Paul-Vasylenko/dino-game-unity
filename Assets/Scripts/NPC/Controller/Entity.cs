using System;
using Drawing;
using NPC.Behaviour;
using StatsSystem;
using UnityEngine;

namespace NPC.Controller
{
    public abstract class Entity : ILevelGraphicElement
    {
        private readonly BaseEntityBehaviour _entityBehaviour;
        protected readonly StatsController StatsController;

        public event Action<Entity> Died;

        public event Action<ILevelGraphicElement> VerticalPositionChanged;

        protected Entity(BaseEntityBehaviour entityBehaviour, StatsController statsController)
        {
            _entityBehaviour = entityBehaviour;
            _entityBehaviour.Initialize();
            StatsController = statsController;
        }

        public void SetDrawingOrder(int order) => _entityBehaviour.SetDrawingOrder(order);

        public virtual void Dispose() => StatsController.Dispose();
    }
}