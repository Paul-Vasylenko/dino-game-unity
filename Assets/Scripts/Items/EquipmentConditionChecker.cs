using System;
using System.Collections.Generic;
using Items.Core;
using Items.Enum;

namespace Items
{
    public class EquipmentConditionChecker
    {
        public bool IsEquipmentConditionFits(Equipment equipment, List<Equipment> currentEquipment)
        {
            return true;
        }

        public bool TryReplaceEquipment(Equipment equipment, List<Equipment> currentEquipment, out Equipment oldEquipment)
        {
            oldEquipment = currentEquipment.Find(slot => slot.EquipmentType == equipment.EquipmentType);
            if (oldEquipment != null)
                return true;

            switch (equipment.EquipmentType)
            {
                case EquipmentType.BothHands:
                    {
                        var hand = currentEquipment.Find(slot => slot.EquipmentType == EquipmentType.OneHand);
                        if (hand != null)
                            return false;

                        oldEquipment = hand;
                        return true;
                    }
                case EquipmentType.OneHand:
                    oldEquipment = currentEquipment.Find(slot => slot.EquipmentType == EquipmentType.BothHands);
                    return true;
                case EquipmentType.Hat:
                case EquipmentType.Boots:
                    return true;
                case EquipmentType.None:
                default:
                    throw new NullReferenceException($"Equipment type of item {equipment.Descriptor.ItemId} is not available for equipping");
            }
        }
    }
}