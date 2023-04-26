using System.Collections.Generic;
using Items.Behaviour;
using Items.Core;
using Items.Data;
using Items.Rarity;
using UnityEngine;

namespace Items
{
    public class ItemsSystem
    {
        private readonly SceneItem _sceneItem;
        private readonly Transform _transform;
        private readonly List<IItemRarityColor> _colors;
        private readonly LayerMask _whatIsPlayer;
        private readonly ItemsFactory _itemsFactory;
        private readonly Inventory _inventory;
        
        private readonly Dictionary<SceneItem, Item> _itemsOnScene;
        
        public ItemsSystem(List<IItemRarityColor> colors, ItemsFactory itemsFactory, LayerMask whatIsPlayer, Inventory inventory)
        {
            _sceneItem = Resources.Load<SceneItem>($"{nameof(ItemsSystem)}/{nameof(SceneItem)}");
            _itemsOnScene = new Dictionary<SceneItem, Item>();
            GameObject gameObject = new GameObject();
            gameObject.name = nameof(ItemsSystem);
            _transform = gameObject.transform;
            _colors = colors;
            _whatIsPlayer = whatIsPlayer;
            _itemsFactory = itemsFactory;
            _inventory = inventory;
            _inventory.ItemDropped += DropItem;
        }

        public void DropItem(ItemDescriptor descriptor, Vector2 position) =>
            DropItem(_itemsFactory.CreateItem(descriptor), position);

        private void DropItem(Item item, Vector2 position)
        {
            SceneItem sceneItem = Object.Instantiate(_sceneItem, _transform);
            sceneItem.SetItem(item.Descriptor.ItemSprite, item.Descriptor.ItemId.ToString(), 
                _colors.Find(color => color.ItemRarity == item.Descriptor.ItemRarity).Color);
            sceneItem.PlayDrop(position);
            sceneItem.ItemClicked += TryPickItem;
            _itemsOnScene.Add(sceneItem, item);
        }
        
        private void TryPickItem(SceneItem sceneItem)
        {
            Collider2D player = 
                Physics2D.OverlapCircle(sceneItem.Position, sceneItem.InteractionDistance, _whatIsPlayer);
            Debug.Log(123);

            if (player == null)
                return;
            Debug.Log(123);

            Item item = _itemsOnScene[sceneItem];
            Debug.Log(_inventory.GetBackpackSize());
            Debug.Log(_inventory.InventorySize);

            if (_inventory.GetBackpackSize() >= _inventory.InventorySize) return;
            Debug.Log(123);

            if (!_inventory.TryAddToInventory(item))
                return;
            
            Debug.Log(_inventory.BackpackItems.Count);
            _itemsOnScene.Remove(sceneItem);
            sceneItem.ItemClicked -= TryPickItem;
            Object.Destroy(sceneItem.gameObject);
        }
    }
}