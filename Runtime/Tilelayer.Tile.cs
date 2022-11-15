using UnityEngine;

namespace VED.Tilemaps
{
    public partial class Tilelayer : MonoBehaviour
    {
        public class Tile : MonoBehaviour
        {
            private SpriteRenderer _spriteRenderer = null;

            public Tile Init(Tileset.Tile definition, int sortingOrder)
            {
                if (_spriteRenderer == null) _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
                _spriteRenderer.sprite = definition.Sprite;
                _spriteRenderer.sortingOrder = sortingOrder;
                return this;
            }
        }
    }
}
