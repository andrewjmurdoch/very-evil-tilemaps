using System.Collections.Generic;
using VED.Utilities;

namespace VED.Tilemaps
{
    public class TilesetManager : Singleton<TilesetManager>
    {
        public TilesetMapper TilesetMapper => _tilesetMapper;
        private TilesetMapper _tilesetMapper = null;

        public Dictionary<long, Tileset> Tilesets => _tilesets;
        private Dictionary<long, Tileset> _tilesets = new Dictionary<long, Tileset>();

        public void Init(TilesetMapper tilesetMapper, List<TilesetDefinition> definitions)
        {
            _tilesetMapper = tilesetMapper;

            _tilesets = new Dictionary<long, Tileset>();
            foreach (TilesetDefinition tilesetDefinition in definitions)
            {
                _tilesets.Add(tilesetDefinition.Uid, new Tileset().Init(tilesetDefinition));
            }
        }
    }

}