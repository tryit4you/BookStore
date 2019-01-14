using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Areas.Admin.ViewModels
{
    public class HomeViewModels
    {
        public EbookVm EbookVm { get; set; }
        public AccountVm AccountVm { get; set; }
        public FeedbackVm FeedbackVm { get; set; }
        public Category_TypeVm Category_TypeVm { get; set; }
    }
    public class EbookVm
    {
        public int Total { get; set; }
        public int InToday { get; set; }
    }
    public class AccountVm
    {
        public int Total { get; set; }
        public int RegisterToday { get; set; }
    }
    public class FeedbackVm
    {
        public int Total { get; set; }
        public int InToday { get; set; }
    }
    public class Category_TypeVm
    {
        public int Category { get; set; }
        public int Type { get; set; }
    }
}
