using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.StatsUI.Element
{
    public class StatSlot : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TMP_Text _statName;
        [SerializeField] private TMP_Text _statValue;

        public event Action<StatSlot> SlotClicked;

        public void OnPointerClick(PointerEventData eventData) => SlotClicked?.Invoke(this);

        public void SetName(string name)
        {
            if (_statValue == null)
                return;

            _statName.text = name;
        }

        public void SetValue(float value)
        {
            if (_statValue == null)
                return;

            _statValue.text = value.ToString();
        }
    }
}