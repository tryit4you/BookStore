using BookStore.Data.Interfaces;
using BookStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Repositories
{
    public class DownloadFormatRepository : IDownloadFormatRepository
    {
        private readonly BookStoreDbContext _dbContext;
        public DownloadFormatRepository(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DownloadFormat GetLinkDownloads(string id) => _dbContext.DownloadFormats.Where(x => x.Id == id).FirstOrDefault();

        public string Post(DownloadFormat formats) => _dbContext.DownloadFormats.Add(formats).Entity.Id;

        public string Put(DownloadFormat format)
        {
            var formatItem= _dbContext.DownloadFormats.Where(x=>x.Id==format.Id).FirstOrDefault();
            if (formatItem==null)
            {
                return formatItem.Id;
            }
            formatItem.DisplayName = format.DisplayName;
            formatItem.EpubLink = format.EpubLink;
            formatItem.PdfLink = format.PdfLink;
            formatItem.MobiLink = format.MobiLink;
            return formatItem.Id;
        }

        public void SaveChange() => _dbContext.SaveChanges();
    }
}
