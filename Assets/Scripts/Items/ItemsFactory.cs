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
                case ItemType.Accessory:
                case ItemType.Weapon:
                    return new Equipment(descriptor, _statsController, GetEquipmentType(descriptor));
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
                case ItemType.Accessory:
                    return EquipmentType.Accessory;
                case ItemType.Weapon:
                    var weaponDescriptor = descriptor as WeaponDescriptor;
                    switch (weaponDescriptor.WeaponType)
                    {
                        case WeaponType.Bow:
                        case WeaponType.Spear:
                            return EquipmentType.BothHands;
                        case WeaponType.Knife:
                            return EquipmentType.OneHand;
                    }
                    throw new NullReferenceException("Weapon has wrong type");
                case ItemType.None:
                case ItemType.Potion:
                default:
                    return EquipmentType.None;
            }
        }
    }
}