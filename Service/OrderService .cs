using Data.Infrastructure;
using Data.Repositories;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IOrderService
    {
        bool Create(Order order, List<OrderDetail> orderDetails);
        void Update(Order order);
        IEnumerable<Order> GetAll();
        IEnumerable<Order> GetAll(string keyword);
        Order GetById(int id);
        void Save();
        IEnumerable<Order> Search(string keyword, int page, int pageSize, string sort, out int totalRow);
        IEnumerable<Order> GetListOrder(string keyword, DateTime? fromDate, DateTime? toDate);
        IEnumerable<OrderDetail> GetListOrderDetail(int keyword);
        IEnumerable<Order> GetListOrderByUserNamePaging(string username, int page, int pageSize, out int totalRow);
        IEnumerable<Order> GetListOrderByUserName(string username);
    }
    public class OrderService : IOrderService
    {
        IOrderRepository _orderRepository;
        IOrderDetailRepository _orderDetailRepository;
        IUnitOfWork _unitOfWork;
        IProductRepository _productRepository;

        public OrderService(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IUnitOfWork unitOfWork, IProductRepository productRepository)
        {
            this._orderRepository = orderRepository;
            this._orderDetailRepository = orderDetailRepository;
            this._unitOfWork = unitOfWork;
            this._productRepository = productRepository;
        }
        public bool Create(Order order, List<OrderDetail> orderDetails)
        {
            try
            {
                if (order.Status == 0)
                {
                    order.PaymentStatus = "Tạo hóa đơn";
                }
                else if (order.Status == 1)
                {
                    order.PaymentStatus = " Đã Thanh Toán";
                }
                else if (order.Status == 8)
                {
                    order.PaymentStatus = " Đã Thanh Toán - Lỗi - Hoàn tiền";
                }
                _orderRepository.Add(order);
                _unitOfWork.Commit();

                foreach (var orderDetail in orderDetails)
                {
                    orderDetail.OrderID = order.ID;
                    DateTime dayNow = DateTime.Now;
                    Product p = _productRepository.GetSingleById(orderDetail.ProductID);
                    orderDetail.ExpiredDate = dayNow.AddMonths(p.Warranty);
                    _orderDetailRepository.Add(orderDetail);
                }

                return true;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        public IEnumerable<Order> Search(string keyword, int page, int pageSize, string sort, out int totalRow)
        {
            var query = _orderRepository.GetMulti(x => x.CustomerName.Contains(keyword));

            switch (sort)
            {
                default:
                    query = query.OrderByDescending(x => x.CreatedDate);
                    break;
            }

            totalRow = query.Count();

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }
        public IEnumerable<Order> GetAll()
        {
            return _orderRepository.GetAll();
        }

        public IEnumerable<Order> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _orderRepository.GetMulti(x => x.CustomerName.Contains(keyword) || x.CustomerMessage.Contains(keyword));
            else
                return _orderRepository.GetAll();
        }
        public Order GetById(int id)
        {
            return _orderRepository.GetSingleById(id);
        }
        public void Save()
        {
            _unitOfWork.Commit();
        }
        public void Update(Order order)
        {
            if (order.Status == 0)
            {
                order.PaymentStatus = "Tạo hóa đơn - chưa thanh toán";
            }
            else if (order.Status == 1)
            {
                order.PaymentStatus = "Tạo hóa đơn - đã thanh toán";
            }
            else if (order.Status == 2)
            {
                order.PaymentStatus = "Vận chuyển - chưa thanh toán";
            }
            else if (order.Status == 3)
            {
                order.PaymentStatus = "Vận chuyển - đã thanh toán";
            }
            else if (order.Status == 4)
            {
                order.PaymentStatus = "Đã nhận hàng - chưa hoàn tiền";
            }
            else if (order.Status == 5)
            {
                order.PaymentStatus = "Đã nhận hàng - đã hoàn tiền";
            }
            else if (order.Status == 7)
            {
                order.PaymentStatus = "Đã Hủy";
            }
            else if (order.Status == 8)
            {
                order.PaymentStatus = "Đã Thanh Toán - Lỗi - Hoàn tiền";
            }
            _orderRepository.Update(order);
        }
        public IEnumerable<Order> GetListOrder(string keyword, DateTime? fromDate, DateTime? toDate)
        {
            IEnumerable<Order> query;
            if (!string.IsNullOrEmpty(keyword))
                query = _orderRepository.GetMulti(x => x.CustomerName.Contains(keyword));
            else
                query = _orderRepository.GetAll();
            if (fromDate.HasValue)
            {
                query = query.Where(x => x.CreatedDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(x => x.CreatedDate <= toDate.Value);
            }
            return query;
        }
        public IEnumerable<OrderDetail> GetListOrderDetail(int keyword)
        {
            return _orderDetailRepository.GetMulti(x => x.OrderID.Equals(keyword));
        }
        public IEnumerable<Order> GetListOrderByUserNamePaging(string username,int page, int pageSize, out int totalRow)
        {
            var query = _orderRepository.GetMulti(x => x.CreatedBy.Equals(username));
            totalRow = query.Count();

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }
        public IEnumerable<Order> GetListOrderByUserName(string username)
        {
            return _orderRepository.GetMulti(x => x.CreatedBy.Equals(username));
        }
    }
}
