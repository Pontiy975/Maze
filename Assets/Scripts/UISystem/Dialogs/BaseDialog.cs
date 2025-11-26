using UISystem.Transitions;
using System;
using UnityEngine;
using UISystem.Dialogs;

namespace UISystem.Dialogs
{
    public abstract class BaseDialog : SafeArea.SafeArea
    {
        [field: SerializeField] public bool Stackable { get; private set; } = true;
        [SerializeField] protected float animationDuration = .3f;
        [SerializeField] protected DialogsManager dialogsManager;

        public event Action<BaseDialog> OnClose;
        
        protected abstract ITransition Transition { get; set; }

        protected virtual void Start()
        {
            dialogsManager.RegisterDialog(this);

            Init();
        }

        protected abstract void Init();

        public virtual void ShowDialog(Action onEnd = null)
        {
            SetActive(true);
            if (!Transition.InTransition)
                StartCoroutine(Transition.Show(onEnd).GetEnumerator());
        }

        //TODO: Merge somehow with HideDialog to use only one Hide function
        public virtual void Hide(Action end = null)
        {
            if (Transition.InTransition) return;
            if (!Active) return;

            void OnEnd()
            {
                SetActive(false);
                end?.Invoke();
                OnClose?.Invoke(this);
            }
            
            StartCoroutine(Transition.Hide(OnEnd).GetEnumerator());
        }

        protected virtual void HideDialog()
        {
            Hide();
        }
    }
}