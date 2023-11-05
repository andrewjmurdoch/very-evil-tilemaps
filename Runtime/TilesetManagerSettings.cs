using UnityEngine;

namespace VED.Tilemaps
{
    [CreateAssetMenu(fileName = "TilesetManagerSettings", menuName = "VED/Tilemaps/TilesetManagerSettings", order = 0)]
    public class TilesetManagerSettings : ScriptableObject
    {
        // tile size decides the size of 1 tile in pixels
        // set by default to 16px/1u
        public int TileSize => _tileSize;
        [SerializeField] private int _tileSize = 16;
    }
}