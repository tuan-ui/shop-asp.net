using AutoMapper;
using Data.Models;
using Web.Models;

namespace Web.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Tag, TagViewModel>();
                cfg.CreateMap<ProductCategory, ProductCategoryViewModel>();
                cfg.CreateMap<Product, ProductViewModel>();
                cfg.CreateMap<ProductTag, ProductTagViewModel>();
                cfg.CreateMap<Search, SearchViewModel>();
                cfg.CreateMap<ProductSearch, ProductSearchViewModel>();
                cfg.CreateMap<Page, PageViewModel>();
                cfg.CreateMap<ContactDetail, ContactDetailViewModel>();
                cfg.CreateMap<Promotion, PromotionViewModel>();
                cfg.CreateMap<ApplicationGroup, ApplicationGroupViewModel>();
                cfg.CreateMap<ApplicationRole, ApplicationRoleViewModel>();
                cfg.CreateMap<ApplicationUser, ApplicationUserViewModel>();
                cfg.CreateMap<Comment, CommentViewModel>();
                cfg.CreateMap<Supplier, SupplierViewModel>();
                cfg.CreateMap<Order, OrderViewModel>();
                cfg.CreateMap<Order, OrderReportViewModel>();
                cfg.CreateMap<OrderDetail, OrderDetailViewModel>();
            });

        }

    }
}