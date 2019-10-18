using EkoShop.DataAccess.Data.Repository.IRepository;
using EkoShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EkoShop.DataAccess.Data.Repository
{
    public class CouponRepository : BaseRepository<Coupon>, ICouponRepository
    {
        private readonly ApplicationDbContext _db;
        public CouponRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Coupon coupon)
        {
            var objFromDb = _db.Coupon.FirstOrDefault(c => c.Id == coupon.Id);

            objFromDb.Name = coupon.Name;
            objFromDb.IsActive = coupon.IsActive;
            objFromDb.MinimumAmount = coupon.MinimumAmount;
            objFromDb.Discount = coupon.Discount;
            objFromDb.CouponType = coupon.CouponType;
            objFromDb.ImageUrl = coupon.ImageUrl;

            _db.SaveChanges();
        }
    }
}
