using System;
using System.Collections.Generic;
using System.Linq;
using InputReader;
using StatsSystem;
using UnityEngine;

namespace Player
{
    public class PlayerSystem : IDisposable
    {
        private readonly List<IDisposable> _disposables;
        private readonly PlayerBrain _playerBrain;
        private readonly PlayerEntity _playerEntity;
        private readonly StatsController _statsController;

        public PlayerSystem(PlayerEntity playerEntity, List<IEntityInputSource> inputSources)
        {
            _disposables = new List<IDisposable>();

            var statStorage = Resources.Load<StatsStorage>($"Player/{nameof(StatsStorage)}");
            // Next line is needed to work with stats, not with their links
            var stats = statStorage.Stats.Select(stat => stat.GetCopy()).ToList();
            _statsController = new StatsController(stats);
            _disposables.Add(_statsController);

            _playerEntity = playerEntity;
            _playerEntity.Initialize(_statsController);
            _playerBrain = new PlayerBrain(_playerEntity, inputSources);
            _disposables.Add(_playerBrain);
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables) disposable.Dispose();
        }
    }
}