using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

namespace VED.Tilemaps
{
    public partial class Tileset
    {
        //private const string SPRITEATLAS_EXTENSION = ".spriteatlas";
        
        public Tile[] Tiles => _tiles;
        protected Tile[] _tiles = null;

        public virtual Tileset Init(TilesetDefinition definition)
        {
            // get size of tileset
            int size = (int)((definition.PxWid / Consts.TILE_SIZE) * (definition.PxHei / Consts.TILE_SIZE));
            _tiles = new Tile[size];

            if (definition.RelPath == null) return null;

            // get tileset
            int start = definition.RelPath.LastIndexOf('/') + 1;
            int end = definition.RelPath.LastIndexOf('.');
            string name = definition.RelPath.Substring(start, end - start);
            SpriteAtlas spriteAtlas = TilesetManager.Instance.TilesetMapper[name];

            if (spriteAtlas == null) return null;

            // create tiles
            for (int i = 0; i < size; i++)
            {
                Sprite sprite = spriteAtlas.GetSprite(name + '_' + i.ToString());
                _tiles[i] = new Tile().Init(sprite);
            }

            return this;
        }
    }
}
