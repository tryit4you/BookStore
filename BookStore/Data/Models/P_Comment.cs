using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Models
{
    [Table("P_Comment")]
    public class P_Comment
    {
        [Key]
        public string p_id { get; set; }
        public string eBookId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UrlAvatar { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        [ForeignKey("eBookId")]
        public virtual Book Book { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
