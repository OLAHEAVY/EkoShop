using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EkoShop.DataAccess.Data.Repository.IRepository;
using EkoShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EkoShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public ProductViewModel ProductVM { get; set; }

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        //Upsert Get Action Method
        public IActionResult Upsert(int? id)
        {
            ProductVM = new ProductViewModel()
            {
                Product = new EkoShop.Models.Product(),
                CategoryList = _unitOfWork.Category.GetCategoryListForDropDown(),
                BrandList = _unitOfWork.Brand.GetBrandListForDropDown()
            };

            //update view
            if(id!= null)
            {
                ProductVM.Product = _unitOfWork.Product.Get(id.GetValueOrDefault());
            }

            return View(ProductVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if(ProductVM.Product.Id == 0)
                {
                    //Creating the product
                    if (files.Count > 0)
                    {
                        byte[] p1 = null;
                        using (var fs1 = files[0].OpenReadStream())
                        {
                            using (var ms1 = new MemoryStream())
                            {
                                fs1.CopyTo(ms1);
                                p1 = ms1.ToArray();
                            }
                        }

                        ProductVM.Product.Picture = p1;
                    }

                    _unitOfWork.Product.Add(ProductVM.Product);
                }
                else
                {
                    //EDiting the products
                    var productFromDb = _unitOfWork.Product.Get(ProductVM.Product.Id);
                    if (files.Count > 0)
                    {
                        byte[] p1 = null;
                        using (var fs1 = files[0].OpenReadStream())
                        {
                            using (var ms1 = new MemoryStream())
                            {
                                fs1.CopyTo(ms1);
                                p1 = ms1.ToArray();
                            }
                        }

                        ProductVM.Product.Picture = p1;
                    }
                    else
                    {
                        ProductVM.Product.Picture = productFromDb.Picture;
                    }

                    _unitOfWork.Product.Update(ProductVM.Product);
                }

                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                //if the model state is not valid
                //populating the dropdownlist back
                ProductVM.BrandList = _unitOfWork.Brand.GetBrandListForDropDown();
                ProductVM.CategoryList = _unitOfWork.Category.GetCategoryListForDropDown();
                return View(ProductVM);
            }
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Product.GetAll(includePropperties: "Category,Brand") });

        }


        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Product.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error While deleting." });
            }

            _unitOfWork.Product.Remove(objFromDb);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successfull." });
        }


        #endregion
    }
}