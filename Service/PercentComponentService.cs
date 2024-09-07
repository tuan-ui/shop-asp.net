using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Infrastructure;
using Data.Repositories;
using Data.Models;

namespace Service
{
    public interface IPercentComponentService
    {
        IEnumerable<PercentComponent> GetAll();
    }
    public class PercentComponentService : IPercentComponentService
    {
        private IPercentComponentRepository _percentComponentRepository;

        public PercentComponentService(IPercentComponentRepository percentComponentRepository)
        {
            this._percentComponentRepository = percentComponentRepository;
        }

        public IEnumerable<PercentComponent> GetAll()
        {
            return _percentComponentRepository.GetAll();
        }
    }
}
