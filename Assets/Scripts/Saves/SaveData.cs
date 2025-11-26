using UnityEngine;
using Utils;

namespace Saves
{
    [CreateAssetMenu(fileName = "SaveData", menuName = "ScriptableObjects/SaveData")]
    public class SaveData : BaseModel
    {
        [SerializeField] private SaveableModel[] models;

        // It's only for representing saves for now
        [field: SerializeField] private SerializableDictionaryBase<string, string> savedData;

        public override void Init()
        {
            foreach (SaveableModel baseModel in models)
                baseModel.Init();
            base.Init();
        }

        public override void Reset()
        {
            ResetModels();
            base.Reset();
        }

        public void SaveAll()
        {
            if (!Initialized)
                return;

            foreach (SaveableModel baseModel in models)
                baseModel.Save();
        }

        private void ResetModels()
        {
            foreach (SaveableModel baseModel in models)
            {
                baseModel.Save();
                baseModel.Reset();
            }
        }

        public T Load<T>(T defaultData) where T : class
        {
            string key = typeof(T).Name;
            return Load(defaultData, key);
        }
        
        public T Load<T>(T defaultData, string saveKey) where T : class
        {
            T load = PrefsUtils.Load(defaultData, out string data, saveKey);
            savedData[saveKey] = data;
            return load;
        }

        public void Save<T>(T instance) where T: class
        {
            if (!Initialized)
                return;

            string key = typeof(T).Name;
            Save(instance, key);
        }
        
        public void Save<T>(T instance, string saveKey) where T: class
        {
            if (!Initialized)
                return;

            PrefsUtils.Save(instance, out string data, saveKey);
            savedData[saveKey] = data;
        }
    }
}