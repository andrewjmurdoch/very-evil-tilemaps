using System.Collections.Generic;
using UnityEngine;
using VED.Utilities;

namespace VED.Tilemaps
{
    public class TilelevelManager : Singleton<TilelevelManager>
    {
        public Dictionary<string, Tilelevel> Tilelevels => _tilelevels;
        private Dictionary<string, Tilelevel> _tilelevels = new Dictionary<string, Tilelevel>();

        public TilelevelManager Init(List<Level> definitions)
        {
            _tilelevels = new Dictionary<string, Tilelevel>();

            for (int i = 0; i < definitions.Count; i++)
            {
                GameObject gameObject = new GameObject("Tilelevel: " + definitions[i].Identifier);
                gameObject.transform.localPosition = new Vector2(definitions[i].WorldX / Consts.TILE_SIZE, -definitions[i].WorldY / Consts.TILE_SIZE);

                _tilelevels.Add(definitions[i].Iid, gameObject.AddComponent<Tilelevel>().Init(definitions[i]));
            }

            for (int i = 0; i < definitions.Count; i++)
            {
                _tilelevels[definitions[i].Iid].InitNeighbours(definitions[i]);
            }

            return this;
        }

        public Tilelevel GetTilelevel(Vector2 position)
        {
            foreach (Tilelevel tilelevel in _tilelevels.Values)
            {
                if (   (position.x >= tilelevel.transform.position.x)
                    && (position.x <  tilelevel.transform.position.x + tilelevel.Size.x)
                    && (position.y <= tilelevel.transform.position.y)
                    && (position.y >  tilelevel.transform.position.y - tilelevel.Size.y)
                   )
                {
                    return tilelevel;
                }
            }
            return null;
        }
    }
}