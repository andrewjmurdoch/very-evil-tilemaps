using System;
using System.Collections.Generic;
using UnityEngine;

namespace VED.Tilemaps
{
    [CreateAssetMenu(fileName = "EntityMapper", menuName = "VED/Tilemaps/EntityMapper")]

    public class EntityMapper : ScriptableObject
    {
        [Serializable]
        public class EntityData
        {
            [SerializeField] public string ID = string.Empty;
            [SerializeField] public Entity Entity = null;
        }

        [SerializeField] private List<EntityData> _entities = new List<EntityData>();
        private Dictionary<string, Entity> _entityDictionary = null;

        public Entity this[string name]
        {
            get
            {
                if (_entityDictionary == null) InitEntityDictionary();
                if (_entityDictionary.TryGetValue(name, out Entity entity)) return entity;
                return null;
            }
        }

        public void InitEntityDictionary()
        {
            _entityDictionary = new Dictionary<string, Entity>();
            foreach (EntityData entityData in _entities)
            {
                if (entityData == null) continue;
                if (_entityDictionary.ContainsKey(entityData.ID)) continue;

                _entityDictionary.Add(entityData.ID, entityData.Entity);
            }
        }
    }
}