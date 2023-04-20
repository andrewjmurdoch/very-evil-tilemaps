using System.Collections.Generic;
using UnityEngine;
using VED.Utilities;

namespace VED.Tilemaps
{
    public class TileLevelManager : Singleton<TileLevelManager>
    {
        public Dictionary<string, TileLevel> Tilelevels => _tileLevels;
        private Dictionary<string, TileLevel> _tileLevels = new Dictionary<string, TileLevel>();

        public TileLevelManager Init(List<Level> definitions)
        {
            _tileLevels = new Dictionary<string, TileLevel>();

            for (int i = 0; i < definitions.Count; i++)
            {
                GameObject gameObject = new GameObject("TileLevel: " + definitions[i].Identifier);
                gameObject.transform.localPosition = new Vector2(definitions[i].WorldX / Consts.TILE_SIZE, -definitions[i].WorldY / Consts.TILE_SIZE);

                _tileLevels.Add(definitions[i].Iid, gameObject.AddComponent<TileLevel>().Init(definitions[i]));
            }

            for (int i = 0; i < definitions.Count; i++)
            {
                _tileLevels[definitions[i].Iid].InitNeighbours(definitions[i]);
            }

            return this;
        }

        public TileLevel GetTileLevel(Vector2 position)
        {
            foreach (TileLevel tileLevel in _tileLevels.Values)
            {
                if (   (position.x >= tileLevel.transform.position.x)
                    && (position.x <  tileLevel.transform.position.x + tileLevel.Size.x)
                    && (position.y <= tileLevel.transform.position.y)
                    && (position.y >  tileLevel.transform.position.y - tileLevel.Size.y)
                   )
                {
                    return tileLevel;
                }
            }
            return null;
        }
    }
}