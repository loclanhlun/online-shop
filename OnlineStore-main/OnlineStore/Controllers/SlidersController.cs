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
using OnlineStore.Models.Sliders;

namespace OnlineStore.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SlidersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public SlidersController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        //[Microsoft.AspNetCore.Cors.EnableCors("CrossOrigin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Slider>>> Gets()
        {
            return await _context.Sliders.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Slider>> Get(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);

            if (slider == null)
            {
                return NotFound(new { message = "Không tìm thấy slider này. Thử lại." });
            }

            return Ok(slider);
        }

        [Authorize(Role.Admin, Role.Manager)]
        [HttpPut("{id}")]
        public IActionResult Put(int id, SliderRequest model)
        {
            var slider = GetSlider(id);
            if (slider == null)
            {
                return NotFound(new { message = "Không tìm thấy slider này. Thử lại." });
            }
            var name = slider.Name;
            var image = slider.Image;
            if (model.Image == null)
            {
                model.Image = image;
            }
            if (model.Name == null)
            {
                model.Name = name;
            }
            if (_context.Sliders.Any(x => x.Name == model.Name))
            {
                return NotFound(new { message = "Tên slider đã tồn tại. Thử lại." });
            }
            _mapper.Map(model, slider);
            _context.Sliders.Update(slider);
            _context.SaveChanges();
            return Ok(slider);
        }

        [Authorize(Role.Admin, Role.Manager)]
        [HttpPost]
        public async Task<ActionResult<Slider>> Post(SliderRequest model)
        {
            if (model.Image == null || model.Name == null)
            {
                return NotFound(new { message = "Vui lòng nhập đủ thông tin." });
            }
            if (_context.Sliders.Any(x => x.Name == model.Name))
            {
                return NotFound(new { message = "Tên slider đã tồn tại. Thử lại." });
            }
            var slider = _mapper.Map<Slider>(model);
            _context.Sliders.Add(slider);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Thêm slider thành công." });
        }

        [Authorize(Role.Admin, Role.Manager)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null)
            {
                return NotFound(new { message = "Không tìm thấy Id này. Hãy thử lại." });
            }

            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa thông tin thành công." });
        }
        private Slider GetSlider(int id)
        {
            var slider = _context.Sliders.Find(id);
            return slider;
        }
    }
}
