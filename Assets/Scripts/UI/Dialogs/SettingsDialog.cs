using Maze.Core.Data;
using Maze.Game;
using UISystem.Dialogs;
using UISystem.Transitions;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Zenject;

namespace Maze.UI.Dialogs
{
    public class SettingsDialog : SimpleDialogWithTransition
    {
        [SerializeField] private Button playButton;
        [SerializeField] private SerializableDictionaryBase<PropertyType, SettingsProperty> properties;

        [Inject] private GameManager _gameManager;
        
        protected override ITransition Transition { get; set; }

        protected override void Init()
        {
            foreach (var property in properties)
            {
                property.Value.Init(_gameManager.MazeModel[property.Key]);
            }

            playButton.onClick.AddListener(() =>
            {
                Vector2Int size = new(properties[PropertyType.Width].Value, properties[PropertyType.Height].Value);
                MazeConfig config = MazeConfigFactory.Create(size, properties[PropertyType.Exits].Value);
                _gameManager.StartGame(config);

                Hide();
            });
        }

        private void OnDestroy()
        {
            playButton.onClick.RemoveAllListeners();
        }
    }
}
