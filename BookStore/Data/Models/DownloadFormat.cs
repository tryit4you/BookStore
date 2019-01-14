using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Models
{
    public class DownloadFormat
    {
        public string Id { get; set; } 
        public string PdfLink { get; set; }
        public string EpubLink { get; set; }
        public string MobiLink { get; set; }
        public string DisplayName { get; set; }

    }
}
