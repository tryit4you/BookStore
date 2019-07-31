using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Models
{
    public class PostCategory
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string MetaName { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
        public string IconUrl { get; set; }
        public string MetakeyWord { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string MetaDescription { get; set; }
        public List<Post> Posts { get; set; }
        public int Status { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual ApplicationUser UserCreated { get; set; }
        [ForeignKey("UpdatedBy")]
        public virtual ApplicationUser UserUpdated { get; set; }

    }
}
