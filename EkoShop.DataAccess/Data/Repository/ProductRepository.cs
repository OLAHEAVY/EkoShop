using EkoShop.DataAccess.Data.Repository.IRepository;
using EkoShop.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EkoShop.DataAccess.Data.Repository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetProductListForDropDown()
        {
            return _db.Product.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
        }

        public void Update(Product product)
        {
            var objFromDb = _db.Product.FirstOrDefault(c => c.Id == product.Id);

            objFromDb.Name = product.Name;
            objFromDb.Picture = product.Picture;
            objFromDb.BrandId = product.BrandId;
            objFromDb.CategoryId = product.CategoryId;
            objFromDb.Description = product.Description;
            objFromDb.Price = product.Price;
            objFromDb.IsInStock = product.IsInStock;
           

            _db.SaveChanges();
        }
    }
}
