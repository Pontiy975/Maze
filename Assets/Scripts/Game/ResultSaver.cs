using Saves;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.Game
{
    [CreateAssetMenu(fileName = "ResultSaver", menuName = "ScriptableObjects/Game/ResultSaver")]
    public class ResultSaver : SaveableModel
    {
        public List<ResultEntry> Results { get; private set; }

        public void AddResult(ResultEntry entry)
        {
            Results ??= new();
            Results.Add(entry);

            Save();
        }

        public override void Save()
        {
            saveData.Save(new ResultSaveData { Results = Results });
        }

        protected override void Load()
        {
            Results = saveData.Load(new ResultSaveData()).Results;
        }
    }

    [Serializable]
    public class ResultSaveData
    {
        public List<ResultEntry> Results = new();
    }

    [Serializable]
    public class ResultEntry
    {
        public int Width;
        public int Height;
        public int Exits;

        public int Time;
        public int Distance;
        public int BestDistance;

        public string CompleteTime;
    }
}
