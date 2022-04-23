using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Authorization;
using OnlineStore.Entities;
using OnlineStore.Helpers;
using OnlineStore.Models.Orders;

namespace OnlineStore.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public OrdersController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [Authorize(Role.Admin, Role.Manager)]
        [HttpGet]
        public ActionResult<IEnumerable<OrderResponse>> Gets()
        {
            var allorder = new List<OrderResponse>();
            var orders = _context.Orders.ToArray(); ;
            foreach (var order in orders)
            {
                var user = GetUser(order.UserId);
                order.user = user;
                var response = _mapper.Map<OrderResponse>(order);
                response.Id = order.Id;
                response.CustomerId = order.UserId;
                response.CustomerName = order.user.Username;
                response.AddressShipping = order.AddressShipping;
                response.OrderDate = order.OrderDate;
                response.OrderStatus = order.OrderStatus;
                response.Note = order.Note;

                allorder.Add(response);
            }
            return Ok(allorder);
        }

      
        [Authorize]
        [HttpGet("user")]
        public ActionResult<IEnumerable<OrderResponse>> GetOrderUser()
        {
            var currentUser = (User)HttpContext.Items["User"];
            var userid = currentUser.Id;
            var user = GetUser(userid);
            var allorder = new List<OrderResponse>();
            var orders = _context.Orders.Where(x => x.UserId == userid).ToArray();
            if (user == null)
            {
                return NotFound(new { message = "Khong ton tai user nay." });
            }
            foreach (var item in orders)
            {
                item.user = user;
                var response = _mapper.Map<OrderResponse>(item);
                response.Id = item.Id;
                response.CustomerId = item.UserId;
                response.CustomerName = item.user.Username;
                response.AddressShipping = item.AddressShipping;
                response.OrderDate = item.OrderDate;
                response.OrderStatus = item.OrderStatus;
                response.Note = item.Note;

                allorder.Add(response);
            }
            return Ok(allorder);
        }

        [Authorize(Role.Admin, Role.Manager)]
        [HttpPut("{id}")]
        public ActionResult<OrderResponse> Put(int id, OrderUpdate model)
        {

            var order = GetOrder(id);
            if (order == null)
            {
                return NotFound(new { message = "Khong tim thay id nay." });
            }
            var status = order.OrderStatus;
            var userid = order.UserId;

            if (model.OrderStatus == null)
            {
                model.OrderStatus = status;
            }
            var user = GetUser(userid);
            _mapper.Map(model, order);
            order.user = user;
            _context.Orders.Update(order);
            _context.SaveChanges();

            var response = _mapper.Map<OrderResponse>(order);
            response.Id = order.Id;
            response.CustomerId = order.UserId;
            response.CustomerName = order.user.Username;
            response.AddressShipping = order.AddressShipping;
            response.OrderDate = order.OrderDate;
            response.OrderStatus = order.OrderStatus;
            response.Note = order.Note;
            return Ok(response);
        }
        [Authorize]
        [HttpPost]
        public ActionResult<OrderResponse> Post(OrderRequest model)
        {
            var currentUser = (User)HttpContext.Items["User"];
            var userid = currentUser.Id;
            var user = GetUser(userid);
            var cart = GetCart(userid);
            if(model.AddressShipping == null)
            {
                return NotFound(new { message = "Địa chỉ bắt buộc phải nhập." });
            }

            if (cart == null)
            {
                return NotFound(new { message = "Giỏ hàng của bạn đang trống. Không thể tiến hành đặt hàng." });
            }
            var order = _mapper.Map<Order>(model);
            order.user = user;
            order.UserId = user.Id;
            order.OrderDate = DateTime.Now;
            order.OrderStatus = "Đơn hàng đang chờ xác nhận";
            order.Note = model.Note;
            order.AddressShipping = model.AddressShipping;
            _context.Orders.Add(order);
            _context.SaveChanges();
            PostOrderDetail(order.Id);
            var response = _mapper.Map<OrderResponse>(order);
            response.Id = order.Id;
            response.CustomerId = order.UserId;
            response.CustomerName = order.user.Username;
            response.AddressShipping = order.AddressShipping;
            response.OrderDate = order.OrderDate;
            response.OrderStatus = order.OrderStatus;
            response.Note = order.Note;
            return Ok(response);

        }

        private void PostOrderDetail(int orderid)
        {
            var allorderdetail = new List<OrderDetail>();
            var order = _context.Orders.Where(x => x.Id == orderid).FirstOrDefault();
            var carts = _context.Carts.Where(x => x.userId == order.UserId).ToArray();

            foreach (var item in carts)
            {
                var product = GetProduct(item.productId);
                var user = GetUser(item.userId);
                var orderdetail = new OrderDetail();
                orderdetail.OrderId = orderid;
                orderdetail.order = order;
                orderdetail.productName = item.product.Name;
                orderdetail.AddressShipping = order.AddressShipping;
                orderdetail.Note = order.Note;
                orderdetail.Status = order.OrderStatus;
                orderdetail.OrderDate = order.OrderDate;
                orderdetail.Quantity = item.Quantity;
                orderdetail.Price = item.product.Price;
                orderdetail.PaymentStatus = "Thanh toan truc tiep khi nhan hang";

                allorderdetail.Add(orderdetail);
            }
            for (int i = 0; i < allorderdetail.Count; i++)
            {
                for (int j = i + 1; j < allorderdetail.Count; j++)
                {
                    if (allorderdetail[i].productName == allorderdetail[j].productName)
                    {
                        allorderdetail[i].Quantity = allorderdetail[i].Quantity + allorderdetail[j].Quantity;
                        allorderdetail.RemoveAt(j);

                    }

                }
                allorderdetail[i].Total = allorderdetail[i].Price * allorderdetail[i].Quantity;
            }
            _context.OrderDetails.AddRange(allorderdetail);
            _context.Carts.RemoveRange(carts);
            _context.SaveChanges();
        }

        //[Authorize(Role.Admin)]
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    try
        //    {
        //        var order = await _context.Orders.FindAsync(id);
        //        if (order == null)
        //        {
        //            return NotFound(new { message = "Không tìm thấy Id này." });

        //        }

        //        _context.Orders.Remove(order);
        //        await _context.SaveChangesAsync();

        //        return Ok(new { message = "Xóa thông tin thành công." });
        //    }
        //    catch (Exception)
        //    {
        //        return NotFound(new { message = "Lỗi liên kết khóa ngoại không thể xóa được." });
        //    }

        //}

        private Order GetOrder(int id)
        {
            var order = _context.Orders.Where(x => x.UserId == id).FirstOrDefault();
            return order;
        }
        private Product GetProduct(int id)
        {
            var product = _context.Products.Find(id);
            return product;
        }
        private User GetUser(int id)
        {
            var user = _context.Users.Find(id);
            return user;
        }
        private Cart GetCart(int id)
        {
            var cart = _context.Carts.Where(x=>x.userId==id).FirstOrDefault();
            return cart;
        }

    }
}