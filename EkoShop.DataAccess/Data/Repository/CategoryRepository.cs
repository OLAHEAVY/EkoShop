using EkoShop.DataAccess.Data.Repository.IRepository;
using EkoShop.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EkoShop.DataAccess.Data.Repository
{
    public class CategoryRepository:BaseRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetCategoryListForDropDown()
        {
            return _db.Category.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
        }

        public void Update(Category category)
        {
            var objFromDb = _db.Category.FirstOrDefault(c => c.Id == category.Id);

            objFromDb.Name = category.Name;
            objFromDb.DisplayOrder = category.DisplayOrder;

            _db.SaveChanges();
        }
    }
}
