using EkoShop.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EkoShop.DataAccess.Data.Repository.IRepository
{
    public interface IUserRepository : IBaseRepository<ApplicationUser>
    {
        void LockUser(string userId);

        void UnlockUser(string userId);

    }
}
