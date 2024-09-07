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
    [RoutePrefix("api/suppliers")]
    [Authorize]
    public class SupplierController : ApiControllerBase
    {
        ISupplierService _supplierService;

        public SupplierController(IErrorService errorService, ISupplierService supplierService)
            : base(errorService)
        {
            this._supplierService = supplierService;
        }

        [Route("getbyid/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _supplierService.GetById(id);

                var responseData = Mapper.Map<Supplier, SupplierViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }

        [Route("getallparents")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var model = _supplierService.GetAll();

                var responseData = Mapper.Map<IEnumerable<Supplier>, IEnumerable<SupplierViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            };
            return CreateHttpResponse(request, func);
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 10)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _supplierService.GetAll(keyword);

                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<Supplier>, IEnumerable<SupplierViewModel>>(query);

                var paginationSet = new PaginationSet<SupplierViewModel>()
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
        [AllowAnonymous]
        public HttpResponseMessage Create(HttpRequestMessage request, SupplierViewModel supplierVm)
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
                    var newSupplier = new Supplier();
                    newSupplier.UpdateSupplier(supplierVm);
                    newSupplier.CreatedBy = User.Identity.Name;
                    _supplierService.Add(newSupplier);
                    _supplierService.Save();

                    var responseData = Mapper.Map<Supplier, SupplierViewModel>(newSupplier);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }
        [Route("update")]
        [HttpPut]
        [AllowAnonymous]
        public HttpResponseMessage Update(HttpRequestMessage request, SupplierViewModel supplierVm)
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
                    var dbSupplier = _supplierService.GetById(supplierVm.ID);

                    dbSupplier.UpdateSupplier(supplierVm);
                    dbSupplier.UpdatedDate = DateTime.Now;
                    dbSupplier.CreatedBy = User.Identity.Name;
                    _supplierService.Update(dbSupplier);
                    _supplierService.Save();
                    var responseData = Mapper.Map<Supplier, SupplierViewModel>(dbSupplier);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }
        [Route("delete")]
        [HttpDelete]
        [AllowAnonymous]
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
                    var oldSupplier = _supplierService.Delete(id);
                    _supplierService.Save();

                    var responseData = Mapper.Map<Supplier, SupplierViewModel>(oldSupplier);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }
        [Route("deletemulti")]
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedSuppliers)
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
                    var listSupplier = new JavaScriptSerializer().Deserialize<List<int>>(checkedSuppliers);
                    foreach (var item in listSupplier)
                    {
                        _supplierService.Delete(item);
                    }

                    _supplierService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listSupplier.Count);
                }

                return response;
            });
        }
    }
}
