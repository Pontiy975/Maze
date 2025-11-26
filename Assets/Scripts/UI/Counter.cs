using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Maze.UI
{
    public enum CounterType
    {
        Distance,
        Time
    }

    public class Counter : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private Image icon;

        public void SetValue(int value)
        {
            text.text = value.ToString();
        }
    }
}
