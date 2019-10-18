using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EkoShop.Models
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Brand Name")]
        [Required]
        public string Name { get; set; }

        public string Type { get; set; }


        [Required]
        [DisplayName("Display Order")]
        public int DisplayOrder { get; set; }

    }
}
