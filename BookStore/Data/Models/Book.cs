using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Models
{
    public class Book
    {
        [Key]
        public string Id { get; set; } 
        [Required(ErrorMessage ="Yêu cầu nhập tên sách")]
        [DisplayName("Tên sách")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Yêu cầu phải có hình đại diện")]
        [DisplayName("Đường dẫn ảnh")]
        public string ThumbnailUrl { get; set; }
        public string FormatDownload { get; set; }
        [Required]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập miêu tả sách")]
        [DisplayName("Miêu tả sách")]
        public string LongDescription { get; set; }
        public string MetaName { get; set; }
        [DisplayName("Ngày tạo")]
        public DateTime CreatedDate { get; set; }
        [DisplayName("Sửa lần cuối")]
        public DateTime LastModifiedDate { get; set; }
        [DisplayName("Lượt tải")]
        [DefaultValue(1)]
        public int CountDownload { get; set; }
        [DisplayName("Lượt xem")]
        [DefaultValue(1)]
        public int views { get; set; }
        public string Language { get; set; }
        public int PageNumber { get; set; }
        public string Authors { get; set; }
        public string TextReference { get; set; }
        public string LinkReference { get; set; }
        public string DateRelease { get; set; }
        public string PublishBy { get; set; }
        public float Cappacity { get; set; }
        [DisplayName("Trạng thái")]
        [DefaultValue(true)]
        public bool Status { get; set; }
        public string CategoryId { get; set; }

        public  string BookTypeId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("BookTypeId")]
        public virtual BookType BookType { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        [ForeignKey("FormatDownload")]
        public virtual DownloadFormat DownloadFormats { get; set; }

    }
}
