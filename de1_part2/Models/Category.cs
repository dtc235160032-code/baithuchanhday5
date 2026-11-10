using System.ComponentModel.DataAnnotations;

namespace de1_part2.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Bạn phải nhập name")]
        [StringLength(100, ErrorMessage = "Name không được vượt quá 100 ký tự")]
        public string? Name { get; set; }
        [StringLength(500, ErrorMessage ="Discription không vượt quá 500 ký tự")]
        public string? Description { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
