using System;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem.Dialogs
{
    [CreateAssetMenu(fileName = "DialogsManager", menuName = "ScriptableObjects/UI/DialogsManager")]
    public class DialogsManager : ScriptableObject
    {
        public event Action<BaseDialog> OnOpenDialog;
        public event Action<BaseDialog> OnCloseDialog;

        private readonly Dictionary<Type, BaseDialog> _dialogs = new();
        private readonly List<BaseDialog> _dialogsStack = new();

        public T GetDialog<T>() where T : BaseDialog
        {
            if (_dialogs.ContainsKey(typeof(T)))
                return _dialogs[typeof(T)] as T;
            
            return null;
        }

        public void RegisterDialog(BaseDialog dialog)
        {
            if (_dialogs.ContainsKey(dialog.GetType())) 
                return;

            _dialogs.Add(dialog.GetType(), dialog);
            dialog.SetActive(false);
        }

        public T OpenDialog<T>() where T : BaseDialog
        {
            if (!_dialogs.ContainsKey(typeof(T)))
                return null;

            BaseDialog dialog = _dialogs[typeof(T)];
            
            if (dialog.Stackable)
            {
                if(_dialogsStack.Count == 0)
                    PauseGame(true);
                
                _dialogsStack.Add(dialog);
                dialog.transform.SetAsLastSibling();
            }

            dialog.OnClose += OnClose;
            dialog.ShowDialog();
            OnOpenDialog?.Invoke(dialog);
            
            return (T) dialog;
        }

        public void CloseDialog<T>(Action onClose = null) where T : BaseDialog
        {
            if (_dialogs.ContainsKey(typeof(T)))
                CloseDialog(_dialogs[typeof(T)], onClose);
        }

        public void CloseDialog(BaseDialog dialog, Action onClose = null)
        {
            if (!_dialogs.ContainsKey(dialog.GetType()))
                return;
            
            dialog.Hide(onClose);
        }

        private void OnClose(BaseDialog dialog)
        {
            dialog.OnClose -= OnClose;
            OnCloseDialog?.Invoke(dialog);

            if (!dialog.Stackable || !_dialogsStack.Contains(dialog)) 
                return;
            
            _dialogsStack.Remove(dialog);
                
            if(_dialogsStack.Count == 0)
                PauseGame(false);
        }

        public void ClearDialogs()
        {
            _dialogsStack.Clear();
            _dialogs.Clear();
        }

        private void PauseGame(bool value)
        {
            float newTimeScale = value ? 0 : 1;
            if (Math.Abs(Time.timeScale - newTimeScale) > .001f)
                Time.timeScale = newTimeScale;
        }
    }
}