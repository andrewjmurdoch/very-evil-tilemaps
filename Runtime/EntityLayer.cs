using System.Collections.Generic;
using UnityEngine;

namespace VED.Tilemaps
{
    public class EntityLayer : MonoBehaviour
    {
        public List<Entity> Entities => _entities;
        protected List<Entity> _entities = null;

        [SerializeField] private bool _visible = false;

        public EntityLayer Init(LayerInstance definition)
        {
            // set up entities
            _entities = new List<Entity>();
            for (int i = 0; i < definition.EntityInstances.Count; i++)
            {
                Entity entityPrefab = EntityManager.Instance.EntityMapper[definition.EntityInstances[i].Identifier];
                if (entityPrefab == null) continue;

                int x = (int)definition.EntityInstances[i].Px[0] / Consts.TILE_SIZE;
                int y = (int)definition.EntityInstances[i].Px[1] / Consts.TILE_SIZE;

                Entity entityInstance = Instantiate(entityPrefab).Init(definition.EntityInstances[i]);
                entityInstance.name = "Entity [" + x + ", " + y + "]: " + definition.EntityInstances[i].Identifier;
                entityInstance.transform.SetParent(transform);

                Vector2 offset = (Vector2.right + Vector2.down) * (1f / 2f);
                entityInstance.transform.localPosition = new Vector2(x, -y) + offset;

                _entities.Add(entityInstance);
            }

            return this;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_entities == null) return;
            foreach (Entity entity in _entities)
            {
                entity.Visible = _visible;
            }
        }
#endif
    }
}