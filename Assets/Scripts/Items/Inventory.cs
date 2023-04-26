using System;
using System.Collections.Generic;
using System.Linq;
using Items.Core;
using UnityEngine;

namespace Items
{
    public class Inventory
    {
        public int InventorySize = 32;
        private readonly Transform _player;
        public List<Item> BackpackItems { get; }
        public List<Equipment> Equipment { get; }
        public event Action<Item, Vector2> ItemDropped;
        public event Action BackpackChanged;
        public event Action EquipmentChanged;

        public Inventory(List<Item> backpackItems, List<Equipment> equipment, Transform player)
        {
            _player = player;
            Equipment = equipment ?? new List<Equipment>();
            if (backpackItems != null) return;
            
            BackpackItems = new List<Item>();
            for (var i = 0; i < InventorySize; i++) 
                BackpackItems.Add(null);
        }

        public bool TryAddToBackPack(Item item)
        {
            if (BackpackItems.All(slot => slot != null))
                return false;

            var index = BackpackItems.IndexOf(null);
            PlaceToBackPack(item, index);
            return true;
        }
        
        public bool TryAddToInventory(Item item)
        {
            if(item is Equipment equipment && 
               Equipment.All(equip => equip.EquipmentType != equipment.EquipmentType)
               && TryEquip(equipment))
                return true;

            return TryAddToBackPack(item);
        }

        public bool TryEquip(Item item)
        {
            if (!(item is Equipment equipment))
                return false;
            var oldEquipment = Equipment.Find(slot => slot.EquipmentType == equipment.EquipmentType);

            if (oldEquipment == null)
            {
                Equipment.Add(equipment);
                equipment.Use();
                EquipmentChanged?.Invoke();
                return true;
            }
            
            if (BackpackItems.Contains(equipment))
            {
                var indexOfItem = BackpackItems.IndexOf(equipment);
                PlaceToBackPack(oldEquipment, indexOfItem);
            }
            else TryAddToBackPack(oldEquipment);

            return false;
        }
        
        public void UseItem(Item item)
        {
            if (item is Potion potion)
            {
                potion.Use();
                if (potion.Amount <= 0)
                    RemoveItem(item, false);
                return;
            }

            if (item is not Equipment equipment) 
                return;

            if (Equipment.Contains(equipment))
            {
                if(TryAddToBackPack(equipment))
                    UnEquip(equipment);
                
                return;
            }

            if (!TryEquip(equipment)) 
                return;
            
            BackpackItems.Remove(item);
            BackpackChanged?.Invoke();
        }
        
        public void RemoveItem(Item item, bool toWorld)
        {
            if (item is Equipment equipment && Equipment.Contains(equipment))
                UnEquip(equipment);
            else 
                RemoveFromBackBack(item);
            
            if(toWorld)
                ItemDropped?.Invoke(item, _player.position);
        }

        private void UnEquip(Equipment equipment)
        {
            Equipment.Remove(equipment);
            equipment.Use();
            EquipmentChanged?.Invoke();
        }

        public void RemoveFromBackpack(Item item, bool toWorld)
        {
            var index = BackpackItems.IndexOf(item);
            BackpackItems[index] = null;
            BackpackChanged?.Invoke();
            
            if(toWorld) ItemDropped?.Invoke(item, _player.position);

        }

        public int GetBackpackSize()
        {
            return BackpackItems.FindAll(item => item != null).Count;
        }
        
        public void MoveItemToPositionInBackPack(Item item, int place)
        {
            if (item is Equipment equipment)
            {
                var backPackItem = BackpackItems[place];
                if (backPackItem != null)
                {
                    TryEquip(backPackItem);
                    return;
                }
                
                if(TryPlaceToBackPack(item, place))
                    UnEquip(equipment);
                
                return;
            }

            TryPlaceToBackPack(item, place);
        }
        
        private void PlaceToBackPack(Item item, int index)
        {
            BackpackItems[index] = item;
            BackpackChanged?.Invoke();
        }
        
        private bool TryPlaceToBackPack(Item item, int index)
        {
            var oldItem = BackpackItems[index];
            if (BackpackItems.Contains(item))
            {
                var indexOfItem = BackpackItems.IndexOf(item);
                BackpackItems[indexOfItem] = oldItem;
            }
            else if (oldItem != null)
                return false;

            BackpackItems[index] = item;
            BackpackChanged?.Invoke();
            return true;
        }
        
        private void RemoveFromBackBack(Item item)
        {
            var index = BackpackItems.IndexOf(item);
            BackpackItems[index] = null;
            BackpackChanged?.Invoke();   
        }
    }
}