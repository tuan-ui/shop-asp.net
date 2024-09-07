using AutoMapper;
using Data.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Service;
using Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Web.App_Start;
using Web.Common;
using Web.Infrastructure.Core;
using Web.Models;

namespace Web.Controllers
{
    public class ProductController : Controller
    {
        IProductService _productService;
        IProductCategoryService _productCategoryService;
        ICommentService _commentService;
        ISupplierService _supplierService;
        IProductTagService _productTagService;
        IOrderService _orderService;
        public ProductController(IProductService productService, IProductCategoryService productCategoryService, ICommentService commentService, 
            ISupplierService supplierService, IProductTagService productTagService, IOrderService orderService)
        {
            this._productService = productService;
            this._productCategoryService = productCategoryService;
            this._commentService = commentService;
            this._supplierService = supplierService;
            this._productTagService = productTagService;
            this._orderService = orderService;
        }
        // GET: Product
        public ActionResult Detail(int productId)
        {
            var productModel = _productService.GetById(productId);

            var viewModel = Mapper.Map<Product, ProductViewModel>(productModel);


            ViewBag.category = _productCategoryService.GetById(productModel.CategoryID);
            if (viewModel.MoreImages != null) { 
            List<string> listImages = new JavaScriptSerializer().Deserialize<List<string>>(viewModel.MoreImages);
            ViewBag.MoreImages = listImages;
            }

            var rateAVG = _commentService.ListCommentRateViewModel(productId);
            int rateSum = 0;
            int Rate1 = 0;
            int Rate2 = 0;
            int Rate3 = 0;
            int Rate4 = 0;
            int Rate5 = 0;
            foreach (Comment c in rateAVG)
            {
                rateSum += c.Rate;
                if (c.Rate == 1) Rate1++;
                else if (c.Rate == 2) Rate2++;
                else if (c.Rate == 3) Rate3++;
                else if (c.Rate == 4) Rate4++;
                else if (c.Rate == 5) Rate5++;
            }
            float value = (float)rateSum / rateAVG.Count();
            ViewBag.rateavg = (float)Math.Round(value, 2);
            ViewBag.ratecount = rateAVG.Count();
            ViewBag.rate1 = Rate1;
            ViewBag.rate2 = Rate2;
            ViewBag.rate3 = Rate3;
            ViewBag.rate4 = Rate4;
            ViewBag.rate5 = Rate5;

            ViewBag.Tags = Mapper.Map<IEnumerable<Tag>, IEnumerable<TagViewModel>>(_productService.GetListTagByProductId(productId));

            return View(viewModel);
        }

        public ActionResult Category(int id, int page = 1, string sort="")
        {
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize9"));
            Session[CommonConstants.Catagory] = id;
            int totalRow = 0;
            var productModel = _productService.GetListProductByCategoryIdPaging(id, page, pageSize, sort, out totalRow);
            var productViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productModel);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);
            ViewBag.Sort = sort;
            var category = _productCategoryService.GetById(id);
            ViewBag.Category = Mapper.Map<ProductCategory, ProductCategoryViewModel>(category);
            var paginationSet = new PaginationSet<ProductViewModel>()
            {
                Items = productViewModel,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };
            return View(paginationSet);
        }
        public ActionResult Supplier(int id, int page = 1, string sort = "")
        {
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
            int totalRow = 0;
            var productModel = _productService.GetListProductByCategoryIdPaging(id, page, pageSize, sort, out totalRow);
            var productViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productModel);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);

            var category = _productCategoryService.GetById(id);
            ViewBag.Category = Mapper.Map<ProductCategory, ProductCategoryViewModel>(category);
            ViewBag.Sort = sort;
            var paginationSet = new PaginationSet<ProductViewModel>()
            {
                Items = productViewModel,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };
            return View(paginationSet);
        }
        public ActionResult Search(string keyword, int page = 1, string sort = "")
        {
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize9"));
            int totalRow = 0;
            var productModel = _productService.Search(keyword, page, pageSize, sort, out totalRow);
            var productViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productModel);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);

            ViewBag.Keyword = keyword;
            ViewBag.Sort = sort;
            var paginationSet = new PaginationSet<ProductViewModel>()
            {
                Items = productViewModel,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };

            return View(paginationSet);
        }
        public ActionResult ListByTag(string tagId, int page = 1, string sort = "")
        {
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize9"));
            int totalRow = 0;
            var productModel = _productService.GetListProductByTag(tagId, page, pageSize, sort, out totalRow);
            var productViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productModel);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);
            ViewBag.Sort = sort;
            ViewBag.Tag = Mapper.Map<Tag, TagViewModel>(_productService.GetTag(tagId));
            var paginationSet = new PaginationSet<ProductViewModel>()
            {
                Items = productViewModel,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };

            return View(paginationSet);
        }
        public ActionResult SearchAdvance(int id, string keyword, int page = 1, string sort = "")
        {
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize9"));
            int totalRow = 0;
            var productModel = _productService.SearchAdvance(id, keyword, page, pageSize, sort, out totalRow);
            var productViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productModel);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);

            ViewBag.Keyword = keyword;
            ViewBag.Sort = sort;
            var paginationSet = new PaginationSet<ProductViewModel>()
            {
                Items = productViewModel,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };

            return View(paginationSet);
        }
        public ActionResult SearchLaptop(string tag, int page = 1, string sort = "")
        {
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize9"));
            int totalRow = 0;
            var productModel = _productService.SearchLaptop(tag, page, pageSize, sort, out totalRow);
            var productViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productModel);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);

            ViewBag.Tag = tag;
            ViewBag.Sort = sort;
            var paginationSet = new PaginationSet<ProductViewModel>()
            {
                Items = productViewModel,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };

            return View(paginationSet);
        }
        public JsonResult GetListProductByName(string keyword)
        {
            var model = _productService.GetListProductByName(keyword);
            return Json(new
            {
                data = model
            }, JsonRequestBehavior.AllowGet);
        }
        [ChildActionOnly]
        public ActionResult _ChildComment(int parentid, int productid)
        {
            var data = _commentService.ListCommentViewModel(parentid, productid);
            
            var sessionuser =Session[CommonConstants.USER_SESSION];
            var dataViewModel = Mapper.Map<IEnumerable<Comment>, IEnumerable<CommentViewModel>>(data);
            string userName = User.Identity.GetUserName();
            ViewBag.productId = productid;
            if (userName != null)
            {
                UserManager<ApplicationUser> userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = userManager.FindByName(userName);
                var applicationGroupService = ServiceFactory.Get<IApplicationGroupService>();
                var listGroup = applicationGroupService.GetListGroupByUserId(user.Id);
                if (listGroup.Any(x => x.Name == CommonConstants.Admin))
                    ViewBag.isAdmin = true;
            }
            return PartialView("~/Views/Shared/_ChildComment.cshtml", dataViewModel);
        }
        public ActionResult _Comment(int productid) { 
        

            var sessionUser = Session[CommonConstants.USER_SESSION];
            if (sessionUser != null)
            {
                ViewBag.UserID = sessionUser;
            }
            ViewBag.productId = productid;
            var lcomment = _commentService.ListCommentViewModel(0, productid);

            string userName = User.Identity.GetUserName();
            if (userName != null)
            {
                UserManager<ApplicationUser> userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = userManager.FindByName(userName);
                var applicationGroupService = ServiceFactory.Get<IApplicationGroupService>();
                var listGroup = applicationGroupService.GetListGroupByUserId(user.Id);
                if (listGroup.Any(x => x.Name == CommonConstants.Admin))
                    ViewBag.isAdmin = true;
            }

            IEnumerable<CommentViewModel>  comment = Mapper.Map<IEnumerable<Comment>, IEnumerable<CommentViewModel>>(lcomment);
            return PartialView("~/Views/Shared/_Comment.cshtml", comment);
        }
        [HttpPost]
        public JsonResult AddNewComment(int productid, string userid, int parentid, string commentmsg, string rate)
        {
            try
            {
                Comment comment = new Comment();
                var sessionUserName = User.Identity.GetUserName();
                var sessionUserId = User.Identity.GetUserId();
                if (sessionUserName == null)
                {
                    return Json(new
                    {
                        status = false,
                        mgs="Chưa đăng nhập"
                    });
                }
                var orders = _orderService.GetListOrderByUserName(sessionUserName);
                List<OrderDetail> listOrderDetail = new List<OrderDetail>();
                foreach(var order in orders)
                {
                    var orderDetail = _orderService.GetListOrderDetail(order.ID);
                    listOrderDetail.AddRange(orderDetail);
                }
                if(!listOrderDetail.Any(order => order.ProductID == productid))
                {
                    return Json(new
                    {
                        status = false,
                        mgs = "Bạn chưa mua hàng nên chưa đc bình luận"
                    });
                }
                comment.CommentMsg = commentmsg;
                comment.ProductID = productid;
                comment.UserID = sessionUserId.ToString();
                comment.ParentID = parentid;
                comment.Rate = Convert.ToInt16(rate);
                comment.CommentDate = DateTime.Now;

                bool addcomment = _commentService.Insert(comment);
                if (addcomment == true)
                {
                    return Json(new
                    {
                        status = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = false,
                        mgs = "Bình luận lỗi"
                    });
                }
            }
            catch
            {
                return Json(new
                {
                    status = false,
                    mgs = "Bình luận lỗi"
                });
            }
        }
        public ActionResult GetComment(int productid)
        {
            var data = _commentService.ListCommentViewModel(0, productid);
            var dataViewModel = Mapper.Map<IEnumerable<Comment>, IEnumerable<CommentViewModel>>(data);
            string userId = User.Identity.GetUserId();
            if (userId != null)
            {
                UserManager<ApplicationUser> userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = userManager.FindById(userId);
                var applicationGroupService = ServiceFactory.Get<IApplicationGroupService>();
                var listGroup = applicationGroupService.GetListGroupByUserId(user.Id);
                if (listGroup.Any(x => x.Name == CommonConstants.Admin))
                    ViewBag.isAdmin = true;
            }
            return PartialView("~/Views/Shared/_Comment.cshtml", dataViewModel);
        }
        public ActionResult GetCommentAll(int productid)
        {
            var data = _commentService.ListCommentViewModelAll(0, productid);
            var dataViewModel = Mapper.Map<IEnumerable<Comment>, IEnumerable<CommentViewModel>>(data);
            string userId = User.Identity.GetUserId();
            if (userId != null)
            {
                UserManager<ApplicationUser> userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = userManager.FindById(userId);
                var applicationGroupService = ServiceFactory.Get<IApplicationGroupService>();
                var listGroup = applicationGroupService.GetListGroupByUserId(user.Id);
                if (listGroup.Any(x => x.Name == CommonConstants.Admin))
                    ViewBag.isAdmin = true;
            }
            return PartialView("~/Views/Shared/_Comment.cshtml", dataViewModel);
        }
        public ActionResult FilterProducts(List<string> suppliers, decimal? minPrice = -1, decimal? maxPrice = decimal.MaxValue, int page = 1, string sort = "")
        {
            bool hasCata=false;
            bool hasSupp = false;
            int cata =-1;
            if (Session[CommonConstants.Catagory] != null) {
                hasCata = true;
               cata = (int)Session[CommonConstants.Catagory];
            }
            if (suppliers != null)
                hasSupp = true;
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize9"));
            int totalRow = 0;

            if (minPrice == -1) {
                suppliers = Session[CommonConstants.Suppliers] as List<string>; ;
                minPrice = Convert.ToDecimal(Session[CommonConstants.MinPrice]);
                maxPrice = Convert.ToDecimal(Session[CommonConstants.MaxPrice]);
            }
            IEnumerable<Product> filteredProducts = new List<Product>();
            if (hasSupp == true && hasCata == true)
            {
                var supplierIds = _supplierService.GetAll()
                       .Where(s => suppliers.Contains(s.Name))
                       .Select(s => s.ID);


                filteredProducts = _productService.GetAll()
                        .Where(p => (suppliers.Count == 0 || supplierIds.Contains(p.SupplierID)) &&
                                    (p.Price >= minPrice) &&
                                    (p.Price <= maxPrice) && p.CategoryID == cata);
                  
            }
            if (hasSupp != true && hasCata == true)
            {

                filteredProducts = _productService.GetAll()
                        .Where(p =>
                                    (p.Price >= minPrice) &&
                                    (p.Price <= maxPrice) && p.CategoryID == cata);
                      
            }
            if (hasSupp == true && hasCata != true)
            {
                var supplierIds = _supplierService.GetAll()
                       .Where(s => suppliers.Contains(s.Name))
                       .Select(s => s.ID);


                filteredProducts = _productService.GetAll()
                        .Where(p => (suppliers.Count == 0 || supplierIds.Contains(p.SupplierID)) &&
                                    (p.Price >= minPrice) &&
                                    (p.Price <= maxPrice));
                       
            }
            if (hasSupp != true && hasCata != true)
            {

                filteredProducts = _productService.GetAll()
                        .Where(p =>
                                    (p.Price >= minPrice) &&
                                    (p.Price <= maxPrice));
                        
            }

            ViewBag.Sort = sort;
            switch (sort)
            {
                case "popular":
                    filteredProducts = filteredProducts.OrderByDescending(x => x.ViewCount);
                    break;
                case "discount":
                    filteredProducts = filteredProducts.OrderByDescending(x => x.PromotionPrice.HasValue);
                    break;
                case "price":
                    filteredProducts = filteredProducts.OrderBy(x => x.Price);
                    break;
                case "priceDes":
                    filteredProducts = filteredProducts.OrderByDescending(x => x.Price);
                    break;
                default:
                    filteredProducts = filteredProducts.OrderByDescending(x => x.CreatedDate);
                    break;
            }


            totalRow = filteredProducts.ToList().Count;
            var productsForCurrentPage = filteredProducts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var productViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productsForCurrentPage);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);
            var paginationSet = new PaginationSet<ProductViewModel>()
            {
                Items = productViewModel,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };
            Session.Add(CommonConstants.Suppliers, suppliers);
            Session.Add(CommonConstants.MinPrice, minPrice);
            Session.Add(CommonConstants.MaxPrice, maxPrice);
          
            return View(paginationSet);
        }
        [HttpPost]
        public JsonResult DeleteComment(int id,int productid)
        {
            try
            {
                var cmt = _commentService.Delete(id);
                _commentService.Save();
                if (cmt !=null)
                {
                    return Json(new
                    {
                        status = true,
                        productid= productid
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = false
                    });
                }
            }
            catch
            {
                return Json(new
                {
                    status = false
                });
            }
        }
        public ActionResult FilterLaptops(List<string> tags, decimal? minPrice = -1, decimal? maxPrice = decimal.MaxValue, int page = 1, string sort = "")
        {
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize9"));
            int totalRow = 0;

            if (minPrice == -1)
            {
                tags = Session[CommonConstants.Tags] as List<string>;
                minPrice = Convert.ToDecimal(Session[CommonConstants.MinPrice]);
                maxPrice = Convert.ToDecimal(Session[CommonConstants.MaxPrice]);
            }

            if (tags == null)
            {
                tags = new List<string>();
            }

            IEnumerable<Product> filteredProducts = _productService.GetAll()
             .Where(p => (tags.Count == 0 || (p.Tags != null && tags.Any(tag => p.Tags.Split(',').Contains(tag)))) &&
                 (p.Price >= minPrice) &&
                 (p.Price <= maxPrice));

            ViewBag.Sort = sort;
            switch (sort)
            {
                case "popular":
                    filteredProducts = filteredProducts.OrderByDescending(x => x.ViewCount);
                    break;
                case "discount":
                    filteredProducts = filteredProducts.OrderByDescending(x => x.PromotionPrice.HasValue);
                    break;
                case "price":
                    filteredProducts = filteredProducts.OrderBy(x => x.Price);
                    break;
                case "priceDes":
                    filteredProducts = filteredProducts.OrderByDescending(x => x.Price);
                    break;
                default:
                    filteredProducts = filteredProducts.OrderByDescending(x => x.CreatedDate);
                    break;
            }

            totalRow = filteredProducts.Count();
            var productsForCurrentPage = filteredProducts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var productViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productsForCurrentPage);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);
            var paginationSet = new PaginationSet<ProductViewModel>()
            {
                Items = productViewModel,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };

            Session[CommonConstants.Tags] = tags;
            Session[CommonConstants.MinPrice] = minPrice;
            Session[CommonConstants.MaxPrice] = maxPrice;

            return View(paginationSet);
        }
        public ActionResult FilterComponent(List<string> tags, decimal? minPrice = -1, decimal? maxPrice = decimal.MaxValue, int page = 1, string sort = "")
        {
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize9"));
            int totalRow = 0;

            if (minPrice == -1)
            {
                tags = Session[CommonConstants.Tags] as List<string>;
                minPrice = Convert.ToDecimal(Session[CommonConstants.MinPrice]);
                maxPrice = Convert.ToDecimal(Session[CommonConstants.MaxPrice]);
            }

            if (tags == null)
            {
                tags = new List<string>();
            }

            IEnumerable<Product> filteredProducts = _productService.GetAll()
             .Where(p => (tags.Count == 0 || (p.Tags != null && tags.Any(tag => p.Tags.Split(',').Contains(tag)))) &&
                 (p.Price >= minPrice) &&
                 (p.Price <= maxPrice));

            ViewBag.Sort = sort;
            switch (sort)
            {
                case "popular":
                    filteredProducts = filteredProducts.OrderByDescending(x => x.ViewCount);
                    break;
                case "discount":
                    filteredProducts = filteredProducts.OrderByDescending(x => x.PromotionPrice.HasValue);
                    break;
                case "price":
                    filteredProducts = filteredProducts.OrderBy(x => x.Price);
                    break;
                case "priceDes":
                    filteredProducts = filteredProducts.OrderByDescending(x => x.Price);
                    break;
                default:
                    filteredProducts = filteredProducts.OrderByDescending(x => x.CreatedDate);
                    break;
            }

            totalRow = filteredProducts.Count();
            var productsForCurrentPage = filteredProducts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var productViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productsForCurrentPage);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);
            var paginationSet = new PaginationSet<ProductViewModel>()
            {
                Items = productViewModel,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };

            Session[CommonConstants.Tags] = tags;
            Session[CommonConstants.MinPrice] = minPrice;
            Session[CommonConstants.MaxPrice] = maxPrice;

            return View(paginationSet);
        }
    }
}
