using System;
using UnityEngine;

namespace Maze.Core.Data
{
    public enum PropertyType
    {
        Width,
        Height,
        Exits
    }

    [CreateAssetMenu(fileName = "MazeModel", menuName = "ScriptableObjects/MazeData/MazeModel")]
    public class MazeModel : ScriptableObject
    {
        [SerializeField] private PropertyData[] properties;

        public PropertyData this[PropertyType t]
        {
            get
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    if (properties[i].Type == t)
                        return properties[i];
                }

                return null;
            }
        }
    }

    [Serializable]
    public class PropertyData
    {
        [field: SerializeField] public PropertyType Type { get; private set; }
        [field: SerializeField] public int Min { get; private set; }
        [field: SerializeField] public int Max { get; private set; }
    }

    public class MazeConfig
    {
        public Vector2Int Size { get; private set; }
        public int Exits { get; private set; }

        public MazeConfig(Vector2Int size, int exits)
        {
            Size = size;
            Exits = exits;
        }
    }
}