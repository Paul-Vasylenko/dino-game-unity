using System;
using UnityEngine;
using UnityEngine.UI;

namespace InputReader
{
    public class GameUIInputView : MonoBehaviour, IEntityInputSource, IWindowsInputSource
    {
        [SerializeField] private Joystick _joystick;
        [SerializeField] private Button _jumpButton;

        public float HorizontalDirection => _joystick.Horizontal;
        public bool Jump { get; private set; }

        public event Action InventoryRequested;
        public event Action StatsWindowRequested;

        private void Awake()
        {
            _jumpButton.onClick.AddListener(() => Jump = true);
        }

        private void OnDestroy()
        {
            _jumpButton.onClick.RemoveAllListeners();
        }
       
        public void ResetOneTimeActions()
        {
            Jump = false;
        }
    }
}