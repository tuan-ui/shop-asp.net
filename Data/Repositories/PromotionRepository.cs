using Data.Infrastructure;
using Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace Data.Repositories
{
    public interface IPromotionRepository : IRepository<Promotion>
    {
        Promotion GetByCode(string code);
    }

    class PromotionRepository : RepositoryBase<Promotion>, IPromotionRepository
    {
        public PromotionRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
        public Promotion GetByCode(string code)
        {
            var query = from p in DbContext.Promotions
                        where p.Code == code 
                        && p.Status ==true
                        select p;

            return query.FirstOrDefault();
        }
    }
}
