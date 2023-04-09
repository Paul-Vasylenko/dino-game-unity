using UnityEngine;
using UnityEngine.UI;

namespace InputReader
{
    public class GameUIInputView : MonoBehaviour, IEntityInputSource
    {
        [SerializeField] private Joystick _joystick;
        [SerializeField] private Button _jumpButton;

        private void Awake()
        {
            _jumpButton.onClick.AddListener(() => Jump = true);
        }

        private void OnDestroy()
        {
            _jumpButton.onClick.RemoveAllListeners();
        }

        public float HorizontalDirection => _joystick.Horizontal;
        public bool Jump { get; private set; }

        public void ResetOneTimeActions()
        {
            Jump = false;
        }
    }
}