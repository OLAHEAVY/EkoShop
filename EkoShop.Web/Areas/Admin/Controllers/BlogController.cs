using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EkoShop.DataAccess.Data.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EkoShop.Web.Areas.Admin.Controllers
{
   
    [Authorize]
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;


        public BlogController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Blog.GetAll() });

        }


        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Blog.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error While deleting." });
            }

            _unitOfWork.Blog.Remove(objFromDb);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successfull." });
        }


        #endregion
    }
}