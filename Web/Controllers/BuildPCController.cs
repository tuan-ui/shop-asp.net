using Data.Models;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class BuildPCController : Controller
    {
        IProductService _productService;
        IProductTagService _productTagService;
        IPercentComponentService _percentComponentService;
        public BuildPCController(IProductService productService, IProductTagService productTagService, IPercentComponentService percentComponentService)
        {
            this._productService = productService;
            this._productTagService = productTagService;
            this._percentComponentService = percentComponentService;
        }
        // GET: BuildPC
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetProducts(int cata)
        {
            var products = _productService.GetListProductByCategoryId(cata);
            return Json(products, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetCata(int[] productIds)
        {
            List<int> products = new List<int>();
            foreach(var productId in productIds)
            {
                var product = _productService.GetById(productId);
                products.Add(product.CategoryID);
            }
            
            return Json(products, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult CheckPC(int[] productIds)
        {
            List<Product> listProduct = new List<Product>();
            IEnumerable<ProductTag> mainTag = new List<ProductTag>();
            IEnumerable<ProductTag> cpuTag = new List<ProductTag>();
            IEnumerable<ProductTag> ramTag = new List<ProductTag>();
            IEnumerable<ProductTag> gpuTag = new List<ProductTag>();
            IEnumerable<ProductTag> psuTag = new List<ProductTag>();
            IEnumerable<ProductTag> caseTag = new List<ProductTag>();
            string Mgs = "";
            foreach (var productId in productIds)
            {
                Product p = _productService.GetById(productId);
                if (p.CategoryID == 7)
                    cpuTag = _productTagService.GetById(productId);
                if (p.CategoryID == 8)
                    mainTag = _productTagService.GetById(productId);
                if (p.CategoryID == 17)
                    ramTag = _productTagService.GetById(productId);
                if (p.CategoryID == 11)
                    gpuTag = _productTagService.GetById(productId);
                if (p.CategoryID == 13)
                    psuTag = _productTagService.GetById(productId);
                if (p.CategoryID == 12)
                    caseTag = _productTagService.GetById(productId);
            }
            //main vs ram
            if (ramTag.Count() != 0 && mainTag.Count() != 0)
            {
                if (compareTag(ramTag, mainTag))
                    Mgs += "<div class='text-success'>Ram bạn chọn phù hợp với main</div>";
                else
                    Mgs += "<div class='text-danger'>Ram bạn chọn không phù hợp với main</div>";
            }
            //main vs cpu
            if (cpuTag.Count() != 0 && mainTag.Count() != 0)
            {
                if (compareTag(cpuTag, mainTag))
                    Mgs += "<div class='text-success'>Cpu bạn chọn phù hợp với main</div>";
                else
                    Mgs += "<div class='text-danger'>Cpu bạn chọn không phù hợp với main</div>";
            }
            //main vs cpu
            if (caseTag.Count() != 0 && mainTag.Count() != 0)
            {
                if (compareTag(caseTag, mainTag))
                    Mgs += "<div class='text-success'>Case bạn chọn phù hợp với main</div>";
                else
                    Mgs += "<div class='text-danger'>Case bạn chọn không phù hợp với main</div>";
            }
            //case vs psu
            if (caseTag.Count() != 0 && psuTag.Count() != 0)
            {
                if (compareTag(caseTag, psuTag))
                    Mgs += "<div class='text-success'>Case bạn chọn phù hợp với nguồn</div>";
                else
                    Mgs += "<div class='text-danger'>Case bạn chọn không phù hợp với nguồn</div>";
            }
            //cpu vs gpu
            if (cpuTag.Count() != 0 && gpuTag.Count() == 0)
            {
                int count = 0;
                foreach (var cpu in cpuTag)
                    if (String.Compare(cpu.TagID, "gpu") == 0)
                        count++;
                if (count == 0)
                    Mgs += "<div class='text-danger'>Cpu của bạn chưa bao gồm card đồ họa. Vui lòng chọn thêm card</div>";
            }
            return Json(new
            {
                Mgs = Mgs
            }, JsonRequestBehavior.AllowGet);
        }
        public bool compareTag(IEnumerable<ProductTag> firstTag, IEnumerable<ProductTag> secondTag)
        {
            foreach (var first in firstTag)
                foreach (var second in secondTag)
                    if (String.Compare(first.TagID, second.TagID) == 0)
                        return true;
            return false;

        }
        [HttpGet]
        public JsonResult GetProductsTag(int[] productIds,int cata)
        {
            if (productIds==null||cata==14||cata==15||cata==10)
            {
                var products = _productService.GetListProductByCategoryId(cata);
                return Json(products, JsonRequestBehavior.AllowGet);
            }
            else {
                List<Product> products = new List<Product>();
                foreach (var productId in productIds)
                {
                    Product p = _productService.GetById(productId);
                    if (cata == 7&&p.CategoryID==8)
                    {
                        string str = p.Tags;
                        string[] array = str.Split(',');
                        if (array.Count() > 1)
                            products.AddRange(_productService.GetListProductByTag(array[0], cata));
                    }
                    if (cata == 8)
                    {
                        string str = p.Tags;
                        string[] array = str.Split(',');
                        if (array.Count() > 1)
                            products.AddRange(_productService.GetListProductByTag(array[0], cata));
                    }
                    if (cata == 17)
                    {
                        string str = p.Tags;
                        string[] array = str.Split(',');
                        if(array.Count()>1)
                            products.AddRange(_productService.GetListProductByTag(array[1], cata));
                    }
                    if (cata == 11)
                    {
                        products.AddRange(_productService.GetListProductByCategoryId(cata));
                        return Json(products, JsonRequestBehavior.AllowGet);
                    }
                    if (cata == 13&&p.CategoryID==11)
                    {
                        string str = p.Tags;
                        string[] array = str.Split(',');
                        int valueA = int.Parse(array[0].TrimEnd('W'));
                        var gpus=_productService.GetListProductByCategoryId(cata);
                        foreach(var gpu in gpus)
                        {
                            string[] arrayGpu = gpu.Tags.Split(',');
                            int valueB = int.Parse(arrayGpu[0].TrimEnd('W'));
                            if (valueA <= valueB)
                                products.Add(gpu);
                        }
                    }
                }
                return Json(products, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult FilterPurpose(string purpose, decimal selectedPrice)
        {
            IEnumerable<PercentComponent> list = _percentComponentService.GetAll();
            string[] mainTag = new string[10];
            string[] cpuTag = new string[10];
            string[] ramTag = new string[10];
            string[] gpuTag = new string[10];
            string[] psuTag = new string[10];
            string[] caseTag = new string[10];

            List<Product> listPurpose = new List<Product>();
            foreach (var percentComponent in list)
                if (string.Compare(percentComponent.Purpose, purpose) == 0)
                {
                    
                    decimal price = (selectedPrice*percentComponent.Price)*10000;
                    IEnumerable<Product> listpro = _productService.GetListProductByCategoryId(percentComponent.Component);
                    Product maxPromotionPriceProduct = listpro.Where(x => x.PromotionPrice < price)
                                                                .OrderByDescending(x => x.PromotionPrice)
                                                                .FirstOrDefault();
                    if (maxPromotionPriceProduct != null)
                    {
                        if (maxPromotionPriceProduct.CategoryID == 7)
                            if(String.Compare(purpose, "vanphong") != 0)
                                cpuTag = maxPromotionPriceProduct.Tags.Split(',');
                            else
                            {
                                while (true)
                                {
                                    cpuTag = maxPromotionPriceProduct.Tags.Split(',');

                                    if (String.Compare(cpuTag[1], "non-gpu") != 0)
                                        break;
                                    else
                                    {
                                        maxPromotionPriceProduct = listpro.Where(x => x.PromotionPrice < maxPromotionPriceProduct.PromotionPrice)
                                                                    .OrderByDescending(x => x.PromotionPrice)
                                                                    .FirstOrDefault();
                                        if (maxPromotionPriceProduct == null)
                                        {
                                            return Json(false, JsonRequestBehavior.AllowGet);
                                        }
                                    }
                                }
                            }
                        if (maxPromotionPriceProduct.CategoryID == 8)
                        {
                            while (true)
                            {
                                mainTag = maxPromotionPriceProduct.Tags.Split(',');

                                if (String.Compare(cpuTag[0], mainTag[0]) == 0)
                                    break;
                                else
                                {
                                    maxPromotionPriceProduct = listpro.Where(x => x.PromotionPrice < maxPromotionPriceProduct.PromotionPrice)
                                                                .OrderByDescending(x => x.PromotionPrice)
                                                                .FirstOrDefault();
                                    if (maxPromotionPriceProduct == null)
                                    {
                                        return Json(false, JsonRequestBehavior.AllowGet);
                                    }
                                }
                            }
                        }
                        if (maxPromotionPriceProduct.CategoryID == 17)
                        {
                            while (true)
                            {
                                ramTag = maxPromotionPriceProduct.Tags.Split(',');
                                if (String.Compare(ramTag[0], mainTag[1]) == 0)
                                    break;
                                else
                                {
                                    maxPromotionPriceProduct = listpro.Where(x => x.PromotionPrice < maxPromotionPriceProduct.PromotionPrice)
                                                                .OrderByDescending(x => x.PromotionPrice)
                                                                .FirstOrDefault();
                                    if (maxPromotionPriceProduct == null)
                                    {
                                        return Json(false, JsonRequestBehavior.AllowGet);
                                    }
                                }
                            }
                        }
                        if (maxPromotionPriceProduct.CategoryID == 11)
                            gpuTag = maxPromotionPriceProduct.Tags.Split(',');
                        if (maxPromotionPriceProduct.CategoryID == 13&& gpuTag[0]!=null) { 
                            while (true)
                            {
                                psuTag = maxPromotionPriceProduct.Tags.Split(',');
                                int valueA = int.Parse(psuTag[0].TrimEnd('W'));
                                int valueB = int.Parse(gpuTag[0].TrimEnd('W'));

                                if (valueB <= valueA)
                                    break;
                                else
                                {
                                    maxPromotionPriceProduct = listpro.Where(x => x.PromotionPrice < maxPromotionPriceProduct.PromotionPrice)
                                                                .OrderByDescending(x => x.PromotionPrice)
                                                                .FirstOrDefault();
                                    if (maxPromotionPriceProduct == null)
                                    {
                                        return Json(false, JsonRequestBehavior.AllowGet);
                                    }
                                }
                            }
                        }

                        if (maxPromotionPriceProduct.CategoryID == 12)
                            caseTag = maxPromotionPriceProduct.Tags.Split(',');
                        listPurpose.Add(maxPromotionPriceProduct);
                    }
                    
                }
            return Json(listPurpose, JsonRequestBehavior.AllowGet);
        }
    }
}