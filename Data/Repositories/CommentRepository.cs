using Data.Infrastructure;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Data.Repositories
{

    public interface ICommentRepository : IRepository<Comment>
    {
        List<Comment> ListCommentViewModel(int parentId, int productId);
        List<Comment> ListCommentRateViewModel(int productId);
        List<Comment> ListCommentViewModelAll(int parentId, int productId);
    }
    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {

        public CommentRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
        public List<Comment> ListCommentViewModel(int parentId, int productId)
        {
            var model = (from a in DbContext.Comments
                         join b in DbContext.Users
                             on a.UserID equals b.Id
                         where a.ParentID == parentId && a.ProductID == productId

                         select new
                         {
                             ID = a.ID,
                             CommentMsg = a.CommentMsg,
                             CommentDate = a.CommentDate,
                             ProductID = a.ProductID,
                             UserID = a.UserID,
                             ParentID = a.ParentID,
                             Rate = a.Rate,
                             User = b
                         }).AsEnumerable().Select(x => new Comment()
                         {
                             ID = x.ID,
                             CommentMsg = x.CommentMsg,
                             CommentDate = x.CommentDate,
                             ProductID = x.ProductID,
                             UserID = x.UserID,
                             User = x.User,
                             ParentID = x.ParentID,
                             Rate = x.Rate
                         });
            return model.OrderByDescending(y => y.ID).Take(3).ToList();
        }
        public List<Comment> ListCommentRateViewModel(int productId)
        {
            var model = (from a in DbContext.Comments
                         where a.ParentID == 0 && a.ProductID == productId

                         select new
                         {
                             ID = a.ID,
                             Rate = a.Rate
                         }).AsEnumerable().Select(x => new Comment()
                         {
                             ID = x.ID,
                             CommentMsg = "",
                             CommentDate = DateTime.Now,
                             ProductID = productId,
                             UserID = "",
                             User = new ApplicationUser(),
                             ParentID = 0,
                             Rate = x.Rate
                         });
            return model.OrderByDescending(y => y.ID).ToList();
        }
        public List<Comment> ListCommentViewModelAll(int parentId, int productId)
        {
            var model = (from a in DbContext.Comments
                         join b in DbContext.Users
                             on a.UserID equals b.Id
                         where a.ParentID == parentId && a.ProductID == productId

                         select new
                         {
                             ID = a.ID,
                             CommentMsg = a.CommentMsg,
                             CommentDate = a.CommentDate,
                             ProductID = a.ProductID,
                             UserID = a.UserID,
                             User = b,
                             ParentID = a.ParentID,
                             Rate = a.Rate
                         }).AsEnumerable().Select(x => new Comment()
                         {
                             ID = x.ID,
                             CommentMsg = x.CommentMsg,
                             CommentDate = x.CommentDate,
                             ProductID = x.ProductID,
                             UserID = x.UserID,
                             User = x.User,
                             ParentID = x.ParentID,
                             Rate = x.Rate
                         });
            return model.OrderByDescending(y => y.ID).ToList();
        }
    }
        
}
