using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Models
{
    public class CourseCategory
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string MetaName { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Status { get; set; }
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
