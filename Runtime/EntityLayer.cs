using System.Collections.Generic;
using UnityEngine;

namespace VED.Tilemaps
{
    public class EntityLayer : MonoBehaviour
    {
        public string ID => _id;
        protected string _id = string.Empty;

        public Dictionary<string, Entity> Entities => _entities;
        protected Dictionary<string, Entity> _entities = null;

        [SerializeField] private bool _visible = false;

        public EntityLayer Init(LayerInstance definition, string levelID)
        {
            _id = definition.Iid;

            // set up entities
            _entities = new Dictionary<string, Entity>();
            for (int i = 0; i < definition.EntityInstances.Count; i++)
            {
                Entity entityPrefab = EntityManager.Instance.EntityMapper[definition.EntityInstances[i].Identifier];
                if (entityPrefab == null) continue;

                int xPosition = (int)definition.EntityInstances[i].Px[0] / Consts.TILE_SIZE;
                int yPosition = (int)definition.EntityInstances[i].Px[1] / Consts.TILE_SIZE;
                float xPivot = (float)definition.EntityInstances[i].Pivot[0];
                float yPivot = (float)definition.EntityInstances[i].Pivot[1];

                Vector2 position = (Vector2)transform.position + new Vector2(xPosition, -yPosition) + new Vector2(xPivot, -yPivot);

                Entity entityInstance = Instantiate(entityPrefab, position, Quaternion.identity, transform).Init(definition.EntityInstances[i], levelID);
                entityInstance.name = "Entity [" + xPosition + ", " + yPosition + "]: " + definition.EntityInstances[i].Identifier;

                _entities.Add(definition.EntityInstances[i].Iid, entityInstance);
            }

            return this;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_entities == null) return;
            foreach (Entity entity in _entities.Values)
            {
                entity.Visible = _visible;
            }
        }
#endif
    }
}