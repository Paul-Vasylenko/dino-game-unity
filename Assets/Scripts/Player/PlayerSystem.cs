﻿using System;
using System.Collections.Generic;
using System.Linq;
using Core.Services.Updater;
using InputReader;
using Items;
using StatsSystem;
using UnityEngine;

namespace Player
{
    public class PlayerSystem : IDisposable
    {
        private readonly List<IDisposable> _disposables;
        private readonly PlayerBrain _playerBrain;

        private readonly ProjectUpdater _projectUpdater;
        private readonly PlayerEntity _playerEntity;
        public StatsController StatsController { get; }
        public Inventory Inventory { get; }

        public PlayerSystem(PlayerEntity playerEntity, List<IEntityInputSource> inputSources)
        {
            _disposables = new List<IDisposable>();

            var statStorage = Resources.Load<StatsStorage>($"Player/{nameof(StatsStorage)}");
            // Next line is needed to work with stats, not with their links
            var stats = statStorage.Stats.Select(stat => stat.GetCopy()).ToList();
            StatsController = new StatsController(stats);
            _disposables.Add(StatsController);

            _playerEntity = playerEntity;
            _playerEntity.Initialize(StatsController);
            _playerBrain = new PlayerBrain(_playerEntity, inputSources);
            _disposables.Add(_playerBrain);

            Inventory = new Inventory(null, null, _playerEntity.transform, new EquipmentConditionChecker());
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables) disposable.Dispose();
        }
    }
}