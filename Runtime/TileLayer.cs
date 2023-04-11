using UnityEngine;

namespace VED.Tilemaps
{
    public partial class Tilelayer : MonoBehaviour
    {
        public Tile[,] Tiles => _tiles;
        protected Tile[,] _tiles = null;

        public Tilelayer Init(LayerInstance definition, int sortingOrder)
        {
            if (definition.Type == Consts.TILELAYER_TYPE) return InitTilelayer(definition, sortingOrder);
            if (definition.Type == Consts.AUTOLAYER_TYPE) return InitAutolayer(definition, sortingOrder);
            if (definition.Type == Consts.INTLAYER_TYPE ) return InitIntlayer (definition, sortingOrder);
            return this;
        }

        protected Tilelayer InitTilelayer(LayerInstance definition, int sortingOrder)
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

        protected Tilelayer InitAutolayer(LayerInstance definition, int sortingOrder)
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

        protected Tilelayer InitIntlayer(LayerInstance definition, int sortingOrder)
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