using UnityEngine;
using UnityEngine.UI;
using System;

public class Tooltip : MonoBehaviour
{
    public Text ItemName;
    public Text TypeOfItem;

    public Image ItemIcon;

    [Serializable]
    public struct EquipmentTooltip
    {
        public Text Strength;
        public Text Dexterity;
        public Text Agility;
        public Text Inteligence;
    }

    [Serializable]
    public struct ConsumableTooltip
    {
        public Text Description;
    }

    public EquipmentTooltip EquipTooltip;
    public ConsumableTooltip ConsumTooltip;
}
