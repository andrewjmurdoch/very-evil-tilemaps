using System.Collections.Generic;
using UnityEngine;

namespace VED.Tilemaps
{
    public partial class Tilelevel : MonoBehaviour
    {
        public Dictionary<char, List<Tilelevel>> NeighbourTilelevels => _neighbourTilelevels;
        protected Dictionary<char, List<Tilelevel>> _neighbourTilelevels = new Dictionary<char, List<Tilelevel>>()
        {
            { 'n', new List<Tilelevel>() },
            { 'e', new List<Tilelevel>() },
            { 's', new List<Tilelevel>() },
            { 'w', new List<Tilelevel>() }
        };

        public Dictionary<string, Tilelayer> Tilelayers => _tilelayers;
        protected Dictionary<string, Tilelayer> _tilelayers = new Dictionary<string, Tilelayer>();

        public Dictionary<string, EntityLayer> EntityLayers => _entityLayers;
        protected Dictionary<string, EntityLayer> _entityLayers = new Dictionary<string, EntityLayer>();

        public Vector2 Size => _size;
        protected Vector2 _size = Vector2.zero;

        public Tilelevel Init(Level definition)
        {
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

            _tilelayers = new Dictionary<string, Tilelayer>();

            for (int i = 0; i < layerDefinitions.Count; i++)
            {
                GameObject gameObject = new GameObject("Tilelayer: " + layerDefinitions[i].Identifier);
                gameObject.transform.SetParent(transform);
                gameObject.transform.localPosition = Vector3.zero;

                _tilelayers.Add(layerDefinitions[i].Iid, gameObject.AddComponent<Tilelayer>().Init(layerDefinitions[i], layerDefinitions.Count - i));
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
            TilelevelManager tilelevelManager = TilelevelManager.Instance;

            // set up neighbours
            _neighbourTilelevels = new Dictionary<char, List<Tilelevel>>()
            {
                { 'n', new List<Tilelevel>() },
                { 'e', new List<Tilelevel>() },
                { 's', new List<Tilelevel>() },
                { 'w', new List<Tilelevel>() }
            };
            foreach (NeighbourLevel neighbourLevel in definition.Neighbours)
            {
                if (!tilelevelManager.Tilelevels.ContainsKey(neighbourLevel.LevelIid)) continue;

                if (tilelevelManager.Tilelevels[neighbourLevel.LevelIid] is Tilelevel physicsTilelevel)
                    _neighbourTilelevels[neighbourLevel.Dir[0]].Add(physicsTilelevel);
            }
        }
    }
}