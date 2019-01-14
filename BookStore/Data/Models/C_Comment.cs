using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Models
{
    [Table("C_Comment")]
    public class C_Comment
    {
        [Key]
        public string c_id { get; set; }
        public string UserId { get; set; }

        public string UserName { get; set; }
        public string UrlAvatar { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public string p_id { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("p_id")]
        public virtual P_Comment P_Comment { get; set; }
    }
}
