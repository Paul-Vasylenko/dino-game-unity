using System;
using System.Collections.Generic;
using Items.Enum;
using StatsSystem;
using UnityEngine;

namespace Items.Data
{
    [Serializable]
    public class WeaponDescriptor : StatChangingItemDescriptor
    {
        public WeaponDescriptor(WeaponType weaponType, ItemId itemId, ItemType type, Sprite itemSprite,
            ItemRarity itemRarity, float price, float level, List<StatModificator> stats) :
            base(itemId, type, itemSprite, itemRarity, price, level, stats)
        {
            WeaponType = weaponType;
        }

        [field: SerializeField] public WeaponType WeaponType { get; private set; }
    }
}