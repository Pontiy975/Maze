using UISystem.Transitions;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem.Dialogs
{
    public abstract class SimpleDialogWithTransition : BaseDialog
    {
        [SerializeField] protected RectTransform dialogRectTransform;
        [SerializeField] private Image background;

        protected override void Start()
        {
            Transition = new SimpleDialogTransition(dialogRectTransform, background, animationDuration);

            dialogRectTransform.gameObject.SetActive(true);
            background?.gameObject.SetActive(true);

            if (background && background.sprite)
            {
                Color color = background.color;
                color.a = 0f;
                background.color = color;
                background.gameObject.SetActive(false);
            }

            dialogRectTransform.localScale = Vector3.zero;
            dialogRectTransform.gameObject.SetActive(false);
            
            base.Start();
        }
    }
}