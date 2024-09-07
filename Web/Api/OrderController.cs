using AutoMapper;
using Data.Models;
using Service;
using Service.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Web.Common;
using Web.Infrastructure.Core;
using Web.Infrastructure.Extensions;
using Web.Models;

namespace Web.Api
{
    [RoutePrefix("api/order")]
    [Authorize]
    public class OrderController : ApiControllerBase
    {
        private IOrderService _orderService;
        private IProductService _productService;

        public OrderController(IErrorService errorService, IOrderService orderService, IProductService productService)
            : base(errorService)
        {
            this._orderService = orderService;
            this._productService = productService;
        }


        [Route("getallparents")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var model = _orderService.GetAll();

                var responseData = Mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            };
            return CreateHttpResponse(request, func);
        }
        [Route("getbyid/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _orderService.GetById(id);

                var responseData = Mapper.Map<Order, OrderViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 10)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _orderService.GetAll(keyword);

                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(query.AsEnumerable());

                var paginationSet = new PaginationSet<OrderViewModel>()
                {
                    Items = responseData,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };
                var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }

       

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage Update(HttpRequestMessage request, OrderViewModel orderVm)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var dbProduct = _orderService.GetById(orderVm.ID);

                    dbProduct.UpdateOrder(orderVm);
                    _orderService.Update(dbProduct);
                    _orderService.Save();

                    var responseData = Mapper.Map<Order, OrderViewModel>(dbProduct);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        

        [HttpGet]
        [Route("ExportPdf")]
        public async Task<HttpResponseMessage> ExportPdf(HttpRequestMessage request, int id)
        {
            string fileName = string.Concat("Product" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".pdf");
            var folderReport = ConfigHelper.GetByKey("ReportFolder");
            string filePath = HttpContext.Current.Server.MapPath(folderReport);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string fullPath = Path.Combine(filePath, fileName);
            try
            {
                var template = File.ReadAllText(HttpContext.Current.Server.MapPath("/Assets/admin/templete/order-detail.html"));
                var replaces = new Dictionary<string, string>();
                var order = _orderService.GetById(id);
                replaces.Add("{{CreatedDate}}", order.CreatedDate.HasValue ? order.CreatedDate.Value.ToString("dd/MM/yyyy") : "");
                replaces.Add("{{CustomerName}}", order.CustomerName);
                replaces.Add("{{CustomerAddress}}", order.CustomerAddress);
                replaces.Add("{{CustomerMobiphhone}}", order.CustomerMobile);

                var orderdetail = _orderService.GetListOrderDetail(order.ID);

                var orderDetailsHtml = new StringBuilder();
                foreach (var detail in orderdetail)
                {
                    orderDetailsHtml.Append("<tr>");
                    Product p = _productService.GetById(detail.ProductID);
                    orderDetailsHtml.AppendFormat("<td>{0}</td>", p.Name);
                    orderDetailsHtml.AppendFormat("<td>{0}</td>", detail.Quantity);
                    orderDetailsHtml.AppendFormat("<td>{0:C}</td>", detail.Price);
                    orderDetailsHtml.AppendFormat("<td>{0:C}</td>", detail.Quantity * detail.Price);
                    orderDetailsHtml.Append("</tr>");
                }

                // Thêm nội dung chi tiết đơn hàng vào replacements
                replaces.Add("{{OrderDetails}}", orderDetailsHtml.ToString());
                replaces.Add("{{TotalPrice}}", order.TotalPrice.ToString());

                template = template.Parse(replaces);

                await ReportHelper.GeneratePdf(template, fullPath);
                return request.CreateErrorResponse(HttpStatusCode.OK, Path.Combine(folderReport, fileName));
            }
            catch (Exception ex)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [HttpGet]
        [Route("ExportXls")]
        public async Task<HttpResponseMessage> ExportXls(HttpRequestMessage request, string filter = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            string fileName = string.Concat("Order_" + DateTime.Now.ToString("yyyyMMddhhmmsss") + ".xlsx");
            var folderReport = ConfigHelper.GetByKey("ReportFolder");
            string filePath = HttpContext.Current.Server.MapPath(folderReport);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string fullPath = Path.Combine(filePath, fileName);
            try
            {
                var data = _orderService.GetListOrder(filter, fromDate, toDate).ToList();
                var responseData = Mapper.Map< List<Order>, List< OrderReportViewModel>>(data);
                await ReportHelper.GenerateXlsOrder(responseData, fullPath, fromDate.Value, toDate.Value);
                return request.CreateErrorResponse(HttpStatusCode.OK, Path.Combine(folderReport, fileName));
            }
            catch (Exception ex)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Route("getbyusername")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetByUserName(HttpRequestMessage request,string username, int page, int pageSize = 10)
        {
            Func<HttpResponseMessage> func = () =>
            {
                int totalRow = 0;
                var model = _orderService.GetListOrderByUserNamePaging(username, page, pageSize, out totalRow);
                int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);
                var responseData = new
                {
                    items = Mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(model),
                    totalPages = totalPage
                };

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            };
            return CreateHttpResponse(request, func);
        }
        [Route("details")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage details(HttpRequestMessage request, int id)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var models = _orderService.GetListOrderDetail(id);

                var responseData = Mapper.Map<IEnumerable<OrderDetail>, IEnumerable<OrderDetailViewModel>>(models);
                foreach(var model in responseData)
                {
                    Product temp = _productService.GetById(model.ProductID);
                    model.ProductName = temp.Name;
                    if (temp.PromotionPrice != null) 
                        model.Price = temp.PromotionPrice.Value;
                }
                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            };
            return CreateHttpResponse(request, func);
        }
    }
}
