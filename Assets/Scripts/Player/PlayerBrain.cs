using System;
using System.Collections.Generic;
using System.Linq;
using Core.Services.Updater;
using InputReader;
using NPC.Controller;
using StatsSystem;
using StatsSystem.Enum;

namespace Player
{
    public class PlayerBrain : Entity, IDisposable
    {
        private readonly List<IEntityInputSource> _inputSources;
        private readonly PlayerEntity _playerEntity;

        public PlayerBrain(PlayerEntity playerEntity, StatsController statsController,
            List<IEntityInputSource> inputSources) : base(playerEntity, statsController)
        {
            _playerEntity = playerEntity;
            _inputSources = inputSources;
            ProjectUpdater.Instance.FixedUpdateCalled += OnFixedUpdate;
        }

        private bool IsJump => _inputSources.Any(source => source.Jump);
        private bool IsKick => _inputSources.Any(source => source.Kick);
        private bool IsBite => _inputSources.Any(source => source.Bite);

        public override void VisualiseHp(float currentHp)
        {
            if (currentHp > _playerEntity.StatsUIView.HpBar.maxValue)
                _playerEntity.StatsUIView.HpBar.maxValue = currentHp;

            _playerEntity.StatsUIView.HpBar.value = currentHp;
        }

        public void Dispose()
        {
            ProjectUpdater.Instance.FixedUpdateCalled -= OnFixedUpdate;
        }

        private void OnFixedUpdate()
        {
            _playerEntity.MoveHorizontally(GetHorizontalDirection() * StatsController.GetStatValue(StatType.Speed));

            if (IsJump)
                _playerEntity.Jump(StatsController.GetStatValue(StatType.JumpForce));

            if (IsKick)
                _playerEntity.StartKick();

            if (IsBite)
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