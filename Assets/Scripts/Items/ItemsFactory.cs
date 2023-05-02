using System;
using Items.Core;
using Items.Data;
using Items.Enum;
using StatsSystem;

namespace Items
{
    public class ItemsFactory
    {
        private readonly StatsController _statsController;
        public ItemsFactory(StatsController statsController) => _statsController = statsController;

        public Item CreateItem(ItemDescriptor descriptor)
        {
            switch (descriptor.Type)
            {
                case ItemType.Potion:
                    return new Potion(descriptor, _statsController);
                case ItemType.Hat:
                case ItemType.Boots:
                default:
                    throw new NullReferenceException($"Item type {descriptor.Type} is not implemented yet");
            }
        }

        private EquipmentType GetEquipmentType(ItemDescriptor descriptor)
        {
            switch (descriptor.Type)
            {
                case ItemType.Hat:
                    return EquipmentType.Hat;
                case ItemType.Boots:
                    return EquipmentType.Boots;
                case ItemType.None:
                case ItemType.Potion:
                default:
                    return EquipmentType.None;
            }
        }
    }
}