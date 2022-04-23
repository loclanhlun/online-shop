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
    public class ProductBrandsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;


        public ProductBrandsController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductBrand>>> Gets()
        {
            return await _context.ProductBrands.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductBrand>> Get(int id)
        {
            var productBrand = await _context.ProductBrands.FindAsync(id);

            if (productBrand == null)
            {
                return NotFound(new { message = "Không tìm thấy danh mục này. Thử lại." });
            }

            return Ok(productBrand);
        }

        [Authorize(Role.Admin, Role.Manager)]
        [HttpPut("{id}")]
        public ActionResult<ProductBrand> Put(int id, ProductBrandRequest model)
        {
            var productBrand = GetBrand(id);
            if (productBrand == null)
            {
                return NotFound(new { message = "Không tìm thấy danh mục này. Thử lại." });
            }
            var name = productBrand.Name;
            var description = productBrand.Description;
            if (model.Name == null)
            {
                model.Name = name;
            }
            if (model.Description == null)
            {
                model.Description = description;
            }
            if (_context.ProductBrands.Any(x => x.Name == model.Name))
            {
                return NotFound(new { message = "Tên thương hiệu đã tồn tại. Thử lại." });
            }
            _mapper.Map(model, productBrand);

            _context.ProductBrands.Update(productBrand);
            _context.SaveChanges();
            return Ok(productBrand);
        }

        [Authorize(Role.Admin, Role.Manager)]
        [HttpPost]
        public async Task<ActionResult<ProductBrand>> Post(ProductBrandRequest model)
        {
            if ((model.Name == null) || (model.Description == null))
            {
                return NotFound(new { message = "Vui lòng điền đầy đủ thông tin." });
            }
            if (_context.ProductBrands.Any(x => x.Name == model.Name))
            {
                return NotFound(new { message = "Tên thương hiệu đã tồn tại. Thử lại." });
            }
            var productBrand = _mapper.Map<ProductBrand>(model);
            _context.ProductBrands.Add(productBrand);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Thêm danh mục sản phẩm thành công." });
        }

        [Authorize(Role.Admin, Role.Manager)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var productBrand = await _context.ProductBrands.FindAsync(id);
                if (productBrand == null)
                {
                    return NotFound(new { message = "Không tìm thấy Id này. Thử lại." });
                }

                _context.ProductBrands.Remove(productBrand);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Xóa thông tin thành công." });
            }
            catch (Exception)
            {
                return NotFound(new { message = "Lỗi liên kết khóa ngoại không thể xóa được." });
            }

        }

        private ProductBrand GetBrand(int id)
        {
            var productBrand = _context.ProductBrands.Find(id);
            return productBrand;
        }
    }
}
