using VED.Utilities;

namespace VED.Tilemaps
{
    public class EntityManager : Singleton<EntityManager>
    {
        public EntityMapper EntityMapper => _entityMapper;
        private EntityMapper _entityMapper = null;

        public void Init(EntityMapper entityMapper)
        {
            _entityMapper = entityMapper;
        }
    }
}