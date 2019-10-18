using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EkoShop.DataAccess.Data.Repository.IRepository;
using EkoShop.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace EkoShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CouponController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CouponController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Coupon coupon)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if(files.Count > 0)
                {
                    if(coupon.Id == 0)
                    {
                        //New Coupon
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(webRootPath, @"images\coupon");
                        var extension = Path.GetExtension(files[0].FileName);

                        using(var filestreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(filestreams);
                        }

                        coupon.ImageUrl = @"\images\coupon\" + fileName + extension;

                        _unitOfWork.Coupon.Add(coupon);

                    }
                }
                else
                {
                    //Edit Service
                    var couponFromDb = _unitOfWork.Coupon.Get(coupon.Id);
                    if(files.Count > 0)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(webRootPath, @"images\coupon");
                        var extension_new = Path.GetExtension(files[0].FileName);

                        var imagePath = Path.Combine(webRootPath, couponFromDb.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }

                        using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension_new), FileMode.Create))
                        {
                            files[0].CopyTo(fileStreams);
                        }

                        coupon.ImageUrl = @"\images\coupon\" + fileName + extension_new;
                    }
                    else
                    {
                        coupon.ImageUrl = couponFromDb.ImageUrl;
                    }

                    _unitOfWork.Coupon.Update(coupon);
                }

                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                //if model state is not valid
                return View(coupon);
            }

           
        }




        //method to update and create Category(Get Action Method)
        public IActionResult Upsert(int? id)
        {
            Coupon coupon = new Coupon();

            if(id == null)
            {

            }
            else
            {
                coupon = _unitOfWork.Coupon.GetFirstOrDefault(c => c.Id == id);
                if(coupon == null)
                {
                    return NotFound();
                }

            }

            return View(coupon);
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Coupon.GetAll() });

        }


        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Coupon.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error While deleting." });
            }

            _unitOfWork.Coupon.Remove(objFromDb);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successfull." });
        }


        #endregion
    }
}