using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerStatsUIView : MonoBehaviour
    {
        [field: SerializeField] public Slider HpBar { get; private set; }
    }
}