using UnityEngine;

namespace VED.Tilemaps
{
    public class Entity : MonoBehaviour
    {
        public virtual bool Visible
        {
            get { return gameObject.activeSelf; }
            set { gameObject.SetActive(value);  }
        }

        public virtual Entity Init(EntityInstance definition)
        {
            gameObject.SetActive(false);
            return this;
        }
    }
}