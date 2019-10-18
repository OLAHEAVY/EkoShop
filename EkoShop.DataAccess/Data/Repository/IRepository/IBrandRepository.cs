using EkoShop.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace EkoShop.DataAccess.Data.Repository.IRepository
{
    public interface IBrandRepository:IBaseRepository<Brand>
    {

        IEnumerable<SelectListItem> GetBrandListForDropDown();

        void Update(Brand brand);
    }
}
