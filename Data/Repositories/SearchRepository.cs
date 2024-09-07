using Data.Infrastructure;
using Data.Models;


namespace Data.Repositories
{
    public interface ISearchRepository : IRepository<Search>
    {
    }
    public class SearchRepository : RepositoryBase<Search>, ISearchRepository
    {
        public SearchRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
