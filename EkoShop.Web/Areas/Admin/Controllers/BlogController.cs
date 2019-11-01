using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EkoShop.DataAccess.Data.Repository.IRepository;
using EkoShop.Models;
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


        //Upsert Get Action Method
        public IActionResult Upsert(int? id)
        {
            Post post = new Post();

            if (id == null)
            {
                return View(post);
            }


            post = _unitOfWork.Blog.Get(id.GetValueOrDefault());
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Post post)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if(post.Id == 0)
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

                        post.Picture = p1;
                        post.CreatedOn = DateTime.Now;
                    }

                    _unitOfWork.Blog.Add(post);
                }
                else
                {
                    //editing the post
                    var postFromDb = _unitOfWork.Blog.Get(post.Id);
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

                        post.Picture = p1;
                    }
                    else
                    {
                        post.Picture = postFromDb.Picture;
                    }

                    _unitOfWork.Blog.Update(post);
                }

                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                
                return View(post);
            }
        }

        public IActionResult Details(int id)
        {
            var post = _unitOfWork.Blog.Get(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
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