
using data.entity;
using data.repository;
using structure.factory;

namespace structure.repository
{
    public class UserSessionRepository : RepositoryBase<UserSession>, IUserSessionRepository
    {
        public UserSessionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
