using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Items.Behaviour
{
    public class SceneItem : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _button;
        [SerializeField] private Canvas _canvas;

        [Header("Drop Animation")]
        [SerializeField] private float _dropAnimationDuration;
        [SerializeField] private float _dropRotation;
        [SerializeField] private float _dropRadius;

        private Sequence _sequence;

        [field: SerializeField] public float InteractionDistance { get; private set; }

        public Vector2 Position => _sprite.transform.position;
        public event Action<SceneItem> ItemClicked;

        private bool _isTextEnabled = true;
        public bool IsTextEnabled
        {
            set
            {
                if (_isTextEnabled != value)
                {
                    return;
                }

                _isTextEnabled = value;
                _canvas.enabled = false;
            }
        }

        public void SetItem(Sprite sprite, string itemName, Color textColor)
        {
            _sprite.sprite = sprite;
            _text.text = itemName;
            _text.color = textColor;
            _canvas.enabled = false;
        }

        public void PlayDrop(Vector2 position)
        {
            transform.position = position;
            var movePosition = transform.position + new Vector3(Random.Range(-_dropRadius, _dropRadius), -1, 0);
            _sequence = DOTween.Sequence();
            _sequence.Join(transform.DOMove(movePosition, _dropAnimationDuration));
            _sequence.Join(_sprite.transform.DORotate(new Vector3(0, 0, Random.Range(-_dropRotation, _dropRotation)), _dropAnimationDuration));
            _sequence.OnComplete(() => _canvas.enabled = _isTextEnabled);
        }

        private void Awake()
        {
            _button.onClick.AddListener(() => ItemClicked?.Invoke(this));
        }
        private void OnMouseDown() => ItemClicked?.Invoke(this);

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(_sprite.transform.position, InteractionDistance);
        }

        private void OnDestroy() => _button.onClick.RemoveAllListeners();
    }
}
