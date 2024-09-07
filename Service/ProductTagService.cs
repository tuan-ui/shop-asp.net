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
    public interface IProductTagService
    {
        IEnumerable<Tag> GetAll();
        IEnumerable<ProductTag> GetById(int id);

    }
    public class ProductTagService : IProductTagService
    {
        private IProductTagRepository _ProductTagRepository;
        private ITagRepository _TagRepository;
        private IUnitOfWork _unitOfWork;

        public ProductTagService(IProductTagRepository ProductTagRepository, ITagRepository TagRepository, IUnitOfWork unitOfWork)
        {
            this._ProductTagRepository = ProductTagRepository;
            this._TagRepository = TagRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<Tag> GetAll()
        {
            return _TagRepository.GetAll();
        }

        public IEnumerable<ProductTag> GetById(int id)
        {
            return _ProductTagRepository.GetMulti(x => x.ProductID == id);
        }
    }
}
