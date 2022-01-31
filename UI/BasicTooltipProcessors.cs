using TMPro;
using UnityEngine;

namespace Ludole.Inventory
{
    public class BasicTooltipProcessors : MonoBehaviour
    {
        [SerializeField] private GameObject _basicTextPrefab;

        public void ColorizedItemName(ItemBase item, GameObject tooltipRoot)
        {
            GameObject obj = Instantiate(_basicTextPrefab, tooltipRoot.transform);
            TextMeshProUGUI text = obj.GetComponent<TextMeshProUGUI>();
            text.text = item.Name;
            text.fontWeight = FontWeight.Bold;
            text.fontSize = 48;
            text.color = item.Rarity.Color;
            text.fontStyle = FontStyles.Bold;
        }
    }
}