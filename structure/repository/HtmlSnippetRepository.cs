
using data.entity;
using data.repository;
using structure.factory;

namespace structure.repository
{
    public class HtmlSnippetRepository : RepositoryBase<HtmlSnippet>, IHtmlSnippetRepository
    {
        public HtmlSnippetRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
