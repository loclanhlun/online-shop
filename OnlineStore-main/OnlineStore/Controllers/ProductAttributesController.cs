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
using OnlineStore.Models.Products;

namespace OnlineStore.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductAttributesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;


        public ProductAttributesController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductAttribute>>> Gets()
        {
            return await _context.ProductAttributes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductAttribute>> Get(int id)
        {
            var productAttribute = await _context.ProductAttributes.FindAsync(id);

            if (productAttribute == null)
            {
                return NotFound(new { message = "Không tìm thấy màu sắc này. Thử lại." });
            }

            return Ok(productAttribute);
        }

        [Authorize(Role.Admin, Role.Manager)]
        [HttpPut("{id}")]
        public ActionResult<ProductAttribute> Put(int id, ProductAttributeRequest model)
        {
            var productAttribute = GetAttribute(id);
            if (productAttribute == null)
            {
                return NotFound(new { message = "Không tìm thấy màu sắc này. Thử lại." });
            }
            var name = productAttribute.ColorName;
            var description = productAttribute.Description;
            if (model.ColorName == null)
            {
                model.ColorName = name;
            }
            if (model.Description == null)
            {
                model.Description = description;
            }
            if (_context.ProductAttributes.Any(x => x.ColorName == model.ColorName))
            {
                return NotFound(new { message = "Tên màu sắc đã tồn tại. Thử lại." });
            }
            _mapper.Map(model, productAttribute);

            _context.ProductAttributes.Update(productAttribute);
            _context.SaveChanges();
            return Ok(productAttribute);
        }

        [Authorize(Role.Admin, Role.Manager)]
        [HttpPost]
        public async Task<ActionResult<ProductAttribute>> Post(ProductAttributeRequest model)
        {
            if ((model.ColorName == null) || (model.Description == null))
            {
                return NotFound(new { message = "Vui lòng điền đầy đủ thông tin." });
            }
            if (_context.ProductAttributes.Any(x => x.ColorName == model.ColorName))
            {
                return NotFound(new { message = "Tên màu sắc đã tồn tại. Thử lại." });
            }
            var productAttribute = _mapper.Map<ProductAttribute>(model);

            _context.ProductAttributes.Add(productAttribute);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Thêm màu sắc thành công." });
        }

        [Authorize(Role.Admin, Role.Manager)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var productAttribute = await _context.ProductAttributes.FindAsync(id);
                if (productAttribute == null)
                {
                    return NotFound(new { message = "Không tìm thấy Id này. Thử lại." });
                }

                _context.ProductAttributes.Remove(productAttribute);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Xóa thông tin thành công." });
            }

            catch (Exception)
            {
                return NotFound(new { message = "Lỗi liên kết khóa ngoại không thể xóa được." });
            }
        }

        private ProductAttribute GetAttribute(int id)
        {
            var productAttribute = _context.ProductAttributes.Find(id);
            return productAttribute;
        }
    }
}
