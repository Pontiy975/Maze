using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Maze.UI.Dialogs
{
    public class ResultRow : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text number;
        [SerializeField] private TMP_Text size;
        [SerializeField] private TMP_Text time;
        [SerializeField] private TMP_Text distance;

        public void SetData(int number, (int x, int y) size, int time, int distance, int bestDistance, Color color)
        {
            this.number.text = number.ToString();
            this.size.text = $"{size.x}x{size.y}";
            this.time.text = $"{time}s";
            this.distance.text = $"{distance}/{bestDistance}";
            image.color = color;
        }
    }
}
