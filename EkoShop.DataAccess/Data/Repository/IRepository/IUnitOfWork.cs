using System;
using System.Collections.Generic;
using System.Text;

namespace EkoShop.DataAccess.Data.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {

        ICategoryRepository Category { get; }

        IProductRepository Product { get; }

        IBrandRepository Brand { get; }

        ICouponRepository Coupon { get; }

        IUserRepository User { get; }

        IBlogReposistory Blog { get; }

        void Save();
    }
}
