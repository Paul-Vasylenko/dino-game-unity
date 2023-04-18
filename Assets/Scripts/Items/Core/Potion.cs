using System;
using Items.Data;
using StatsSystem;

namespace Items.Core
{
    public class Potion : Item
    {
        private readonly StatsController _statsController;
        private int _amount;
        private readonly StatChangingItemDescriptor _itemDescriptor;

        public Potion(ItemDescriptor descriptor, StatsController statsController) : base(descriptor)
        {
            _itemDescriptor = descriptor as StatChangingItemDescriptor;
            _statsController = statsController;
            _amount = 1;
        }

        public override int Amount => _amount;

        public override void Use()
        {
            _amount--;
            foreach (var stat in _itemDescriptor.Stats)
                _statsController.ProcessModificator(stat);
            if (_amount <= 0)
                Destroy();
        }

        private void Destroy()
        {
            throw new NotImplementedException();
        }

        public void AddToStack()
        {
            _amount += Amount;
        }

        public void AddToStack(int amount)
        {
            _amount += amount;
        }
    }
}