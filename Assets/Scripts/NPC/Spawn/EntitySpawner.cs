using System;
using System.Collections.Generic;
using Drawing;
using Items;
using NPC.Controller;
using NPC.Data;
using NPC.Enum;
using UnityEngine;

namespace NPC.Spawn
{
    public class EntitySpawner : IDisposable
    {
        private readonly LevelDrawer _levelDrawer;
        private readonly DropGenerator _dropGenerator;
        public readonly List<Entity> Entities;
        private readonly EntitiesFactory _entitiesFactory;

        public EntitySpawner(LevelDrawer levelDrawer, DropGenerator dropGenerator)
        {
            _levelDrawer = levelDrawer;
            _dropGenerator = dropGenerator;
            Entities = new List<Entity>();
            var entitiesSpawnerDataStorage = Resources.Load<EntitiesSpawnerDataStorage>($"{nameof(EntitySpawner)}/{nameof(EntitiesSpawnerDataStorage)}");
            _entitiesFactory = new EntitiesFactory(entitiesSpawnerDataStorage);
        }

        public void SpawnEntity(EntityId entityId, Vector2 position)
        {
            var entity = _entitiesFactory.GetEntityBrain(entityId, position);
            entity.Died += RemoveEntity;
            entity.Died += SpawnRandomDrop;
            _levelDrawer.RegisterElement(entity);
            Entities.Add(entity);
        }

        private void SpawnRandomDrop(Entity entity)
        {
            _dropGenerator.DropRandomItem();
        }

        public void Dispose()
        {
            foreach (var entity in Entities)
                DestroyEntity(entity);
            Entities.Clear();
        }

        private void RemoveEntity(Entity entity)
        {
            Entities.Remove(entity);
            DestroyEntity(entity);
        }

        private void DestroyEntity(Entity entity)
        {
            _levelDrawer.UnregisterElement(entity);
            entity.Died -= RemoveEntity;
            entity.Died -= SpawnRandomDrop;
            entity.Destroy();
        }
    }
}