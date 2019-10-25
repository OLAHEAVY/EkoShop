using System;
using System.Collections.Generic;
using System.Text;

namespace EkoShop.Models
{
    public class Post
    {
        public int Id { get; set; }

        public string Category { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public byte[] Picture { get; set; }
    }
}
