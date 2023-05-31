using System.Collections.Generic;
using System.Linq;
using Core.Services.Updater;
using Items;
using StatsSystem;
using StatsSystem.Enum;
using UI.Core;
using UI.StatsUI.Element;
using UnityEngine;

namespace UI.StatsUI
{
    public class StatsScreenAdapter : ScreenController<StatsScreenView>
    {
        private readonly List<StatSlot> _backPackSlots;
        private readonly StatsController _statsController;

        public StatsScreenAdapter(StatsScreenView view, StatsController statsController) : base(view)
        {
            _backPackSlots = new List<StatSlot>();
            _statsController = statsController;
        }

    public override void Initialize()
        {
            InitializeBackPack();
            View.CloseClicked += RequestClose;
            _statsController.Updated += UpdateBackpack;
            base.Initialize();
        }

        public override void Complete()
        {
            ClearBackPack();
            View.CloseClicked -= RequestClose;
            _statsController.Updated -= UpdateBackpack;
            base.Complete();
        }

        private void InitializeBackPack()
        {
            var backPack = View.StatSlots;
            var statTypes = (StatType[])System.Enum.GetValues(typeof(StatType));
            statTypes = statTypes.Where((statType) => statType != StatType.None).ToArray();

            for (int i = 0; i < statTypes.Length; i++)
            {
                var statType = statTypes[i];
                var slot = backPack[i];

                var value = _statsController.GetStatValue(statType);
                slot.SetName(statType.ToString());
                slot.SetValue(value);
                _backPackSlots.Add(slot);
            }
        }

        private void UpdateBackpack()
        {
            ClearBackPack();
            InitializeBackPack();
        }

        private void ClearBackPack()
        {
            _backPackSlots.Clear();
        }
    }
}