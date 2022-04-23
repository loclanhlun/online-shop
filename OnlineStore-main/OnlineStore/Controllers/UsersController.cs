using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Authorization;
using OnlineStore.Entities;
using OnlineStore.Helpers;
using OnlineStore.Models;
using OnlineStore.Models.Users;
using OnlineStore.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using BCryptNet = BCrypt.Net.BCrypt;

namespace OnlineStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private DataContext _context;
        private IMapper _mapper;

        public UsersController(IUserService userService, DataContext context, IMapper mappper)
        {
            _userService = userService;
            _context = context;
            _mapper = mappper;
        }

        [HttpPost("[action]")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);
            if (response == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });

            }
            return Ok(response);
        }

        [Authorize(Role.Manager)]
        [HttpGet]
        public IActionResult Gets()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            // only admins can access other user records
            var currentUser = (User)HttpContext.Items["User"];
            if (id != currentUser.Id && currentUser.Role != Role.Manager)
                return Unauthorized(new { message = "Unauthorized. Enter your Id" });

            //var user =  _userService.GetById(id);
            var user = getUser(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(user);
        }

        [HttpPut("editprofile/{id}")]
        public ActionResult<User> Put(int id, UpdateRequest model)
        {
            var currentUser = (User)HttpContext.Items["User"];
                if (id != currentUser.Id && currentUser.Role != Role.Admin)
                return Unauthorized(new { message = "Unauthorized. Enter your Id" });
            var user = getUser(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });

            }
            if (_context.Users.Any(x => x.Username == model.Username))
            {
                return NotFound(new { message = "User Name đã được sử dụng. Hãy thử lại." });
            }
            var username = user.Username;
            var address = user.Address;
            var birthdate = user.Birthdate;
            var avatar = user.Avatar;
            user.Birthdate.ToString("d-M-yyyy");

            //Verify whether date entered in dd/MM/yyyy format.

            if (model.Birthdate == DateTime.MinValue)
            {
                return NotFound(new { message = "Ngày sinh không hợp lệ. Phải nhập d-M-yyyy." });
            }
            if (model.Username == null)
            {
                model.Username = username;
            }
            if (model.Address == null)
            {
                model.Address = address;
            }
            if (model.Birthdate == null)
            {
                model.Birthdate = birthdate;
            }
            if (model.Avatar == null)
            {
                model.Avatar = avatar;
            }

            _mapper.Map(model, user);

            _context.Users.Update(user);
            _context.SaveChanges();
            var response = _mapper.Map<User>(user);
            return Ok(response);
        }
        [HttpPost("registerUser")]
        public async Task<ActionResult<User>> RegisterUser(RegisterRequest model)
        {

            if (model.Username == null || model.Email == null || model.Password == null)
            {
                return NotFound(new { message = "Điền đầy đủ thông tin." });
            }
            if (_context.Users.Any(x => x.Email == model.Email))
            {
                return NotFound(new { message = "Email đã tồn tại." });
            }
            if (_context.Users.Any(x => x.Username == model.Username))
            {
                return NotFound(new { message = "User Name đã tồn tại." });
            }
            var user = _mapper.Map<User>(model);
            user.PasswordHash = BCryptNet.HashPassword(model.Password);
            user.Role = Role.User;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đăng ký user thành công." });

        }
        [Authorize(Role.Manager)]
        [HttpPost("registerAdmin")]
        public async Task<ActionResult<User>> RegisterAdmin(RegisterRequest model)
        {

            if (model.Username == null || model.Email == null || model.Password == null)
            {
                return NotFound(new { message = "Điền đầy đủ thông tin." });
            }
            if (_context.Users.Any(x => x.Email == model.Email))
            {
                return NotFound(new { message = "Email đã tồn tại." });
            }
            if (_context.Users.Any(x => x.Username == model.Username))
            {
                return NotFound(new { message = "User Name đã tồn tại." });
            }
            var user = _mapper.Map<User>(model);
            user.PasswordHash = BCryptNet.HashPassword(model.Password);
            user.Role = Role.Admin;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đăng ký thành công." });

        }
        [Authorize(Role.Manager)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Xóa thành công." });

            }
            catch (Exception)
            {
                return NotFound(new { message = "Lỗi liên kết khóa ngoại không thể xóa được." });
            }
        }
        private User getUser(int id)
        {
            var user = _context.Users.Find(id);
            return user;
        }
    }
}
