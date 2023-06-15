using System;
using System.Collections;
using UnityEngine;

namespace VED.Tilemaps
{
    public partial class TileLayer : MonoBehaviour
    {
        public string ID => _id;
        protected string _id = string.Empty;

        public Tile[,] Tiles => _tiles;
        protected Tile[,] _tiles = null;

        public TileLayer Init(LayerInstance definition, int sortingOrder)
        {
            _id = definition.Iid;

            if (definition.Type == Consts.TILELAYER_TYPE) return InitTilelayer(definition, sortingOrder);
            if (definition.Type == Consts.AUTOLAYER_TYPE) return InitAutolayer(definition, sortingOrder);
            if (definition.Type == Consts.INTLAYER_TYPE ) return InitIntlayer (definition, sortingOrder);
            return this;
        }

        public void InitAsync(LayerInstance definition, int sortingOrder, int batchSize, Action<TileLayer> callback)
        {
            _id = definition.Iid;

                 if (definition.Type == Consts.TILELAYER_TYPE) InitTilelayerAsync(definition, sortingOrder, batchSize, callback);
            else if (definition.Type == Consts.AUTOLAYER_TYPE) InitAutolayerAsync(definition, sortingOrder, batchSize, callback);
            else if (definition.Type == Consts.INTLAYER_TYPE ) InitIntlayerAsync (definition, sortingOrder, batchSize, callback);
        }

        #region Synchronous
        protected TileLayer InitTilelayer(LayerInstance definition, int sortingOrder)
        {
            if (!TilesetManager.Instance.Tilesets.TryGetValue((int)definition.TilesetDefUid, out Tileset tileset))
            {
                return this;
            }

            // set up tiles
            _tiles = new Tile[definition.CWid, definition.CHei];
            for (int i = 0; i < definition.GridTiles.Count; i++)
            {
                Tileset.Tile tilesetTile = tileset.Tiles[definition.GridTiles[i].T];

                int x = (int)definition.GridTiles[i].Px[0] / Consts.TILE_SIZE;
                int y = (int)definition.GridTiles[i].Px[1] / Consts.TILE_SIZE;

                InitTile(tilesetTile, x, y, sortingOrder);
            }

            return this;
        }

        protected TileLayer InitAutolayer(LayerInstance definition, int sortingOrder)
        {
            if (!TilesetManager.Instance.Tilesets.TryGetValue((int)definition.TilesetDefUid, out Tileset tileset))
            {
                return this;
            }

            // set up tiles
            _tiles = new Tile[definition.CWid, definition.CHei];
            for (int i = 0; i < definition.AutoLayerTiles.Count; i++)
            {
                Tileset.Tile tilesetTile = tileset.Tiles[definition.AutoLayerTiles[i].T];

                int x = (int)definition.AutoLayerTiles[i].Px[0] / Consts.TILE_SIZE;
                int y = (int)definition.AutoLayerTiles[i].Px[1] / Consts.TILE_SIZE;

                InitTile(tilesetTile, x, y, sortingOrder);
            }

            return this;
        }

        protected TileLayer InitIntlayer(LayerInstance definition, int sortingOrder)
        {
            if (!TilesetManager.Instance.Tilesets.TryGetValue((int)definition.TilesetDefUid, out Tileset tileset))
            {
                return this;
            }

            // set up tiles
            _tiles = new Tile[definition.CWid, definition.CHei];
            for (int i = 0; i < definition.AutoLayerTiles.Count; i++)
            {
                Tileset.Tile tilesetTile = tileset.Tiles[definition.AutoLayerTiles[i].T];

                int x = (int)definition.AutoLayerTiles[i].Px[0] / Consts.TILE_SIZE;
                int y = (int)definition.AutoLayerTiles[i].Px[1] / Consts.TILE_SIZE;

                InitTile(tilesetTile, x, y, sortingOrder);
            }

            return this;
        }
        #endregion

        #region Asynchronous
        protected void InitTilelayerAsync(LayerInstance definition, int sortingOrder, int batchSize, Action<TileLayer> callback)
        {
            if (!TilesetManager.Instance.Tilesets.TryGetValue((int)definition.TilesetDefUid, out Tileset tileset))
            {
                callback?.Invoke(this);
                return;
            }

            _tiles = new Tile[definition.CWid, definition.CHei];

            int count = definition.GridTiles.Count;
            if (count <= 0)
            {
                callback?.Invoke(this);
                return;
            }
            int batches = (count / batchSize) + Math.Clamp(count % batchSize, 0, 1);

            void InstantiateTileAsync(int index)
            {
                Tileset.Tile tilesetTile = tileset.Tiles[definition.GridTiles[index].T];

                int x = (int)definition.GridTiles[index].Px[0] / Consts.TILE_SIZE;
                int y = (int)definition.GridTiles[index].Px[1] / Consts.TILE_SIZE;

                InitTile(tilesetTile, x, y, sortingOrder);
            }

            IEnumerator InstatiateTileBatchesAsync()
            {
                for (int i = 0; i < batches; i++)
                {
                    for (int j = 0; j < batchSize && (i * batchSize) + j < count; j++)
                    {
                        InstantiateTileAsync((i * batchSize) + j);
                    }
                    yield return null;
                }

                callback?.Invoke(this);
            }

            StartCoroutine(InstatiateTileBatchesAsync());
        }

        protected void InitAutolayerAsync(LayerInstance definition, int sortingOrder, int batchSize, Action<TileLayer> callback)
        {
            if (!TilesetManager.Instance.Tilesets.TryGetValue((int)definition.TilesetDefUid, out Tileset tileset))
            {
                callback?.Invoke(this);
                return;
            }

            _tiles = new Tile[definition.CWid, definition.CHei];

            int count = definition.AutoLayerTiles.Count;
            if (count <= 0)
            {
                callback?.Invoke(this);
                return;
            }
            int batches = (count / batchSize) + Math.Clamp(count % batchSize, 0, 1);

            void InstantiateTileAsync(int index)
            {
                Tileset.Tile tilesetTile = tileset.Tiles[definition.AutoLayerTiles[index].T];

                int x = (int)definition.AutoLayerTiles[index].Px[0] / Consts.TILE_SIZE;
                int y = (int)definition.AutoLayerTiles[index].Px[1] / Consts.TILE_SIZE;

                InitTile(tilesetTile, x, y, sortingOrder);
            }

            IEnumerator InstatiateTileBatchesAsync()
            {
                for (int i = 0; i < batches; i++)
                {
                    for (int j = 0; j < batchSize && (i * batchSize) + j < count; j++)
                    {
                        InstantiateTileAsync((i * batchSize) + j);
                    }
                    yield return null;
                }

                callback?.Invoke(this);
            }

            StartCoroutine(InstatiateTileBatchesAsync());
        }

        protected void InitIntlayerAsync(LayerInstance definition, int sortingOrder, int batchSize, Action<TileLayer> callback)
        {
            if (!TilesetManager.Instance.Tilesets.TryGetValue((int)definition.TilesetDefUid, out Tileset tileset))
            {
                callback?.Invoke(this);
                return;
            }

            _tiles = new Tile[definition.CWid, definition.CHei];

            int count = definition.AutoLayerTiles.Count;
            if (count <= 0)
            {
                callback?.Invoke(this);
                return;
            }
            int batches = (count / batchSize) + Math.Clamp(count % batchSize, 0, 1);

            void InstantiateTileAsync(int index)
            {
                Tileset.Tile tilesetTile = tileset.Tiles[definition.AutoLayerTiles[index].T];

                int x = (int)definition.AutoLayerTiles[index].Px[0] / Consts.TILE_SIZE;
                int y = (int)definition.AutoLayerTiles[index].Px[1] / Consts.TILE_SIZE;

                InitTile(tilesetTile, x, y, sortingOrder);
            }

            IEnumerator InstatiateTileBatchesAsync()
            {
                for (int i = 0; i < batches; i++)
                {
                    for (int j = 0; j < batchSize && (i * batchSize) + j < count; j++)
                    {
                        InstantiateTileAsync((i * batchSize) + j);
                    }
                    yield return null;
                }

                callback?.Invoke(this);
            }

            StartCoroutine(InstatiateTileBatchesAsync());
        }
        #endregion

        protected Tile InitTile(Tileset.Tile tilesetTile, int x, int y, int sortingOrder)
        {
            GameObject gameObject = new GameObject("Tile [" + x + ", " + y + "]");
            gameObject.transform.SetParent(transform);
            Vector2 offset = (Vector2.right + Vector2.down) * (1f / 2f);
            gameObject.transform.localPosition = new Vector2(x, -y) + offset;

            Tile tile = gameObject.AddComponent<Tile>().Init(tilesetTile, sortingOrder);
            _tiles[x, y] = tile;

            return tile;
        }
    }
}