using EkoShop.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EkoShop.DataAccess.Data.Repository.IRepository
{
    public interface IBlogReposistory : IBaseRepository<Post>
    {

        void Update(Post post);
    }
}
