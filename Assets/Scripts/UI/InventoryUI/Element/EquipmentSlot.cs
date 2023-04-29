using Items.Enum;
using UnityEngine;

namespace UI.InventoryUI.Element
{
    public class EquipmentSlot : ItemSlot
    {
        [field: SerializeField] public EquipmentType EquipmentType { get; private set; }

        public void SetAfterImage(Sprite sprite, Sprite backSprite)
        {
            SetItem(sprite, backSprite, -1);
            ClearButton.gameObject.SetActive(false);
        }
    }
}