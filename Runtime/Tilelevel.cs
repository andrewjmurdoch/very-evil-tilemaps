using System;
using System.Collections.Generic;
using UnityEngine;

namespace VED.Tilemaps
{
    public partial class TileLevel : MonoBehaviour
    {
        public string ID => _id;
        protected string _id = string.Empty;

        public Dictionary<char, List<TileLevel>> NeighbourTileLevels => _neighbourTileLevels;
        protected Dictionary<char, List<TileLevel>> _neighbourTileLevels = new Dictionary<char, List<TileLevel>>()
        {
            { 'n', new List<TileLevel>() },
            { 'e', new List<TileLevel>() },
            { 's', new List<TileLevel>() },
            { 'w', new List<TileLevel>() }
        };

        public Dictionary<string, TileLayer> TileLayers => _tileLayers;
        protected Dictionary<string, TileLayer> _tileLayers = new Dictionary<string, TileLayer>();

        public Dictionary<string, EntityLayer> EntityLayers => _entityLayers;
        protected Dictionary<string, EntityLayer> _entityLayers = new Dictionary<string, EntityLayer>();

        public Vector2 Size => _size;
        protected Vector2 _size = Vector2.zero;

        public TileLevel Init(Level definition)
        {
            _id = definition.Iid;
            _size = new Vector2(definition.PxWid / TilesetManager.TileSize, definition.PxHei / TilesetManager.TileSize);

            InitTileLayers(definition);
            InitEntityLayers(definition);

            return this;
        }

        public void InitAsync(Level definition, int tileBatchSize, int entityBatchSize, Action<TileLevel> callback)
        {
            _id = definition.Iid;
            _size = new Vector2(definition.PxWid / TilesetManager.TileSize, definition.PxHei / TilesetManager.TileSize);

            InitTileLayersAsync(definition, tileBatchSize, () =>
            {
                InitEntityLayersAsync(definition, entityBatchSize, () =>
                {
                    callback?.Invoke(this);
                });
            });
        }

        protected virtual void InitTileLayers(Level definition)
        {
            _tileLayers = new Dictionary<string, TileLayer>();

            // find all layers which are not entity layers
            List<LayerInstance> layerDefinitions = new List<LayerInstance>();
            for (int i = 0; i < definition.LayerInstances.Count; i++)
            {
                if (definition.LayerInstances[i].Type != Consts.ENTITYLAYER_TYPE)
                {
                    layerDefinitions.Add(definition.LayerInstances[i]);
                    continue;
                }
            }

            for (int i = 0; i < layerDefinitions.Count; i++)
            {
                GameObject gameObject = new GameObject("TileLayer: " + layerDefinitions[i].Identifier);
                gameObject.transform.SetParent(transform);
                gameObject.transform.localPosition = Vector3.zero;

                _tileLayers.Add(layerDefinitions[i].Iid, gameObject.AddComponent<TileLayer>().Init(layerDefinitions[i], -i));
            }
        }

        protected virtual void InitTileLayersAsync(Level definition, int batchSize, Action callback)
        {
            _tileLayers = new Dictionary<string, TileLayer>();

            // find all layers which are not entity layers
            List<LayerInstance> layerDefinitions = new List<LayerInstance>();
            for (int i = 0; i < definition.LayerInstances.Count; i++)
            {
                if (definition.LayerInstances[i].Type != Consts.ENTITYLAYER_TYPE)
                {
                    layerDefinitions.Add(definition.LayerInstances[i]);
                }
            }

            int count = layerDefinitions.Count;
            if (count <= 0)
            {
                callback?.Invoke();
                return;
            }

            for (int i = 0; i < layerDefinitions.Count; i++)
            {
                int index = i;

                GameObject gameObject = new GameObject("TileLayer: " + layerDefinitions[i].Identifier);
                gameObject.transform.SetParent(transform);
                gameObject.transform.localPosition = Vector3.zero;

                gameObject.AddComponent<TileLayer>().InitAsync(layerDefinitions[i], -index, batchSize, (TileLayer tileLayer) =>
                {
                    _tileLayers.Add(layerDefinitions[index].Iid, tileLayer);
                    Join();
                });
            }

            void Join()
            {
                if (count <= 0) return;
                count--;
                if (count > 0) return;

                callback?.Invoke();
            }
        }

        protected virtual void InitEntityLayers(Level definition)
        {
            // find all layers which are entity layers
            List<LayerInstance> layerDefinitions = new List<LayerInstance>();
            for (int i = 0; i < definition.LayerInstances.Count; i++)
            {
                if (definition.LayerInstances[i].Type == Consts.ENTITYLAYER_TYPE)
                {
                    layerDefinitions.Add(definition.LayerInstances[i]);
                }
            }

            _entityLayers = new Dictionary<string, EntityLayer>();

            for (int i = 0; i < layerDefinitions.Count; i++)
            {
                GameObject gameObject = new GameObject("EntityLayer: " + layerDefinitions[i].Identifier);
                gameObject.transform.SetParent(transform);
                gameObject.transform.localPosition = Vector3.zero;

                _entityLayers.Add(layerDefinitions[i].Iid, gameObject.AddComponent<EntityLayer>().Init(layerDefinitions[i], definition.Iid));
            }
        }

        protected virtual void InitEntityLayersAsync(Level definition, int batchSize, Action callback)
        {
            _entityLayers = new Dictionary<string, EntityLayer>();

            // find all layers which are entity layers
            List<LayerInstance> layerDefinitions = new List<LayerInstance>();
            for (int i = 0; i < definition.LayerInstances.Count; i++)
            {
                if (definition.LayerInstances[i].Type == Consts.ENTITYLAYER_TYPE)
                {
                    layerDefinitions.Add(definition.LayerInstances[i]);
                }
            }

            int count = layerDefinitions.Count;
            if (count <= 0)
            {
                callback?.Invoke();
                return;
            }

            for (int i = 0; i < layerDefinitions.Count; i++)
            {
                int index = i;

                GameObject gameObject = new GameObject("EntityLayer: " + layerDefinitions[i].Identifier);
                gameObject.transform.SetParent(transform);
                gameObject.transform.localPosition = Vector3.zero;

                gameObject.AddComponent<EntityLayer>().InitAsync(layerDefinitions[i], definition.Iid, batchSize, (EntityLayer entityLayer) =>
                {
                    _entityLayers.Add(layerDefinitions[index].Iid, entityLayer);
                    Join();
                });
            }

            void Join()
            {
                if (count <= 0) return;
                count--;
                if (count > 0) return;

                callback?.Invoke();
            }
        }

        public virtual void InitNeighbours(Level definition)
        {
            // cache manager
            TileLevelManager tilelevelManager = TileLevelManager.Instance;

            // set up neighbours
            _neighbourTileLevels = new Dictionary<char, List<TileLevel>>()
            {
                { 'n', new List<TileLevel>() },
                { 'e', new List<TileLevel>() },
                { 's', new List<TileLevel>() },
                { 'w', new List<TileLevel>() }
            };
            foreach (NeighbourLevel neighbourLevel in definition.Neighbours)
            {
                if (!tilelevelManager.Tilelevels.ContainsKey(neighbourLevel.LevelIid)) continue;

                if (tilelevelManager.Tilelevels[neighbourLevel.LevelIid] is TileLevel tileLevel)
                    _neighbourTileLevels[neighbourLevel.Dir[0]].Add(tileLevel);
            }
        }
    }
}