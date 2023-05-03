﻿using System;
using System.Collections.Generic;
using System.Linq;
using Core.Services.Updater;
using InputReader;

namespace Player
{
    public class PlayerBrain : IDisposable
    {
        private readonly List<IEntityInputSource> _inputSources;
        private readonly PlayerEntity _playerEntity;

        public PlayerBrain(PlayerEntity playerEntity, List<IEntityInputSource> inputSources)
        {
            _playerEntity = playerEntity;
            _inputSources = inputSources;
            ProjectUpdater.Instance.FixedUpdateCalled += OnFixedUpdate;
        }

        private bool IsJump => _inputSources.Any(source => source.Jump);
        private bool IsKick => _inputSources.Any(source => source.Kick);
        private bool IsBite => _inputSources.Any(source => source.Bite);

        public void Dispose()
        {
            ProjectUpdater.Instance.FixedUpdateCalled -= OnFixedUpdate;
        }

        private void OnFixedUpdate()
        {
            _playerEntity.MoveHorizontally(GetHorizontalDirection());

            if (IsJump)
                _playerEntity.Jump();
            
            if(IsKick)
                _playerEntity.StartKick();
            
            if(IsBite)
                _playerEntity.StartBite();

            foreach (var inputSource in _inputSources) inputSource.ResetOneTimeActions();
        }

        private float GetHorizontalDirection()
        {
            foreach (var inputSource in _inputSources)
            {
                if (inputSource.HorizontalDirection == 0)
                    continue;

                return inputSource.HorizontalDirection;
            }

            return 0;
        }
    }
}