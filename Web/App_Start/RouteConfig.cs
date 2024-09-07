using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*botdetect}", new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });
            routes.MapRoute(
                 name: "Contact",
                 url: "lien-he.html",
                 defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional },
                 namespaces: new string[] { "Web.Controllers" }
             );
                routes.MapRoute(
                  name: "Search",
                  url: "tim-kiem.html",
                  defaults: new { controller = "Product", action = "Search", id = UrlParameter.Optional },
                  namespaces: new string[] { "Web.Controllers" }
              );
            routes.MapRoute(
                    name: "build-Pc",
                    url: "build-Pc.html",
                    defaults: new { controller = "BuildPC", action = "Index", returnUrl = UrlParameter.Optional },
                    namespaces: new string[] { "Web.Controllers" }
                    );
            routes.MapRoute(
                     name: "Login",
                     url: "dang-nhap.html",
                     defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
                     namespaces: new string[] { "Web.Controllers" }
                 );
                    routes.MapRoute(
                      name: "Register",
                      url: "dang-ky.html",
                      defaults: new { controller = "Account", action = "Register", id = UrlParameter.Optional },
                      namespaces: new string[] { "Web.Controllers" }
                  );
                        routes.MapRoute(
                       name: "Page",
                       url: "trang/{alias}.html",
                       defaults: new { controller = "Page", action = "Index", alias = UrlParameter.Optional },
                       namespaces: new string[] { "Web.Controllers" }
                   );
                       routes.MapRoute(
                         name: "ProductFilter",
                         url: "product.filter.html",
                         defaults: new { controller = "Product", action = "FilterProducts", page = UrlParameter.Optional },
                         namespaces: new[] { "Web.Controllers" }
                        );
                     routes.MapRoute(
                         name: "Product Category",
                         url: "{alias}.pc-{id}.html",
                         defaults: new { controller = "Product", action = "Category", id = UrlParameter.Optional },
                           namespaces: new string[] { "Web.Controllers" }
                     );
                    routes.MapRoute(
                            name: "Supplier",
                            url: "{alias}.supp-{id}.html",
                            defaults: new { controller = "Product", action = "Supplier", id = UrlParameter.Optional },
                            namespaces: new string[] { "Web.Controllers" }
                     );
                        routes.MapRoute(
                            name: "Product",
                            url: "{alias}.p-{productId}.html",
                            defaults: new { controller = "Product", action = "Detail", productId = UrlParameter.Optional },
                            namespaces: new string[] { "Web.Controllers" }
                                 );
                        routes.MapRoute(
                            name: "Order",
                            url: "{alias}.p-{orId}.html",
                            defaults: new { controller = "Order", action = "Detail", orderId = UrlParameter.Optional },
                            namespaces: new string[] { "Web.Controllers" }
                      );
                        routes.MapRoute(
                         name: "TagList",
                         url: "tag/{tagId}.html",
                         defaults: new { controller = "Product", action = "ListByTag", tagId = UrlParameter.Optional },
                           namespaces: new string[] { "Web.Controllers" }
                     );
                         routes.MapRoute(
                           name: "Cart",
                           url: "gio-hang.html",
                           defaults: new { controller = "ShoppingCart", action = "Index", id = UrlParameter.Optional },
                           namespaces: new string[] { "Web.Controllers" }
                       );
                        routes.MapRoute(
                         name: "Checkout",
                         url: "thanh-toan.html",
                         defaults: new { controller = "ShoppingCart", action = "Index", id = UrlParameter.Optional },
                         namespaces: new string[] { "Web.Controllers" }
                     );
                    routes.MapRoute(
                         name: "Taikhoan",
                         url: "tai-khoan.html",
                         defaults: new { controller = "Account", action = "Profile", id = UrlParameter.Optional },
                         namespaces: new string[] { "Web.Controllers" }
                     );
            routes.MapRoute(
                        name: "Search Advance",
                        url: "{id}-{keyword}.html",
                        defaults: new { controller = "Product", action = "SearchAdvance", id = UrlParameter.Optional, keyword = UrlParameter.Optional },
                        namespaces: new string[] { "Web.Controllers" }
                        );
            routes.MapRoute(
                name: "Search Laptop",
                url: "{tag}.html",
                defaults: new { controller = "Product", action = "SearchLaptop", tag = UrlParameter.Optional },
                namespaces: new string[] { "Web.Controllers" }
                );
            routes.MapRoute(
                            name: "Default",
                            url: "{controller}/{action}/{id}",
                            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                              namespaces: new string[] { "Web.Controllers" }
                        );
                          
        }

    }
}
