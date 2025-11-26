using System;
using UnityEngine;

namespace Saves
{
    public abstract class BaseModel : ScriptableObject
    {
        public event Action OnInitialized;
        
        public bool Initialized { get; private set; }

        public virtual void Init()
        {
            Initialized = true;
            OnInitialized?.Invoke();
        }

        public virtual void Reset()
        {
            Initialized = false;
        }
    }
}