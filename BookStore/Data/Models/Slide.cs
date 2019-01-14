using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Data.Models
{
    public class Slide
    {
        [Key]
        public string Id { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập tên slide")]
        [MaxLength(256)]
        [DisplayName("Tên slide")]
        public string Name { set; get; }

        [Required]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Yêu cầu phải có hình ảnh")]
        [DisplayName("Hình ảnh ")]
        public string Image { set; get; }

        [MaxLength(256)]
        [DisplayName("Đường dẫn")]
        public string Url { set; get; }

        [DisplayName("Thứ tự hiển thị")]
        public int? DisplayOrder { set; get; }

        [DisplayName("Trạng thái")]
        public bool Status { set; get; }

        [DisplayName("Nội dung")]
        public string Content { set; get; }

        public string Authors { get; set; }

        [DefaultValue("0")]
        public bool IsChoose { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}