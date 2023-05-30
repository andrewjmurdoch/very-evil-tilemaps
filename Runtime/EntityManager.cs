using System.Collections.Generic;
using VED.Utilities;

namespace VED.Tilemaps
{
    public class EntityManager : Singleton<EntityManager>
    {
        public EntityMapper EntityMapper => _entityMapper;
        private EntityMapper _entityMapper = null;

        public Dictionary<string, Dictionary<string, Entity>> TilelevelEntities => _tilelevelEntities;
        private Dictionary<string, Dictionary<string, Entity>> _tilelevelEntities = new Dictionary<string, Dictionary<string, Entity>>();

        public List<Entity> Entities => _entities;
        private List<Entity> _entities = new List<Entity>();

        public void Init(EntityMapper entityMapper)
        {
            _entityMapper = entityMapper;
            _tilelevelEntities = new Dictionary<string, Dictionary<string, Entity>>();
            _entities = new List<Entity>();

            // listen to spawning events
            Entity.Spawned += AddEntity;
            Entity.Despawned += RemoveEntity;
        }

        public void Deinit()
        {
            Entity.Spawned -= AddEntity;
            Entity.Despawned -= RemoveEntity;

            _tilelevelEntities.Clear();
            _entities.Clear();
        }

        private void AddEntity(Entity entity)
        {
            if (_tilelevelEntities.TryGetValue(entity.LevelID, out Dictionary<string, Entity> tilelevelEntities))
            {
                tilelevelEntities.Add(entity.ID, entity);
                _entities.Add(entity);
                return;
            }

            _tilelevelEntities.Add(entity.LevelID, new Dictionary<string, Entity>() { { entity.ID, entity } });
            _entities.Add(entity);
        }

        private void RemoveEntity(Entity entity)
        {
            if (_tilelevelEntities.TryGetValue(entity.LevelID, out Dictionary<string, Entity> tilelevelEntities))
            {
                tilelevelEntities.Remove(entity.ID);
            }
            _entities.Remove(entity);
        }
    }
}