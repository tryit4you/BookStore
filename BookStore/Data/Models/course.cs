using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Models
{
    [Table("Course")]
    public class Course
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string CourseId { get; set; }
        public string MetaName { get; set; }
        public string AvatarData { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public string Authors { get; set; }
        public string Reference { get; set; }
        public string SharedUrl { get; set; }
        public int CountDownload { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("CourseId")]
        public virtual CourseCategory CourseCategory { get; set; }
    }
}
