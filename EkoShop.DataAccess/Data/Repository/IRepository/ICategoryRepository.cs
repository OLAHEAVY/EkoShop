using EkoShop.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace EkoShop.DataAccess.Data.Repository.IRepository
{
    public interface ICategoryRepository:IBaseRepository<Category>
    {

        IEnumerable<SelectListItem> GetCategoryListForDropDown();

        void Update(Category category);
    }
}
