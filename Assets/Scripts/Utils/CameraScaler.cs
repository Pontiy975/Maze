using UnityEngine;

namespace Utils
{
    public static class CameraScaler
    {
        private const float BaseMazeSize = 10f;
        private const float BaseCameraSize = 5f;

        public static void ScaleCamera(Vector2Int mazeSize)
        {
            int maxMazeDimension = Mathf.Max(mazeSize.x, mazeSize.y);
            Camera.main.orthographicSize = BaseCameraSize * (maxMazeDimension / BaseMazeSize);
        }
    }
}
