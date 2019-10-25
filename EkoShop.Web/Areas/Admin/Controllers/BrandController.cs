using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EkoShop.DataAccess.Data.Repository.IRepository;
using EkoShop.Models;
using EkoShop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EkoShop.Web.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public BrandController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        //Upsert get action Method
        public IActionResult Upsert (int? id)
        {
            //Create view 
            Brand brand = new Brand();
            if(id == null)
            {
                return View(brand);
            }

            brand = _unitOfWork.Brand.Get(id.GetValueOrDefault());
            if(brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Brand brand)
        {
            if (ModelState.IsValid)
            {
                if(brand.Id == 0)
                {
                    _unitOfWork.Brand.Add(brand);
                }
                else
                {
                    _unitOfWork.Brand.Update(brand);
                }

                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }

            return View(brand);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Brand.GetAll() });

        }


        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Brand.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error While deleting." });
            }

            _unitOfWork.Brand.Remove(objFromDb);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successfull." });
        }


        #endregion
    }
}