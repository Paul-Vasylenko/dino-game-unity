using System.Collections.Generic;
using NPC.Behaviour;
using NPC.Enum;
using StatsSystem;
using UnityEngine;

namespace NPC.Data
{
    [CreateAssetMenu(fileName = nameof(EntityDataStorage), menuName = ("EntitiesSpawner/EntityDataStorage"))]
    public class EntityDataStorage : ScriptableObject
    {
        [field: SerializeField] public EntityId Id { get; private set; }
        [field: SerializeField] public List<Stat> Stats { get; private set; }
        [field: SerializeField] public BaseEntityBehaviour EntityBehaviourPrefab { get; private set; }
    }
}