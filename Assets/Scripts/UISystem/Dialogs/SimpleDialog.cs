using UnityEngine;
using UnityEngine.UI;

namespace UISystem.Dialogs
{
    public abstract class SimpleDialog : SimpleDialogWithTransition
    {
        [SerializeField] private Button backgroundButton;
        [SerializeField] private Button closeButton;

        protected override void Start()
        {
            closeButton.onClick.AddListener(HideDialog);
            backgroundButton.onClick.AddListener(HideDialog);
            
            base.Start();
        }
        
        protected virtual void OnDestroy()
        {
            closeButton.onClick.RemoveAllListeners();
            backgroundButton.onClick.RemoveAllListeners();
        }
    }
}