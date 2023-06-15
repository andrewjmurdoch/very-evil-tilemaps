using System;
using System.Collections;
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

                int xPosition = (int)definition.EntityInstances[i].Grid[0];
                int yPosition = (int)definition.EntityInstances[i].Grid[1];

                float xPivot = (float)definition.EntityInstances[i].Pivot[0];
                float yPivot = (float)definition.EntityInstances[i].Pivot[1];

                Vector2 position = (Vector2)transform.position + new Vector2(xPosition, -yPosition) + new Vector2(xPivot, -yPivot);

                Entity entityInstance = Instantiate(entityPrefab, position, Quaternion.identity, transform).Init(definition.EntityInstances[i], levelID);
                entityInstance.name = "Entity [" + xPosition + ", " + yPosition + "]: " + definition.EntityInstances[i].Identifier;

                _entities.Add(definition.EntityInstances[i].Iid, entityInstance);
            }

            return this;
        }

        public void InitAsync(LayerInstance definition, string levelID, int batchSize, Action<EntityLayer> callback)
        {
            _id = definition.Iid;

            // set up entities
            _entities = new Dictionary<string, Entity>();

            int count = definition.EntityInstances.Count;
            if (count <= 0)
            {
                callback?.Invoke(this);
                return;
            }
            int batches = (count / batchSize) + Math.Clamp(count % batchSize, 0, 1);

            void InstantiateEntityAsync(int index)
            {
                Entity entityPrefab = EntityManager.Instance.EntityMapper[definition.EntityInstances[index].Identifier];
                if (entityPrefab == null) return;

                int xPosition = (int)definition.EntityInstances[index].Grid[0];
                int yPosition = (int)definition.EntityInstances[index].Grid[1];

                float xPivot = (float)definition.EntityInstances[index].Pivot[0];
                float yPivot = (float)definition.EntityInstances[index].Pivot[1];

                Vector2 position = (Vector2)transform.position + new Vector2(xPosition, -yPosition) + new Vector2(xPivot, -yPivot);

                Entity entityInstance = Instantiate(entityPrefab, position, Quaternion.identity, transform).Init(definition.EntityInstances[index], levelID);
                entityInstance.name = "Entity [" + xPosition + ", " + yPosition + "]: " + definition.EntityInstances[index].Identifier;

                _entities.Add(definition.EntityInstances[index].Iid, entityInstance);
            }

            IEnumerator InstantiateEntityBatchesAsync()
            {
                for (int i = 0; i < batches; i++)
                {
                    for (int j = 0; j < batchSize && (i * batchSize) + j < count; j++)
                    {
                        InstantiateEntityAsync((i * batchSize) + j);
                    }
                    yield return null;
                }

                callback?.Invoke(this);
            }

            StartCoroutine(InstantiateEntityBatchesAsync());
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