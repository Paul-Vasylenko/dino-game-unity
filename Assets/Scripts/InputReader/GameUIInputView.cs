﻿using UnityEngine;
using UnityEngine.UI;

namespace InputReader
{
    public class GameUIInputView : MonoBehaviour, IEntityInputSource
    {
        [SerializeField] private Joystick _joystick;
        [SerializeField] private Button _jumpButton;
        [SerializeField] private Button _kickButton;
        [SerializeField] private Button _biteButton;

        private void Awake()
        {
            _jumpButton.onClick.AddListener(() => Jump = true);
            _kickButton.onClick.AddListener(() => Kick = true);
            _biteButton.onClick.AddListener(() => Bite = true);
        }

        private void OnDestroy()
        {
            _jumpButton.onClick.RemoveAllListeners();
            _kickButton.onClick.RemoveAllListeners();
            _biteButton.onClick.RemoveAllListeners();
        }

        public float HorizontalDirection => _joystick.Horizontal;
        public bool Jump { get; private set; }
        public bool Kick { get; private set; }
        public bool Bite { get; private set; }

        public void ResetOneTimeActions()
        {
            Jump = false;
            Kick = false;
            Bite = false;
        }
    }
}