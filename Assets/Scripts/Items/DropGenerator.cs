using System.Collections.Generic;
using System.Linq;
using Core.Services.Updater;
using Items.Data;
using Items.Enum;
using Player;
using UnityEngine;

namespace Items
{
    public class DropGenerator 
    {
        private readonly PlayerEntity _playerEntity;
        private readonly List<ItemDescriptor> _itemsDescriptors;
        private readonly ItemsSystem _itemsSystem;
        
        public DropGenerator(List<ItemDescriptor> itemDescriptors, PlayerEntity playerEntity, ItemsSystem itemsSystem)
        {
            _playerEntity = playerEntity;
            _itemsDescriptors = itemDescriptors;
            _itemsSystem = itemsSystem;
            ProjectUpdater.Instance.UpdateCalled += Update;
        }
        
        private void DropRandomItem(ItemRarity rarity)
        {
            List<ItemDescriptor> items = _itemsDescriptors.Where(item => item.ItemRarity == rarity).ToList();
            if (!items.Any()) return;
            
            ItemDescriptor itemDescriptor = items[Random.Range(0, items.Count())];
            _itemsSystem.DropItem(itemDescriptor, _playerEntity.transform.position);

        }
        
        private ItemRarity GetDropRarity()
        {
            float chance = Random.Range(0, 100);
            return chance switch
            {
                <= 50 => ItemRarity.Common,
                > 50 and <= 75 => ItemRarity.Rare,
                > 75 and <= 90 => ItemRarity.Epic,
                > 90 and <= 97 => ItemRarity.Legendary,
                > 97 and <= 100 => ItemRarity.Immortal,
                _ => ItemRarity.Common
            };
        }

        private void Update()
        {
            if(Input.GetKeyUp(KeyCode.G))
                DropRandomItem(GetDropRarity());
        }
    }
}