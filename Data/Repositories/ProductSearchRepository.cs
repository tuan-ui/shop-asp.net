using Data.Infrastructure;
using Data.Models;


namespace Data.Repositories
{
    public interface IProductSearchRepository : IRepository<ProductSearch>
    {
    }
    public class ProductSearchRepository : RepositoryBase<ProductSearch>, IProductSearchRepository
    {
        public ProductSearchRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
