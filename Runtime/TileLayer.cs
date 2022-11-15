using UnityEngine;

namespace VED.Tilemaps
{
    public partial class Tilelayer : MonoBehaviour
    {
        public Tile[,] Tiles => _tiles;
        protected Tile[,] _tiles = null;

        public Tilelayer Init(LayerInstance definition, int sortingOrder)
        {
            Tileset tileset = TilesetManager.Instance.Tilesets[(int)definition.TilesetDefUid];

            // set up tiles
            _tiles = new Tile[definition.CWid, definition.CHei];
            for (int i = 0; i < definition.GridTiles.Count; i++)
            {
                Tileset.Tile tilesetTile = tileset.Tiles[definition.GridTiles[i].T];

                int x = (int)definition.GridTiles[i].Px[0] / Consts.TILE_SIZE;
                int y = (int)definition.GridTiles[i].Px[1] / Consts.TILE_SIZE;

                GameObject gameObject = new GameObject("Tile [" + x + ", " + y + "]");
                gameObject.transform.SetParent(transform);
                Vector2 offset = (Vector2.right + Vector2.down) * (1f / 2f);
                gameObject.transform.localPosition = new Vector2(x, -y) + offset;

                _tiles[x, y] = gameObject.AddComponent<Tile>().Init(tilesetTile, sortingOrder);
            }

            return this;
        }
    }
}