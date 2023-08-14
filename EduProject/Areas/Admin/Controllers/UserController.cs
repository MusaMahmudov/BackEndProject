using AutoMapper;
using EduProject.Areas.Admin.ViewModels.UserViewModels;
using EduProject.Contexts;
using EduProject.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EduProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        public UserController(AppDbContext context,UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager,IMapper mapper)
        {
            _mapper = mapper;
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {

            List<AppUser> users = await _context.Users.AsNoTracking().ToListAsync();
            List<UserViewModel> userViewModel = _mapper.Map<List<UserViewModel>>(users);
            return View(userViewModel);
        }
        public async Task<IActionResult> Detail(string Id)
        {
            var User =await _context.Users.AsNoTracking().FirstOrDefaultAsync(u=>u.Id == Id);
            if (User is null)
                return NotFound();
            
            var UserRole = await _context.UserRoles.AsNoTracking().FirstOrDefaultAsync(ur=>ur.UserId == User.Id);
            if (UserRole is null)
                return NotFound();
            IdentityRole role = await _context.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == UserRole.RoleId);
            if (role is null)
                return NotFound();


            DetailUserViewModel detailUserViewModel = new DetailUserViewModel()
            {
                UserName = User.UserName,
                Email = User.Email,
                Fullname = User.Fullname,
                Role = role.Name,
                IsActive = User.IsActive
            };
            return View(detailUserViewModel);

        }
        public async Task<IActionResult> ChangeRole(string Id)
        {
            ViewBag.Roles = new SelectList(await _context.Roles.ToListAsync(),"Id","Name");
           

            var User = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == Id);
            if (User is null)
                return NotFound();

            ChangeUserViewModel changeUserViewModel = _mapper.Map<ChangeUserViewModel>(User);
            return View(changeUserViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRole(ChangeUserViewModel changeUserViewModel,string Id)
        {
            ViewBag.Roles = new SelectList(await _context.Roles.ToListAsync(), "Id", "Name");

            if (changeUserViewModel is null) return BadRequest();

            var userRole = await _context.UserRoles.FirstOrDefaultAsync(ur=>ur.UserId == Id);

            if(userRole is null)
            {
                return NotFound();
            }
            _context.UserRoles.Remove(userRole);
           await _context.UserRoles.AddAsync(new IdentityUserRole<string> { UserId = userRole.UserId,RoleId = changeUserViewModel.RoleId});

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ChangeStatus(string Id)
        {
             
            var User = await _context.Users.FirstOrDefaultAsync(u => u.Id == Id);
            if (User is null) 
                return NotFound();
             
            StatusUserViewModel statusUserViewModel = _mapper.Map<StatusUserViewModel>(User);
           

            return View(statusUserViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(StatusUserViewModel statusUserViewModel,string Id)
        {
            var User = await _context.Users.FirstOrDefaultAsync(u => u.Id == Id);
            if (User is null)
                return NotFound();
            
            if(!User.IsActive)
            {
                User.IsActive = true;
            }
            else
            {
                User.IsActive = false;
            }


            _context.Users.Update(User);
          await  _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        
    }
}
