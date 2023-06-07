using System;
using UnityEngine;
using UnityEngine.UI;

namespace InputReader
{
    public class GameUIInputView : MonoBehaviour, IEntityInputSource, IWindowsInputSource
    {
        [SerializeField] private Joystick _joystick;
        [SerializeField] private Button _jumpButton;
        [SerializeField] private Button _kickButton;
        [SerializeField] private Button _biteButton;
        [SerializeField] private Button _statsButton;
        [SerializeField] private Button _inventoryButton;
        public float HorizontalDirection => _joystick.Horizontal;
        public bool Jump { get; private set; }
        public bool Kick { get; private set; }
        public bool Bite { get; private set; }

        public event Action InventoryRequested;
        public event Action StatsRequested;

        private void Awake()
        {
            _jumpButton.onClick.AddListener(() => Jump = true);
            _kickButton.onClick.AddListener(() => Kick = true);
            _biteButton.onClick.AddListener(() => Bite = true);
            _statsButton.onClick.AddListener(() => StatsRequested?.Invoke());
            _inventoryButton.onClick.AddListener(() => InventoryRequested?.Invoke());
        }

        private void OnDestroy()
        {
            _jumpButton.onClick.RemoveAllListeners();
            _kickButton.onClick.RemoveAllListeners();
            _biteButton.onClick.RemoveAllListeners();
            _statsButton.onClick.RemoveAllListeners();
            _inventoryButton.onClick.RemoveAllListeners();
        }

        public void ResetOneTimeActions()
        {
            Jump = false;
            Kick = false;
            Bite = false;
        }
    }
}