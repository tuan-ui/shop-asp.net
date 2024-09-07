using AutoMapper;
using Data.Models;
using Service;
using Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Common;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        IProductCategoryService _productCategoryService;
        ISupplierService _supplierService;
        IProductService _productService;
        ICommonService _commonService;
        public HomeController (IProductCategoryService productCategoryService, IProductService productService,
            ICommonService commonService, ISupplierService supplierService)
        {
            _productCategoryService = productCategoryService;
            _commonService = commonService;
            _productService = productService;
            _supplierService = supplierService;
        }
        [OutputCache(Duration = 60, Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult Index( int page = 1, string sort = "")
        {
            var homeViewModel = new HomeViewModel();
            Session[CommonConstants.Catagory] = null;
            var lastestProductModel = _productService.GetLastest(3);
            var topSaleProductModel = _productService.GetHotProduct(3);
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize9"));
            int totalRow = 0;
            var allProductModel = _productService.GetListProductPaging(page, pageSize, sort, out totalRow);
            var lastestProductViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(lastestProductModel);
            var topSaleProductViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(topSaleProductModel);
            var allProductViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(allProductModel);
            homeViewModel.LastestProducts = lastestProductViewModel;
            homeViewModel.TopSaleProducts = topSaleProductViewModel;
            homeViewModel.AllProducts = allProductViewModel;
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage"));
            ViewBag.Page = page;
            ViewBag.TotalCount = totalRow;
            ViewBag.TotalPages = totalPage;
            ViewBag.Sort = sort;
            try
            {
                homeViewModel.Title = _commonService.GetSystemConfig(CommonConstants.HomeTitle).ValueString;
                homeViewModel.MetaKeyword = _commonService.GetSystemConfig(CommonConstants.HomeMetaKeyword).ValueString;
                homeViewModel.MetaDescription = _commonService.GetSystemConfig(CommonConstants.HomeMetaDescription).ValueString;
            }
            catch
            {

            }

            return View(homeViewModel);
        }

        [ChildActionOnly]
        [OutputCache(Duration = 3600)]
        public ActionResult Footer()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult Header()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult Category()
        {
            var model = _productCategoryService.GetAll();
            var suppliers = _supplierService.GetAll();
            var listproductCategoryViewModel = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable< ProductCategoryViewModel> >(model);
            var listsupplierViewModel = Mapper.Map<IEnumerable<Supplier>, IEnumerable<SupplierViewModel>>(suppliers);
            ViewBag.listsupplierViewModel = listsupplierViewModel;
            var laptops = _productService.GetListProductByCategoryId(18);
            ViewBag.listproductLaptopViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(laptops);
            return PartialView(listproductCategoryViewModel);
        }
    }
}