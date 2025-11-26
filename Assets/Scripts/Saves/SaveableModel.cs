using UnityEngine;

namespace Saves
{
    public abstract class SaveableModel : BaseModel
    {
        [SerializeField] protected SaveData saveData;
        
        public override void Init()
        {
            Load();
            base.Init();
        }

        protected abstract void Load();
        public abstract void Save();
    }
}
