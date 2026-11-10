using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace de1_part2.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập name")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name phải từ 3 đến 100 ký tự")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập description")]
        [StringLength(500, ErrorMessage = "Description không được vượt quá 500 ký tự")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập price")]
        [Range(0.01, 100000, ErrorMessage = "Price phải từ 0.01 đến 10000.00")]
        public decimal Price { get; set; }
        //khóa ngoại
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
