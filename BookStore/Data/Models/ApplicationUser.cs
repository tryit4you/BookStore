using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        [DisplayName("Tên ")]
        public string DisplayName { get; set; }

        [DisplayName("Ghi chú")]
        public string Notes { get; set; }

        [DisplayName("Ngày tạo")]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Hình đại diện")]
        public string UrlAvatar { get; set; }

        [DisplayName("Địa chỉ")]
        public string Address { get; set; }

        [DisplayName("Sửa ngày cuối")]
        public DateTime LastModifiedDate { get; set; }

        [DefaultValue(1)]
        public bool Status { get; set; }

        public virtual IEnumerable<Book> Books { get; set; }
        public virtual IEnumerable<Feedback> Feedbacks { get; set; }
    }
}