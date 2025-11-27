using Maze.Core.Data;
using Saves;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.Game
{
    [CreateAssetMenu(fileName = "SessionSaver", menuName = "ScriptableObjects/Game/SessionSaver")]
    public class SessionSaver : SaveableModel
    {
        public SessionSaveData SessionSaveData { get; private set; }

        public void SaveSession((MazeConfig config, List<TileSaveData> tilesData) mazeSnapshot, int distance, int time, Vector2Int playerPosition)
        {
            SessionSaveData = new()
            {
                Width = mazeSnapshot.config.Size.x,
                Height = mazeSnapshot.config.Size.y,
                Exits = mazeSnapshot.config.Exits,
                Tiles = mazeSnapshot.tilesData,
                
                Distance = distance,
                Time = time,

                PlayerX = playerPosition.x,
                PlayerY = playerPosition.y,
            };

            saveData.Save(SessionSaveData);
        }

        public override void Save() { }

        protected override void Load()
        {
            SessionSaveData = saveData.Load<SessionSaveData>(null);
        }

        public void Clear()
        {
            if (SessionSaveData != null)
            {
                string key = SessionSaveData.GetType().Name;

                if (PlayerPrefs.HasKey(key))
                    PlayerPrefs.DeleteKey(key);

                SessionSaveData = null;
            }
        }
    }

    [Serializable]
    public class SessionSaveData
    {
        public int Width;
        public int Height;
        public int Exits;

        public int PlayerX;
        public int PlayerY;
        
        public int Distance;
        public int Time;
        
        public List<TileSaveData> Tiles = new();
    }

    [Serializable]
    public class TileSaveData
    {
        public int X;
        public int Y;
        public byte Walls;
        public bool IsExit;
        public List<(int x, int y)> Neighbors = new();
    }

    [Flags]
    public enum WallFlags : byte
    {
        Up = 1 << 0, // 0001
        Down = 1 << 1, // 0010
        Left = 1 << 2, // 0100
        Right = 1 << 3  // 1000
    }
}
