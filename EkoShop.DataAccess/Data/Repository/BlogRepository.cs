using EkoShop.DataAccess.Data.Repository.IRepository;
using EkoShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EkoShop.DataAccess.Data.Repository
{
    public class BlogRepository : BaseRepository<Post>, IBlogReposistory
    {
        private readonly ApplicationDbContext _db;
        public BlogRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Post post)
        {
            var objFromDb = _db.Post.FirstOrDefault(c => c.Id == post.Id);

            objFromDb.Category = post.Category;
            objFromDb.Content = post.Content;
            objFromDb.ModifiedOn = DateTime.Now;
            objFromDb.Title = post.Title;

            _db.SaveChanges();
        }
    }
}
