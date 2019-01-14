using BookStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Interfaces
{
    public interface IDownloadFormatRepository
    {
        string Post(DownloadFormat formats);
        string Put(DownloadFormat format);
       DownloadFormat GetLinkDownloads(string id);
        void SaveChange();
    }
}
