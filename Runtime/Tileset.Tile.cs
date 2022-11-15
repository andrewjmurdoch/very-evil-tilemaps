using UnityEngine;

namespace VED.Tilemaps
{
    public partial class Tileset
    {
        public class Tile
        {
            public Sprite Sprite => _sprite;
            protected Sprite _sprite = null;

            public Tile Init(Sprite sprite)
            {
                _sprite = sprite;

                return this;
            }
        }
    }
}
