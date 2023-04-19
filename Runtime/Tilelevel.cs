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
            _size = new Vector2(definition.PxWid / Consts.TILE_SIZE, definition.PxHei / Consts.TILE_SIZE);

            InitTileLayers(definition);
            InitEntityLayers(definition);

            return this;
        }


        protected virtual void InitTileLayers(Level definition)
        {
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

            _tileLayers = new Dictionary<string, TileLayer>();

            for (int i = 0; i < layerDefinitions.Count; i++)
            {
                GameObject gameObject = new GameObject("TileLayer: " + layerDefinitions[i].Identifier);
                gameObject.transform.SetParent(transform);
                gameObject.transform.localPosition = Vector3.zero;

                _tileLayers.Add(layerDefinitions[i].Iid, gameObject.AddComponent<TileLayer>().Init(layerDefinitions[i], layerDefinitions.Count - i));
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

                _entityLayers.Add(layerDefinitions[i].Iid, gameObject.AddComponent<EntityLayer>().Init(layerDefinitions[i]));
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

                if (tilelevelManager.Tilelevels[neighbourLevel.LevelIid] is TileLevel physicsTilelevel)
                    _neighbourTileLevels[neighbourLevel.Dir[0]].Add(physicsTilelevel);
            }
        }
    }
}