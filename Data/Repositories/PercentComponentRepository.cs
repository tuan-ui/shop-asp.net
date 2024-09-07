using System.Collections.Generic;
using System.Linq;
using Data.Infrastructure;
using Data.Models;

namespace Data.Repositories
{
    public interface IPercentComponentRepository : IRepository<PercentComponent>
    {
    }
    class PercentComponentRepository : RepositoryBase<PercentComponent>, IPercentComponentRepository
    {
        public PercentComponentRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
