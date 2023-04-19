using System;
using UnityEngine;

namespace VED.Tilemaps
{
    public class Entity : MonoBehaviour
    {
        public static Action<Entity> Spawned;
        public static Action<Entity> Despawned;

        public virtual bool Visible
        {
            get { return gameObject.activeSelf; }
            set { gameObject.SetActive(value);  }
        }

        public string LevelID => _levelID;
        private string _levelID;

        public virtual Entity Init(EntityInstance definition, string levelID)
        {
            _levelID = levelID;
            Spawned?.Invoke(this);
            gameObject.SetActive(false);

            return this;
        }

        public virtual void OnDestroy()
        {
            Despawned?.Invoke(this);
        }
    }
}