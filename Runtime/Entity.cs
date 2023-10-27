using System;
using UnityEngine;

namespace VED.Tilemaps
{
    public class Entity : MonoBehaviour
    {
        [Serializable]
        public class Reference
        {
            [SerializeField] public string entityIid;
            [SerializeField] public string layerIid;
            [SerializeField] public string levelIid;
            [SerializeField] public string worldIid;
        }

        public static Action<Entity> Spawned;
        public static Action<Entity> Despawned;

        public virtual bool Visible
        {
            get { return gameObject.activeSelf; }
            set { gameObject.SetActive(value);  }
        }

        public string ID => _id;
        protected string _id;

        public string LevelID => _levelID;
        protected string _levelID;

        public virtual Entity Init(EntityInstance definition, string levelID)
        {
            _id = definition.Iid;
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