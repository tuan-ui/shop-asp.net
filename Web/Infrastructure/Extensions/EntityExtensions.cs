using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Data.Models;
using Web.Models;

namespace Web.Infrastructure.Extensions
{
    public static class EntityExtensions
    {

        public static void UpdateProductCategory(this ProductCategory productCategory, ProductCategoryViewModel productCategoryVm)
        {
            productCategory.ID = productCategoryVm.ID;
            productCategory.Name = productCategoryVm.Name;
            productCategory.Description = productCategoryVm.Description;
            productCategory.Alias = productCategoryVm.Alias;
            productCategory.ParentID = productCategoryVm.ParentID;
            productCategory.DisplayOrder = productCategoryVm.DisplayOrder;
            productCategory.Searchs = productCategoryVm.Searchs;
            productCategory.CreatedDate = productCategoryVm.CreatedDate;
            productCategory.CreatedBy = productCategoryVm.CreatedBy;
            productCategory.UpdatedDate = productCategoryVm.UpdatedDate;
            productCategory.UpdatedBy = productCategoryVm.UpdatedBy;
            productCategory.MetaKeyword = productCategoryVm.MetaKeyword;
            productCategory.MetaDescription = productCategoryVm.MetaDescription;
            productCategory.Status = productCategoryVm.Status;

        }
        public static void UpdateProduct(this Product product, ProductViewModel productVm)
        {
            product.ID = productVm.ID;
            product.Name = productVm.Name;
            product.Description = productVm.Description;
            product.Alias = productVm.Alias;
            product.CategoryID = productVm.CategoryID;
            product.SupplierID = productVm.SupplierID;
            product.Content = productVm.Content;
            product.Image = productVm.Image;
            product.MoreImages = productVm.MoreImages;
            product.Quantity = productVm.Quantity;
            product.OriginalPrice = productVm.OriginalPrice;
            product.Price = productVm.Price;
            product.PromotionPrice = productVm.PromotionPrice;
            product.HotFlag = productVm.HotFlag;
            product.ViewCount = productVm.ViewCount;
            product.Warranty = productVm.Warranty;

            product.CreatedDate = productVm.CreatedDate;
            product.CreatedBy = productVm.CreatedBy;
            product.UpdatedDate = productVm.UpdatedDate;
            product.UpdatedBy = productVm.UpdatedBy;
            product.MetaKeyword = productVm.MetaKeyword;
            product.MetaDescription = productVm.MetaDescription;
            product.Status = productVm.Status;
            product.Tags = productVm.Tags;

        }
        public static void UpdateSupplier(this Supplier suppliers, SupplierViewModel suppliersVm)
        {
            suppliers.ID = suppliersVm.ID;
            suppliers.Name = suppliersVm.Name;
            suppliers.Description = suppliersVm.Description;
            suppliers.Alias = suppliersVm.Alias;
            suppliers.DisplayOrder = suppliersVm.DisplayOrder;

            suppliers.CreatedDate = suppliersVm.CreatedDate;
            suppliers.CreatedBy = suppliersVm.CreatedBy;
            suppliers.UpdatedDate = suppliersVm.UpdatedDate;
            suppliers.UpdatedBy = suppliersVm.UpdatedBy;
            suppliers.MetaKeyword = suppliersVm.MetaKeyword;
            suppliers.MetaDescription = suppliersVm.MetaDescription;
            suppliers.Status = suppliersVm.Status;

        }
        public static void UpdateFeedback(this Feedback feedback, FeedbackViewModel feedbackVm)
        {
            feedback.Name = feedbackVm.Name;
            feedback.Email = feedbackVm.Email;
            feedback.Message = feedbackVm.Message;
            feedback.Status = feedbackVm.Status;
            feedback.CreatedDate = DateTime.Now;
        }
        public static void UpdateOrder(this Order order, OrderViewModel orderVm)
        {
            order.CustomerName = orderVm.CustomerName;
            order.CustomerAddress = orderVm.CustomerAddress;
            order.CustomerEmail = orderVm.CustomerEmail;
            order.CustomerMobile = orderVm.CustomerMobile;
            order.CustomerMessage = orderVm.CustomerMessage;
            order.PaymentMethod = orderVm.PaymentMethod;
            order.PaymentStatus = orderVm.PaymentStatus;
            order.CreatedDate = DateTime.Now;
            order.CreatedBy = orderVm.CreatedBy;
            order.Status = orderVm.Status;
            order.CustomerId = orderVm.CustomerId;
            order.TotalPrice = orderVm.TotalPrice;
        }
        public static void UpdateApplicationGroup(this ApplicationGroup appGroup, ApplicationGroupViewModel appGroupViewModel)
        {
            appGroup.ID = appGroupViewModel.ID;
            appGroup.Name = appGroupViewModel.Name;
        }

        public static void UpdateApplicationRole(this ApplicationRole appRole, ApplicationRoleViewModel appRoleViewModel, string action = "add")
        {
            if (action == "update")
                appRole.Id = appRoleViewModel.Id;
            else
                appRole.Id = Guid.NewGuid().ToString();
            appRole.Name = appRoleViewModel.Name;
            appRole.Description = appRoleViewModel.Description;
        }
        public static void UpdateUser(this ApplicationUser appUser, ApplicationUserViewModel appUserViewModel, string action = "add")
        {

            appUser.Id = appUserViewModel.Id;
            appUser.FullName = appUserViewModel.FullName;
            appUser.BirthDay = appUserViewModel.BirthDay;
            appUser.Email = appUserViewModel.Email;
            appUser.UserName = appUserViewModel.UserName;
            appUser.PhoneNumber = appUserViewModel.PhoneNumber;
        }

        public static void UpdateComment(this Comment comment, CommentViewModel commentViewModel)
        {

            comment.ID = commentViewModel.ID;
            comment.CommentMsg = commentViewModel.CommentMsg;
            comment.CommentDate = commentViewModel.CommentDate;
            comment.ProductID = commentViewModel.ProductID;
            comment.UserID = commentViewModel.UserID;
            comment.ParentID = commentViewModel.ParentID;
            comment.Rate = commentViewModel.Rate;
        }
        public static void UpdatePromotion(this Promotion promotion, PromotionViewModel promotionyVm)
        {
            promotion.ID = promotionyVm.ID;
            promotion.Name = promotionyVm.Name;
            promotion.Code = promotionyVm.Code;
            promotion.Quantity = promotionyVm.Quantity;
            promotion.ProductID = promotionyVm.ProductID;
            promotion.DiscountPercent = promotionyVm.DiscountPercent;
            promotion.DateStart = promotionyVm.DateStart;
            promotion.DateEnd = promotionyVm.DateEnd;
            promotion.Status = promotionyVm.Status;

        }
    }
}