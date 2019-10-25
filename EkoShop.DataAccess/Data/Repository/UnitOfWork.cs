using EkoShop.DataAccess.Data.Repository.IRepository;
using EkoShop.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EkoShop.DataAccess.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Brand = new BrandRepository(_db);
            Product = new ProductRepository(_db);
            Coupon = new CouponRepository(_db);
            User = new UserRepository(_db);
            Blog = new BlogRepository(_db);
        }
        public ICategoryRepository Category { get; private set; }

        public IBrandRepository Brand { get; private set; }

        public IProductRepository Product { get; private set; }

        public ICouponRepository Coupon { get; private set; }

        public IUserRepository User { get; private set; }

        public IBlogReposistory Blog { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }

   
}
