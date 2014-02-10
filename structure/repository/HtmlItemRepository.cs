
using data.entity;
using data.repository;
using structure.factory;

namespace structure.repository
{
    public class HtmlItemRepository : RepositoryBase<HtmlItem>, IHtmlItemRepository
    {
        public HtmlItemRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
