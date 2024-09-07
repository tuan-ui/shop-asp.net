using Data;
using Data.Infrastructure;
using Data.Models;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface ICommentService
    {
        List<Comment> ListComment(int parentId, int productId);
        List<Comment> ListCommentViewModel(int parentId, int productId);
        List<Comment> ListCommentRateViewModel(int productId);
        List<Comment> ListCommentViewModelAll(int parentId, int productId);
        bool Insert(Comment comment);
        Comment Delete(int id);
        void Save();
    }
    public class CommentService : ICommentService
    {
        ShopDbContext db = new ShopDbContext();
        ICommentRepository _commentRepository;
        IUnitOfWork _unitOfWork;
        public CommentService(ICommentRepository commentRepository, IUnitOfWork unitOfWork)
        {
            this._commentRepository = commentRepository;
            this._unitOfWork = unitOfWork;
        }
        public bool Insert(Comment comment)
        {
            _commentRepository.Add(comment);
            _unitOfWork.Commit();
            return true;
        }
        public void Save()
        {
            _unitOfWork.Commit();
        }
        public List<Comment> ListComment(int parentId, int productId)
        {
            return db.Comments.Where(x => x.ParentID == parentId && x.ProductID == productId).ToList();
        }
        public List<Comment> ListCommentViewModel(int parentId, int productId)
        {
            return _commentRepository.ListCommentViewModel(parentId, productId);
        }
        public List<Comment> ListCommentRateViewModel(int productId)
        {
            return _commentRepository.ListCommentRateViewModel(productId);
        }
        public List<Comment> ListCommentViewModelAll(int parentId, int productId)
        {
            return _commentRepository.ListCommentViewModelAll(parentId,productId);
        }

        public Comment Delete(int id)
        {
            return _commentRepository.Delete(id); ;
        }
    }
}

