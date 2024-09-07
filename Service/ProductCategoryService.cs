using System.Collections.Generic;
using Data.Infrastructure;
using Data.Repositories;
using Data.Models;
using Service.Common;

namespace Service
{
    public interface IProductCategoryService
    {
        ProductCategory Add(ProductCategory ProductCategory);

        void Update(ProductCategory ProductCategory);

        ProductCategory Delete(int id);

        IEnumerable<ProductCategory> GetAll();
        IEnumerable<ProductCategory> GetAll(string keyword);

        IEnumerable<ProductCategory> GetAllByParentId(int parentId);

        ProductCategory GetById(int id);

        void Save();
    }

    public class ProductCategoryService : IProductCategoryService
    {
        private IProductCategoryRepository _ProductCategoryRepository;
        private ISearchRepository _searchRepository;
        private IProductSearchRepository _productSearchRepository;
        private IUnitOfWork _unitOfWork;

        public ProductCategoryService(IProductCategoryRepository ProductCategoryRepository, IUnitOfWork unitOfWork, ISearchRepository searchRepository, IProductSearchRepository productSearchRepository)
        {
            this._ProductCategoryRepository = ProductCategoryRepository;
            this._searchRepository = searchRepository;
            this._productSearchRepository = productSearchRepository;
            this._unitOfWork = unitOfWork;
        }

        public ProductCategory Add(ProductCategory ProductCategory)
        {
            var productCategory = _ProductCategoryRepository.Add(ProductCategory);
            _unitOfWork.Commit();
            if (!string.IsNullOrEmpty(ProductCategory.Searchs))
            {
                string[] searchs = ProductCategory.Searchs.Split(',');
                for (var i = 0; i < searchs.Length; i++)
                {
                    var searchId = StringHelper.ToUnsignString(searchs[i]);
                    if (_searchRepository.Count(x => x.ID == searchId) == 0)
                    {
                        Search search = new Search();
                        search.ID = searchId;
                        search.Name = searchs[i];
                        _searchRepository.Add(search);
                    }

                    ProductSearch productSearch = new ProductSearch();
                    productSearch.CategoryID = ProductCategory.ID;
                    productSearch.SearchID = searchId;
                    _ProductCategoryRepository.Add(ProductCategory);
                }
            }
            return productCategory;
        }

        public ProductCategory Delete(int id)
        {
            return _ProductCategoryRepository.Delete(id);
        }

        public IEnumerable<ProductCategory> GetAll()
        {
            return _ProductCategoryRepository.GetAll();
        }
        public IEnumerable<ProductCategory> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _ProductCategoryRepository.GetMulti(x => x.Name.Contains(keyword) || x.Description.Contains(keyword));
            else
                return _ProductCategoryRepository.GetAll();

        }

        public IEnumerable<ProductCategory> GetAllByParentId(int parentId)
        {
            return _ProductCategoryRepository.GetMulti(x => x.Status && x.ParentID == parentId);
        }

        public ProductCategory GetById(int id)
        {
            return _ProductCategoryRepository.GetSingleById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(ProductCategory ProductCategory)
        {
            _ProductCategoryRepository.Update(ProductCategory);
            if (!string.IsNullOrEmpty(ProductCategory.Searchs))
            {
                string[] searchs = ProductCategory.Searchs.Split(',');
                for (var i = 0; i < searchs.Length; i++)
                {
                    var searchId = StringHelper.ToUnsignString(searchs[i]);
                    if (_searchRepository.Count(x => x.ID == searchId) == 0)
                    {
                        Search search = new Search();
                        search.ID = searchId;
                        search.Name = searchs[i];
                        _searchRepository.Add(search);
                    }
                    _productSearchRepository.DeleteMulti(x => x.CategoryID == ProductCategory.ID);
                    ProductSearch productSearch = new ProductSearch();
                    productSearch.CategoryID = ProductCategory.ID;
                    productSearch.SearchID = searchId;
                    _productSearchRepository.Add(productSearch);
                }
            }
        }

    }
}