using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace VED.Tilemaps
{
    [CreateAssetMenu(fileName = "TilesetMapper", menuName = "VED/Tilemaps/TilsetMapper")]

    public class TilesetMapper : ScriptableObject
    {
        [SerializeField] private List<SpriteAtlas> _spriteAtlases = new List<SpriteAtlas>();
        private Dictionary<string, SpriteAtlas> _spriteAtlasDictionary = new Dictionary<string, SpriteAtlas>();

        public SpriteAtlas this[string name]
        {
            get
            {
                if (!_spriteAtlasDictionary.ContainsKey(name)) return null;
                return _spriteAtlasDictionary[name];
            }
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            _spriteAtlasDictionary.Clear();
            foreach (SpriteAtlas spriteAtlas in _spriteAtlases)
            {
                if (spriteAtlas == null) continue;
                if (_spriteAtlasDictionary.ContainsKey(spriteAtlas.name)) continue;

                _spriteAtlasDictionary.Add(spriteAtlas.name, spriteAtlas);
            }
        }
#endif
    }
}
