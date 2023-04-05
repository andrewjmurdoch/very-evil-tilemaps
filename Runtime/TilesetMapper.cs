using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace VED.Tilemaps
{
    [CreateAssetMenu(fileName = "TilesetMapper", menuName = "VED/Tilemaps/TilesetMapper")]

    public class TilesetMapper : ScriptableObject
    {
        [SerializeField] private List<SpriteAtlas> _spriteAtlases = new List<SpriteAtlas>();
        private Dictionary<string, SpriteAtlas> _spriteAtlasDictionary = null;

        public SpriteAtlas this[string name]
        {
            get
            {
                if (_spriteAtlasDictionary == null) InitSpriteAtlasDictionary();
                if (_spriteAtlasDictionary.TryGetValue(name, out SpriteAtlas spriteAtlas)) return spriteAtlas;
                return null;
            }
        }

        public void InitSpriteAtlasDictionary()
        {
            _spriteAtlasDictionary = new Dictionary<string, SpriteAtlas>();
            foreach (SpriteAtlas spriteAtlas in _spriteAtlases)
            {
                if (spriteAtlas == null) continue;
                if (_spriteAtlasDictionary.ContainsKey(spriteAtlas.name)) continue;

                _spriteAtlasDictionary.Add(spriteAtlas.name, spriteAtlas);
            }
        }
    }
}