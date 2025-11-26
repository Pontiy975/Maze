using Maze.Core.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Maze.UI.Dialogs
{
    public class SettingsProperty : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TMP_Text valueText;

        public int Value => Mathf.RoundToInt(slider.value);

        public void Init(PropertyData data)
        {
            slider.wholeNumbers = true;
            slider.minValue = data.Min;
            slider.maxValue = data.Max;
            slider.value = data.Min;

            valueText.text = Mathf.RoundToInt(slider.value).ToString();

            slider.onValueChanged.AddListener((float value) =>
            {
                int intValue = Mathf.RoundToInt(value);
                valueText.text = intValue.ToString();
            });
        }
    }
}
