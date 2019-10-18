using EkoShop.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace EkoShop.DataAccess.Data.Repository.IRepository
{
    public interface IProductRepository : IBaseRepository<Product>
    {

        IEnumerable<SelectListItem> GetProductListForDropDown();

        void Update(Product product);
    }
}
