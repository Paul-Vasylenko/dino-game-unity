using Items.Data;
using UnityEngine;

namespace Items.Scriptable
{
    [CreateAssetMenu(fileName = "Item", menuName = "ItemsSystem/Item")]
    public class ItemScriptable : BaseItemScriptable
    {
        [SerializeField] private StatChangingItemDescriptor _itemDescriptor;

        public ItemScriptable(StatChangingItemDescriptor descriptor)
        {
            _itemDescriptor = descriptor;
        }

        public override ItemDescriptor ItemDescriptor => _itemDescriptor;
    }
}