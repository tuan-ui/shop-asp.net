using Data.Infrastructure;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface ISupplierRepository : IRepository<Supplier>
    {
        IEnumerable<Supplier> GetByAlias(string alias);
    }
    public class SupplierRepository : RepositoryBase<Supplier>, ISupplierRepository
    {
        public SupplierRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public IEnumerable<Supplier> GetByAlias(string alias)
        {
            return this.DbContext.Suppliers.Where(x => x.Alias == alias);
        }
    }
}
