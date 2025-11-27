using System;
using UnityEngine;

namespace Maze.Game
{
    [CreateAssetMenu(fileName = "GameStateMachine", menuName = "ScriptableObjects/Game/GameStateMachine")]
    public class GameStateMachine : ScriptableObject
    {
        public event Action<GameState> OnStateChanged;

        [SerializeField] private bool debug;

        public GameState State { get; private set; } = GameState.Menu;

        public GameState PreviousState { get; private set; }

        public void SetState(GameState gameState)
        {
            PreviousState = State;
            State = gameState;
            OnStateChanged?.Invoke(gameState);
            Log($"State changed: {PreviousState} => {State}");
        }

        public bool CheckStates(params GameState[] states)
        {
            for (int i = 0; i < states.Length; i++)
            {
                if (State == states[i])
                    return true;
            }

            return false;
        }

        private void Log(string message)
        {
            if (debug)
                Debug.Log(message.LogFormat(this, Color.yellow));
        }
    }

    public enum GameState
    {
        Menu,
        InGame
    }
}
