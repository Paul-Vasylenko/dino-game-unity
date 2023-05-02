using System;
using System.Collections.Generic;
using System.Linq;
using Items.Core;
using UnityEngine;

namespace Items
{
    public class Inventory
    {
        public const int InventorySize = 30;
        private readonly EquipmentConditionChecker _equipmentFitter;
        private readonly Transform _player;

        public List<Item> BackPackItems { get; }
        public List<Equipment> Equipment { get; }

        public event Action<Item, Vector2> ItemDropped;
        public event Action BackpackChanged;
        public event Action EquipmentChanged;

        public Inventory(List<Item> items, List<Equipment> equipment, Transform player, EquipmentConditionChecker equipmentFitter)
        {
            _equipmentFitter = equipmentFitter;
            BackPackItems = new List<Item>();
            for (var i = 0; i < InventorySize; i++)
                BackPackItems.Add(null);

            Equipment = new List<Equipment>();
            _player = player;
        }

        public bool TryAddToInventory(Item item)
        {
            if (item is Equipment equipment &&
               Equipment.All(equip => equip.EquipmentType != equipment.EquipmentType)
               && TryEquip(equipment))
                return true;

            return TryAddToBackPack(item);
        }

        public bool TryEquip(Item item)
        {
            Debug.Log(1);
            if (!(item is Equipment equipment))
                return false;
            Debug.Log(2);

            #region InventoryScreen
            if (!_equipmentFitter.TryReplaceEquipment(equipment, Equipment, out var oldEquipment))
                return false;

            if (oldEquipment != null)
                UnEquip(oldEquipment);

            if (BackPackItems.Contains(equipment))
            {
                var indexOfItem = BackPackItems.IndexOf(equipment);
                PlaceToBackPack(oldEquipment, indexOfItem);
            }
            else TryAddToBackPack(oldEquipment);
            #endregion

            Equipment.Add(equipment);
            equipment.Use();
            EquipmentChanged?.Invoke();
            return true;
        }

        private bool TryAddToBackPack(Item item)
        {
            if (BackPackItems.All(slot => slot != null))
                return false;

            var index = BackPackItems.IndexOf(null);
            PlaceToBackPack(item, index);
            return true;
        }

        #region Model


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
                if (TryAddToBackPack(equipment))
                    UnEquip(equipment);

                return;
            }

            if (!TryEquip(equipment))
                return;

            BackPackItems.Remove(item);
            BackpackChanged?.Invoke();
        }

        public void RemoveItem(Item item, bool toWorld)
        {
            if (item is Equipment equipment && Equipment.Contains(equipment))
                UnEquip(equipment);
            else
                RemoveFromBackBack(item);

            if (toWorld)
                ItemDropped?.Invoke(item, _player.position);
        }

        public void MoveItemToPositionInBackPack(Item item, int place)
        {
            if (item is Equipment equipment)
            {
                var backPackItem = BackPackItems[place];
                if (backPackItem != null)
                {
                    TryEquip(backPackItem);
                    return;
                }

                if (TryPlaceToBackPack(item, place))
                    UnEquip(equipment);

                return;
            }

            TryPlaceToBackPack(item, place);
        }

        private void UnEquip(Equipment equipment)
        {
            Equipment.Remove(equipment);
            equipment.Use();
            EquipmentChanged?.Invoke();
        }

        private bool TryPlaceToBackPack(Item item, int index)
        {
            var oldItem = BackPackItems[index];
            if (BackPackItems.Contains(item))
            {
                var indexOfItem = BackPackItems.IndexOf(item);
                BackPackItems[indexOfItem] = oldItem;
            }
            else if (oldItem != null)
                return false;

            BackPackItems[index] = item;
            BackpackChanged?.Invoke();
            return true;
        }

        private void PlaceToBackPack(Item item, int index)
        {
            BackPackItems[index] = item;
            BackpackChanged?.Invoke();
        }

        private void RemoveFromBackBack(Item item)
        {
            var index = BackPackItems.IndexOf(item);
            BackPackItems[index] = null;
            BackpackChanged?.Invoke();
        }

        #endregion
    }
}