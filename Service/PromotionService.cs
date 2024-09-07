using Data.Infrastructure;
using Data.Models;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IPromotionService
    {
        Promotion Add(Promotion Promotion);

        void Update(Promotion Promotion);

        Promotion Delete(int id);

        IEnumerable<Promotion> GetAll();

        IEnumerable<Promotion> GetAll(string keyword);

        Promotion GetById(int id);

        Promotion GetByCode(string code);

        void Save();
    }
    public class PromotionService : IPromotionService
    {
        private IProductRepository _ProductRepository;
        private IPromotionRepository _promotionRepository;
        private IUnitOfWork _unitOfWork;

        public PromotionService(IProductRepository ProductyRepository, IUnitOfWork unitOfWork, IPromotionRepository promotionRepository)
        {
            this._ProductRepository = ProductyRepository;
            this._promotionRepository = promotionRepository;
            this._unitOfWork = unitOfWork;
        }

        public Promotion Add(Promotion Promotion)
        {
            return _promotionRepository.Add(Promotion);
        }

        public Promotion Delete(int id)
        {
            return _promotionRepository.Delete(id);
        }

        public IEnumerable<Promotion> GetAll()
        {
            return _promotionRepository.GetAll();
        }

        public IEnumerable<Promotion> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _promotionRepository.GetMulti(x => x.Name.Contains(keyword) || x.Name.Contains(keyword));
            else
                return _promotionRepository.GetAll();
        }

        public Promotion GetById(int id)
        {
            return _promotionRepository.GetSingleById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Promotion Promotion)
        {
            _promotionRepository.Update(Promotion);
        }

        public Promotion GetByCode(string code)
        {
            return _promotionRepository.GetByCode(code);
        }
    }
}
