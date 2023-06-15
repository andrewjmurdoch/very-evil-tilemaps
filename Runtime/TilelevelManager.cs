using System;
using System.Collections.Generic;
using UnityEngine;
using VED.Utilities;

namespace VED.Tilemaps
{
    public class TileLevelManager : Singleton<TileLevelManager>
    {
        public Dictionary<string, TileLevel> Tilelevels => _tileLevels;
        private Dictionary<string, TileLevel> _tileLevels = new Dictionary<string, TileLevel>();

        private const int ASYNC_BATCH_SIZE_TILE = 100;
        private const int ASYNC_BATCH_SIZE_ENTITY = 10;

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

        public void InitAsync(List<Level> definitions, Action callback, int tileBatchSize = ASYNC_BATCH_SIZE_TILE, int entityBatchSize = ASYNC_BATCH_SIZE_ENTITY)
        {
            _tileLevels = new Dictionary<string, TileLevel>();

            int count = definitions.Count;
            if (count <= 0)
            {
                callback?.Invoke();
                return;
            }

            for (int i = 0; i < definitions.Count; i++)
            {
                int index = i;

                GameObject gameObject = new GameObject("TileLevel: " + definitions[i].Identifier);
                gameObject.transform.localPosition = new Vector2(definitions[i].WorldX / Consts.TILE_SIZE, -definitions[i].WorldY / Consts.TILE_SIZE);

                gameObject.AddComponent<TileLevel>().InitAsync(definitions[i], tileBatchSize, entityBatchSize, (TileLevel tileLevel) =>
                {
                    _tileLevels.Add(definitions[index].Iid, tileLevel);
                    Join();
                });
            }

            void Join()
            {
                if (count <= 0) return;
                count--;
                if (count > 0) return;

                for (int i = 0; i < definitions.Count; i++)
                {
                    _tileLevels[definitions[i].Iid].InitNeighbours(definitions[i]);
                }

                callback?.Invoke();
            }
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