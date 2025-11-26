using System;
using System.Collections;

namespace UISystem.Transitions
{
    public interface ITransition
    {
        public bool InTransition { get; }
        public IEnumerable Show(Action onEnd);
        public IEnumerable Hide(Action onEnd);
    }
}