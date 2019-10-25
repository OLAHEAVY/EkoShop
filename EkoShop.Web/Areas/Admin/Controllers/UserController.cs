using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EkoShop.DataAccess.Data.Repository.IRepository;
using EkoShop.Models;
using EkoShop.Models.ViewModels;
using EkoShop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EkoShop.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.ManagerUser)]
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserViewModel UserVM { get; set; }

        public UserController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            //getting the loggedin user
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var allTheUsers = _unitOfWork.User.GetAll(u => u.Id != claims.Value);
            
            var users = allTheUsers.Select(async u =>
            {
                UserVM = new UserViewModel();
                var roles = await _userManager.GetRolesAsync(u);
                UserVM.Email = u.Email;
                UserVM.Name = u.Name;
                UserVM.PhoneNumber = u.PhoneNumber;
                UserVM.LockoutEnd = u.LockoutEnd;
                UserVM.UserId = u.Id;
                UserVM.Role = string.Join(',', roles);
                return UserVM;
            });

            
            return View(users);


        }

        public IActionResult Lock(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _unitOfWork.User.LockUser(id);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Unlock(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _unitOfWork.User.UnlockUser(id);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}