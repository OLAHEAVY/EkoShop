using EkoShop.DataAccess.Data.Repository.IRepository;
using EkoShop.Models;
using EkoShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EkoShop.DataAccess.Data.Repository
{
    public class UserRepository : BaseRepository<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void LockUser(string userId)
        {
            //retieve user from the database
            var userFromDb = _db.ApplicationUser.FirstOrDefault(u => u.Id == userId);
            userFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
        }

        public void UnlockUser(string userId)
        {
            //retieve user from the database
            var userFromDb = _db.ApplicationUser.FirstOrDefault(u => u.Id == userId);
            userFromDb.LockoutEnd = DateTime.Now;
        }

       

    }
}
