using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Data.Models
{
    public class Category
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [DefaultValue(0)]
        public int SummaryBook { get; set; }
        [Required]
        public string MetaName { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [DefaultValue(true)]
        public bool Status { get; set; }
        public List<BookType> BookTypes{ get; set; }
    }
}