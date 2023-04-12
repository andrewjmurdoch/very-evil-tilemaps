using System.Collections.Generic;
using VED.Utilities;

namespace VED.Tilemaps
{
    public class EntityManager : Singleton<EntityManager>
    {
        public EntityMapper EntityMapper => _entityMapper;
        private EntityMapper _entityMapper = null;

        public Dictionary<long, List<Entity>> TilelevelEntities => _tilelevelEntities;
        private Dictionary<long, List<Entity>> _tilelevelEntities = new Dictionary<long, List<Entity>>();

        public List<Entity> Entities => _entities;
        private List<Entity> _entities = new List<Entity>();


        public void Init(EntityMapper entityMapper)
        {
            _entityMapper = entityMapper;

            // listen to spawning events
            Entity.Spawned += AddEntity;
            Entity.Despawned += RemoveEntity;
        }

        private void AddEntity(Entity entity)
        {
            if (_tilelevelEntities.TryGetValue(entity.LevelID, out List<Entity> tilelevelEntities))
            {
                tilelevelEntities.Add(entity);
                _entities.Add(entity);
                return;
            }

            _tilelevelEntities.Add(entity.LevelID, new List<Entity>() { entity });
            _entities.Add(entity);
        }

        private void RemoveEntity(Entity entity)
        {
            if (_tilelevelEntities.TryGetValue(entity.LevelID, out List<Entity> tilelevelEntities))
            {
                tilelevelEntities.Remove(entity);
            }
            _entities.Remove(entity);
        }
    }
}