using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Models
{
    public class History
    {
        public string Id { get; set; }
        public string ActionName { get; set; }
        public string UserId { get; set; }
        public DateTime ActionTime { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser{ get; set; }
    }
}
