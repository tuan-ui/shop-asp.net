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
    public interface ISupplierService
    {
        Supplier Add(Supplier supplier);

        void Update(Supplier supplier);

        Supplier Delete(int id);

        IEnumerable<Supplier> GetAll();
        IEnumerable<Supplier> GetAll(string keyword);

        Supplier GetById(int id);

        void Save();
    }
    public class SupplierService : ISupplierService
    {
        private ISupplierRepository _SupplierRepository;
        private IUnitOfWork _unitOfWork;

        public SupplierService(ISupplierRepository SupplierRepository, IUnitOfWork unitOfWork)
        {
            this._SupplierRepository = SupplierRepository;
            this._unitOfWork = unitOfWork;
        }

        public Supplier Add(Supplier supplier)
        {
            return _SupplierRepository.Add(supplier);
        }

        public Supplier Delete(int id)
        {
            return _SupplierRepository.Delete(id);
        }

        public IEnumerable<Supplier> GetAll()
        {
            return _SupplierRepository.GetAll();
        }
        public IEnumerable<Supplier> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _SupplierRepository.GetMulti(x => x.Name.Contains(keyword) || x.Description.Contains(keyword));
            else
                return _SupplierRepository.GetAll();

        }

        public Supplier GetById(int id)
        {
            return _SupplierRepository.GetSingleById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Supplier supplier)
        {
            _SupplierRepository.Update(supplier);
        }
    }
}
