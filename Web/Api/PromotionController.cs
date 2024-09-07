using AutoMapper;
using Data.Models;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using Web.Infrastructure.Core;
using Web.Infrastructure.Extensions;
using Web.Models;

namespace Web.Api
{
    [RoutePrefix("api/promotion")]
    [Authorize]
    public class PromotionController : ApiControllerBase
    {
        private IProductService _productService;
        private IPromotionService _promotionService;

        public PromotionController(IErrorService errorService, IProductService productService, IPromotionService promotionService)
            : base(errorService)
        {
            this._productService = productService;
            this._promotionService = promotionService;
        }

        [Route("getallparents")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var model = _promotionService.GetAll();

                var responseData = Mapper.Map<IEnumerable<Promotion>, IEnumerable<PromotionViewModel>>(model);

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
                var model = _promotionService.GetById(id);

                var responseData = Mapper.Map<Promotion, PromotionViewModel>(model);

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
                var model = _promotionService.GetAll(keyword);

                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.DateStart).Skip(page * pageSize).Take(pageSize);
                foreach (var promotion in query)
                {
                    if (promotion.ProductID.HasValue)
                    {
                        promotion.Product = _productService.GetById(promotion.ProductID.Value);
                    }
                }
                var responseData = Mapper.Map<IEnumerable<Promotion>, IEnumerable<PromotionViewModel>>(query.AsEnumerable());

                var paginationSet = new PaginationSet<PromotionViewModel>()
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


        [Route("create")]
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request, PromotionViewModel promotionVm)
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
                    var newPromotion = new Promotion();
                    newPromotion.UpdatePromotion(promotionVm);
                    _promotionService.Add(newPromotion);
                    _promotionService.Save();

                    var responseData = Mapper.Map<Promotion, PromotionViewModel>(newPromotion);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage Update(HttpRequestMessage request, PromotionViewModel promotionVm)
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
                    var dbProduct = _promotionService.GetById(promotionVm.ID);

                    dbProduct.UpdatePromotion(promotionVm);
                    _promotionService.Update(dbProduct);
                    _promotionService.Save();

                    var responseData = Mapper.Map<Promotion, PromotionViewModel>(dbProduct);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
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
                    var oldProductCategory = _promotionService.Delete(id);
                    _promotionService.Save();

                    var responseData = Mapper.Map<Promotion, PromotionViewModel>(oldProductCategory);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }
        [Route("deletemulti")]
        [HttpDelete]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedpromotion)
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
                    var listProductCategory = new JavaScriptSerializer().Deserialize<List<int>>(checkedpromotion);
                    foreach (var item in listProductCategory)
                    {
                        _promotionService.Delete(item);
                    }

                    _promotionService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listProductCategory.Count);
                }

                return response;
            });
        }
    }
}
