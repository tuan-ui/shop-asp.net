using Data.Common;
using Data.Infrastructure;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Data.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        IEnumerable<RevenueStatisticViewModel> GetRevenueStatistic(DateTime fromDate, DateTime toDate);
    }

    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<RevenueStatisticViewModel> GetRevenueStatistic(DateTime fromDate, DateTime toDate)
        {
            var query = from o in DbContext.Orders
                        where o.CreatedDate >= fromDate && o.CreatedDate <= toDate
                        group o by o.CreatedDate into g
                        select new RevenueStatisticViewModel
                        {
                            Date = (DateTime) g.Key, 
                            Revenues = g.Sum(x => x.TotalPrice),
                            Benefit = g.Sum(x => x.TotalPrice) 
                        };
            return query.ToList();
        }
    }
}