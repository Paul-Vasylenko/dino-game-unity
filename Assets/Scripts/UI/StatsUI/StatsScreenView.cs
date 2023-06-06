using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UI.Core;
using UI.InventoryUI.Element;
using UI.StatsUI.Element;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.StatsUI
{
    public class StatsScreenView : ScreenView
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _inventoryButton;
        [SerializeField] private Transform _backPackContainer;

        public List<StatSlot> StatSlots { get; private set; }

        public event Action CloseClicked;
        public event Action InventoryRequested;

        private void Awake()
        {
            _closeButton.onClick.AddListener(() => CloseClicked?.Invoke());
            _inventoryButton.onClick.AddListener(() => InventoryRequested?.Invoke());
            StatSlots = GetComponentsInChildren<StatSlot>().ToList();
        }

        private void OnDestroy()
        {
            _closeButton.onClick.RemoveAllListeners();
        }
    }
}