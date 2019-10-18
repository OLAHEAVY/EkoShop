using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EkoShop.Models
{
    public class Coupon
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string CouponType { get; set; }


        public enum ECouponType { Percent = 0, Dollar = 1 }

        [Required]
        public double Discount { get; set; }

        [Required]
        public double MinimumAmount { get; set; }

        //This is image will be stored on the server
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        public bool IsActive { get; set; }
    }
}
