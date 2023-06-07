using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UI.Core;
using UI.InventoryUI.Element;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.InventoryUI
{
    public class InventoryScreenView : ScreenView
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _statsButton;

        [SerializeField] private TMP_Text _potionsText;
        [SerializeField] private TMP_Text _moneyText;

        [SerializeField] private Transform _backPackContainer;
        [SerializeField] private Transform _equipmentContainer;

        public List<ItemSlot> ItemSlots { get; private set; }
        public List<EquipmentSlot> EquipmentSlots { get; private set; }


        [field: SerializeField] public Image MovingImage { get; private set; }

        public event Action CloseClicked;
        public event Action InventoryClicked;

        private void Awake()
        {
            _closeButton.onClick.AddListener(() => CloseClicked?.Invoke());
            _statsButton.onClick.AddListener(() => InventoryClicked?.Invoke());
            ItemSlots = GetComponentsInChildren<ItemSlot>().ToList();
            EquipmentSlots = GetComponentsInChildren<EquipmentSlot>().ToList();
        }

        private void OnDestroy()
        {
            _closeButton.onClick.RemoveAllListeners();
        }

        public void SetPotionsAmount(string amount) => _potionsText.text = amount;
    }
}