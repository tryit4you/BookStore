using BookStore.Areas.Admin.ViewModels;
using BookStore.Data.Interfaces;
using BookStore.Data.Models;
using BookStore.SharedComponents;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using System.Security.Claims;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Administrator")]
    [Route("admin/[controller]/[action]")]
    public class BooksController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IDownloadFormatRepository _downloadFormat;

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public BooksController(IBookRepository bookRepository,
            IHostingEnvironment hostingEnvironment,
            UserManager<ApplicationUser> userManager,
            IDownloadFormatRepository downloadFormat
            )
        {
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
            _bookRepository = bookRepository;
            _downloadFormat = downloadFormat;
        }

        public IActionResult Index()
        {
            var bookList = _bookRepository.Books.Where(x => x.Status == false).ToList();
            var userList = (from user in _userManager.Users.Where(x => x.Status == true)
                           join book in bookList on user.Id equals book.UserId
                           select user).Distinct().ToList();
            var dataList = new ApproveEbooksViewModels
            {
                Users = userList,
                Books = bookList
            };
            var dataCollapse = new ApproveEbooksViewModels
            {
                Users = (from user in _userManager.Users.Where(x => x.Status == true)
                         join book in _bookRepository.Books on user.Id equals book.UserId
                         select user).Distinct().ToList(),
                Books = _bookRepository.Books.ToList()
            };
            ViewBag.CollapseList = dataCollapse;
            return View(dataList);
        }

        public IActionResult GetAll(string filter, int page, int pageSize = 10)
        {
            var totalRows = 0;
            var books = _bookRepository.Books;
            totalRows =books.Count();
            if (!string.IsNullOrEmpty(filter))
            {
                books = books.Where(p => p.Name.ToLower().Contains(filter)).OrderBy(x => x.Name).ToList();
                totalRows = books.Count();
            }

            var data = (from book in books
                       join user in _userManager.Users on
                       book.UserId equals user.Id
                       orderby book.CreatedDate descending
                       select new
                       {
                           book.Name,
                           book.CreatedDate,
                           book.Authors,
                           book.Cappacity,
                           book.CountDownload,
                           book.Id,
                           book.ThumbnailUrl,
                           book.Status,
                           userName = user.DisplayName ?? user.UserName,
                       });
            var model = data.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Ok(new
            {
                data = model,
                total = totalRows,
                currentPage=page,
                status = true
            });
        }

        private string SaveLinkDownload(DownloadFormat downloadFormat)
        {
            var formatId= _downloadFormat.Post(downloadFormat);
            return formatId;
        }
        [HttpPost]
        public IActionResult Post(string book,string downloadLink)
        {
            var status = false;
            string message = string.Empty;
            var data = JsonConvert.DeserializeObject<Book>(book);
            var downloadUrls = JsonConvert.DeserializeObject<DownloadFormat>(downloadLink);
            var formatId= SaveLinkDownload(downloadUrls);
            if (User.Identity.IsAuthenticated)
            {
                var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                data.UserId = userId;
                data.FormatDownload = formatId;
                _bookRepository.Post(data);
                _bookRepository.SaveChange();
                status = true;
                message = ResultState.Add_SUCCESS;
            }
            else
            {
                status = false;
                message = ResultState.Add_FALSE;
            }

            return Json(new
            {
                status = status,
                message=message
            });
        }

        [HttpPost]
        public IActionResult Put(string book, string downloadLink)
        {
            var message = string.Empty;
            bool status = false;
            var data = JsonConvert.DeserializeObject<Book>(book);
            var links = JsonConvert.DeserializeObject<DownloadFormat>(downloadLink);
            _downloadFormat.Put(links);
            data.FormatDownload = links.Id;
            var result = _bookRepository.Put(data);
            try
            {
                if (result)
                    _bookRepository.SaveChange();
                status = true;
                message = ResultState.Update_SUCCESS;
            }
            catch
            {
                status = false;
                message = ResultState.Update_FALSE;
            }
            return Json(new
            {
                status = status,
                message = message
            });
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            var result = _bookRepository.Delete(id);
            _bookRepository.SaveChange();
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public IActionResult DeleteMul(string[] ids)
        {
            var status = false;
            string message = string.Empty;
            try
            {
                foreach (var id in ids)
                {
                    _bookRepository.Delete(id);
                }
                status = true;
                message = ResultState.Delete_SUCCESS;
            }
            catch
            {
                status = false;
                message = ResultState.Delete_FALSE;
            }
            _bookRepository.SaveChange();
            return Json(new
            {
                status = status,
                message = message
            });
        }

        [HttpPost]
        public IActionResult GetDetail(string id)
        {
            var book = _bookRepository.GetBook(id);
            var formatId = book.FormatDownload;
            var getlink = _downloadFormat.GetLinkDownloads(formatId);
            var getBookDetailVm = new GetBookDetailViewModels
            {
                Book = book,
                DownloadLinks = getlink
            };
            return Json(new
            {
                data = getBookDetailVm,
                status = true
            });
        }

        [HttpPost]
        public IActionResult EditImage(string id)
        {
            var book = _bookRepository.GetBook(id);
            return Json(new
            {
                data = book,
                status = true
            });
        }
       
        [HttpPost]
        public IActionResult CheckExist(string name)
        {
            var result = _bookRepository.CheckName(name);
            return Json(new
            {
                result = !result
            });
        }

        public IActionResult ChangeStatus(string id)
        {
            bool status;
            string message = string.Empty;
            var book = _bookRepository.GetById(id);
            if (book == null)
            {
                status = false;
                message = ResultState.NotFound;
            }
            else
            {
                book.Status = !book.Status;
                _bookRepository.SaveChange();
                status = book.Status;
            }
            return Json(new
            {
                status = status,
                message = message
            });
        }
    }
}