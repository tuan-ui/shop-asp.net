using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Data.Models;
using Service;
using Web.Models;
using Web.App_Start;
using Microsoft.AspNet.Identity;
using Web.Infrastructure.Extensions;
using System.Configuration;
using Data;
using Service.Common;

namespace Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        IProductService _productService;
        IOrderService _orderService;
        IVnPayService _vnPayService;
        IPromotionService _promotionService;
        private ApplicationUserManager _userManager;
        public ShoppingCartController(IOrderService orderService, IProductService productService, ApplicationUserManager userManager, IVnPayService vnPayService,IPromotionService promotionService)
        {
            this._productService = productService;
            this._userManager = userManager;
            this._orderService = orderService;
            this._vnPayService = vnPayService;
            this._promotionService = promotionService;
        }
        // GET: ShoppingCart
        public ActionResult Index()
        {
            if (Session[CommonConstants.SessionCart] == null)
                Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            return View();
        }
        public ActionResult CheckOut()
        {
            if (Session[CommonConstants.SessionCart] == null)
            {
                return Redirect("/gio-hang.html");
            }
            return View();
        }
        public JsonResult GetUser()
        {
            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var user = _userManager.FindById(userId);
                return Json(new
                {
                    data = user,
                    status = true
                });
            }
            return Json(new
            {
                status = false
            });
        }
        public JsonResult CreateOrder(string orderViewModel)
        {
            var order = new JavaScriptSerializer().Deserialize<OrderViewModel>(orderViewModel);
            var orderNew = new Order();

            orderNew.UpdateOrder(order);

            if (Request.IsAuthenticated)
            {
                orderNew.CustomerId = User.Identity.GetUserId();
                orderNew.CreatedBy = User.Identity.GetUserName();
            }

            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            int b=cart.Count();
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            bool isEnough = true;
            foreach (var item in cart)
            {
                var detail = new OrderDetail();
                detail.ProductID = item.ProductId;
                detail.Quantity = item.Quantity;
                detail.Price = (decimal)item.Product.PromotionPrice;
                orderDetails.Add(detail);
                isEnough = _productService.SellProduct(item.ProductId, item.Quantity);
            }
            if (isEnough)
            {
                _orderService.Create(orderNew, orderDetails);
                _productService.Save();
                return Json(new
                {
                    orderId= orderNew.ID,
                    status = true
                });
            }
            else
            {
                var message = "Không đủ hàng";
                foreach (var item in cart)
                {
                    if (!_productService.SellProduct(item.ProductId, item.Quantity))
                    {
                        if (item.Quantity >= 20)
                        {
                            message = "Không đủ hàng " + item.Product.Name.ToString() + " Liện hệ với chúng tôi để đặt hàng số lượng lớn ";
                        }
                        else
                        {
                            message = "Không đủ hàng " + item.Product.Name.ToString();
                        }
                    }
                }
                return Json(new
                {
                    status = false,
                    message = message
                });
            }
        }
        public JsonResult GetAll()
        {
            if (Session[CommonConstants.SessionCart] == null)
                Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            return Json(new
            {
                data = cart,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Add(int productId)
        {
            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            var product = _productService.GetById(productId);
            if (cart == null)
            {
                cart = new List<ShoppingCartViewModel>();
            }
            if (product.Quantity == 0)
            {
                return Json(new
                {
                    status = false,
                    message = "Sản phẩm này hiện đang hết hàng"
                });
            }
            if (cart.Any(x => x.ProductId == productId))
            {
                foreach (var item in cart)
                {
                    if (item.ProductId == productId)
                    {
                        item.Quantity += 1;
                    }
                }
            }
            else
            {
                ShoppingCartViewModel newItem = new ShoppingCartViewModel();
                newItem.ProductId = productId;
                newItem.Product = Mapper.Map<Product, ProductViewModel>(product);
                newItem.Quantity = 1;
                cart.Add(newItem);
            }

            Session[CommonConstants.SessionCart] = cart;
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult DeleteItem(int productId)
        {
            var cartSession = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            if (cartSession != null)
            {
                cartSession.RemoveAll(x => x.ProductId == productId);
                Session[CommonConstants.SessionCart] = cartSession;
                return Json(new
                {
                    status = true
                });
            }
            return Json(new
            {
                status = false
            });
        }

        [HttpPost]
        public JsonResult Update(string cartData)
        {
            var cartViewModel = new JavaScriptSerializer().Deserialize<List<ShoppingCartViewModel>>(cartData);

            var cartSession = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            foreach (var item in cartSession)
            {
                foreach (var jitem in cartViewModel)
                {
                    if (item.ProductId == jitem.ProductId)
                    {
                        item.Quantity = jitem.Quantity;
                    }
                }
            }

            Session[CommonConstants.SessionCart] = cartSession;
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult DeleteAll()
        {
            Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            return Json(new
            {
                status = true
            });
        }
        public ActionResult CreatePaymentUrl(string orderViewModel)
        {
            var orderVM = new JavaScriptSerializer().Deserialize<OrderViewModel>(orderViewModel);
            if(orderVM.TotalPrice>200000000)
            {
                var code = "Số tiền thanh toán quá hạn mức 200 triệu của VNPay";
                return Json(new { code = code });
            }
            Session[CommonConstants.orderViewModel] = orderViewModel;
            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            int b = cart.Count();
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            bool isEnough = true;
            foreach (var item in cart)
            {
                var detail = new OrderDetail();
                detail.ProductID = item.ProductId;
                detail.Quantity = item.Quantity;
                detail.Price = (decimal)item.Product.PromotionPrice;
                orderDetails.Add(detail);
                isEnough = _productService.SellProduct(item.ProductId, item.Quantity);
            }
            if (isEnough)
            {
                var order = new Order();
                order.UpdateOrder(orderVM);
                var url = _vnPayService.CreatePaymentUrl(order, HttpContext);
                return Json(new { redirectUrl = url });
            }
            else
            {
                var code = "Không đủ hàng";
                foreach (var item in cart)
                {
                    if(!_productService.SellProduct(item.ProductId, item.Quantity))
                    {
                        if (item.Quantity >= 20)
                        {
                            code = "Không đủ hàng " + item.Product.Name.ToString()+" Liện hệ với chúng tôi để đặt hàng số lượng lớn";
                        }
                        else
                        {
                            code = "Không đủ hàng " + item.Product.Name.ToString();
                        }
                    }
                }
                return Json(new { code = code });
            }

        }

        public ActionResult PaymentCallback()
        {
            var response = _vnPayService.PaymentExecute(Request.QueryString);
            if (response.Success == false)
            {
                int code = 1;
                return RedirectToAction("ErrorVnPay", "ShoppingCart", new { code = code });
            }
            string orderViewModel = (string)Session[CommonConstants.orderViewModel];
            var order = new JavaScriptSerializer().Deserialize<OrderViewModel>(orderViewModel);
            var orderNew = new Order();

            orderNew.UpdateOrder(order);

            if (Request.IsAuthenticated)
            {
                orderNew.CustomerId = User.Identity.GetUserId();
                orderNew.CreatedBy = User.Identity.GetUserName();
            }
            orderNew.PaymentMethod = "VnPay";
            orderNew.Status = 1;
            orderNew.CustomerMessage = "Ma hoa don" + response.OrderId;
            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            int b = cart.Count();
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            bool isEnough = true;
            string returnUrl = ConfigHelper.GetByKey("ReturnUrl");
            foreach (var item in cart)
            {
                var detail = new OrderDetail();
                detail.ProductID = item.ProductId;
                detail.Quantity = item.Quantity;
                detail.Price = (decimal)item.Product.PromotionPrice;
                orderDetails.Add(detail);
                isEnough = _productService.SellProduct(item.ProductId, item.Quantity);
            }
            if (isEnough)
            {
                _orderService.Create(orderNew, orderDetails);    
                _productService.Save();
                return RedirectToAction("SuccessVnPay", "ShoppingCart");
            }
            else
            {
                orderNew.Status = 8;
                List<OrderDetail> orderDetailsBlank = new List<OrderDetail>();
                _orderService.Create(orderNew, orderDetailsBlank);
                _productService.Save();
                int code = 2;
                return RedirectToAction("ErrorVnPay", "ShoppingCart", new { code = code });
            }
        }
        public ActionResult SuccessVnPay()
        {
            Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            ViewBag.mgs = "Thanh toán thành công";
            return View();
        }
        public ActionResult ErrorVnPay(int code)
        {
            if (code == 1) {
                ViewBag.mgs = "Hủy thanh toán.";
            }
            else if (code == 2) {
                ViewBag.mgs = "Thanh toán thất bại. Không đủ hàng. Vui lòng liên hệ nhân viên để hoàn tiền";
            }
                return View();
        }

        [HttpPost]
        public JsonResult Promo(string promotion,float temp)
        {
            Promotion p = _promotionService.GetByCode(promotion);
            if (p != null && p.DateStart <= DateTime.Now && p.DateEnd >= DateTime.Now)
            {
                temp = temp * (p.DiscountPercent) / 100;
                return Json(new
                {
                    status = true,
                    promo = temp
                });
            }
            else
            {
                string message = "Mã không hợp lệ."; 

                if (p == null)
                {
                    message = "Mã không tồn tại.";
                }
                else if (p.DateStart > DateTime.Now)
                {
                    message = "Thời gian khuyến mãi chưa bắt đàu.";
                }
                else if (p.DateEnd < DateTime.Now)
                {
                    message = "Thời gian khuyến mãi đã kết thúc.";
                }

                return Json(new
                {
                    status = false,
                    promo = temp,
                    message = message
                });
            }
        }

        [HttpPost]
        public JsonResult MinusQuantity(string promotion)
        {
            Promotion p = _promotionService.GetByCode(promotion);
            p.Quantity--;
            _promotionService.Update(p);
            _promotionService.Save();
            return Json(new
            {
                status = true
            });
            
        }
    }
}