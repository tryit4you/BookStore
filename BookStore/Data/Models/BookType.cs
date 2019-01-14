using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Models
{
    public class BookType
    {
        [Key]
        public string Id { get; set; }
        [DisplayName("Tên")]
        [Required]
        public string Name { get; set; }
        public string ThumbnailUrl { get; set; }
        [Required]
        [DisplayName("MetaName")]
        public string MetaName { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        public List<Book> Books { get; set; }
        public string CategoryId { get; set; }
        public bool Status { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}
