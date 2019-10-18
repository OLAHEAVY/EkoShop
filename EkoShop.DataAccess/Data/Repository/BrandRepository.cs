using EkoShop.DataAccess.Data.Repository.IRepository;
using EkoShop.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EkoShop.DataAccess.Data.Repository
{
    public class BrandRepository:BaseRepository<Brand>, IBrandRepository
    {
        private readonly ApplicationDbContext _db;
        public BrandRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetBrandListForDropDown()
        {
            return _db.Brand.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
        }

        public void Update(Brand brand)
        {
            var objFromDb = _db.Brand.FirstOrDefault(c => c.Id == brand.Id);

            objFromDb.Name = brand.Name;
            objFromDb.DisplayOrder = brand.DisplayOrder;
            objFromDb.Type = brand.Type;

            _db.SaveChanges();
        }
    }
}
