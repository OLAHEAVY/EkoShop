using EkoShop.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EkoShop.DataAccess.Data.Repository.IRepository
{
    public interface ICouponRepository : IBaseRepository<Coupon>
    {

        void Update(Coupon coupon);
    }
}
