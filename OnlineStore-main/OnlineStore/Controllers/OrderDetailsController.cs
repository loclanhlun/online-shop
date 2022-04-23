using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Authorization;
using OnlineStore.Entities;
using OnlineStore.Helpers;
using OnlineStore.Models.Orders;

namespace OnlineStore.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public OrderDetailsController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet("{orderid}")]
        public ActionResult<IEnumerable<OrderDetailResponse>> Get(int orderid)
        {
            try
            {
               
                var order = _context.Orders.Find(orderid);
                if (order == null)
                {
                    return NotFound(new { message = "Khong tim thay don hang nay." });
                }
                var currentUser = (User)HttpContext.Items["User"];
                if (order.UserId != currentUser.Id && currentUser.Role != Role.Admin)
                    return Unauthorized(new { message = "Unauthorized. Your order Id could not be found." });
                
                else
                {
                    var orderdetail = _context.OrderDetails.Where(x => x.OrderId == orderid).ToArray();
                    var listOrderDetail = new List<OrderDetailResponse>();
                    foreach (var item in orderdetail)
                    {
                        var orderdetailUser = _mapper.Map<OrderDetailResponse>(item);
                        orderdetailUser.productName = item.productName;
                        orderdetailUser.AddressShipping = item.AddressShipping;
                        orderdetailUser.Note = item.Note;
                        orderdetailUser.Status = item.Status;
                        orderdetailUser.OrderDate = item.OrderDate;
                        orderdetailUser.Price = item.Price;
                        orderdetailUser.Quantity = item.Quantity;
                        orderdetailUser.Total = item.Total;
                        orderdetailUser.PaymentStatus = item.PaymentStatus;
                        listOrderDetail.Add(orderdetailUser);
                    }
                    return Ok(listOrderDetail);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
