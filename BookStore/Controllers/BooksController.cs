using BookStore.Data.Interfaces;
using BookStore.Data.Models;
using BookStore.Services;
using BookStore.SharedComponents;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBookTypeRepository _bookTypeRepository;
        private readonly IDownloadFormatRepository _downloadFormat;
        private readonly UserManager<ApplicationUser> _userManager;
        public BooksController(IBookRepository bookRepository,
            ICategoryRepository categoryRepository,
            IBookTypeRepository bookTypeRepository,
            IDownloadFormatRepository downloadFormat,
            UserManager<ApplicationUser> userManager)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
            _bookTypeRepository = bookTypeRepository;
            _downloadFormat = downloadFormat;
            _userManager = userManager;
        }
        public IActionResult Index()
        {

            return View();
        }
        public IActionResult GetAll(string filter, int page, int pageSize = 10)
        {
            var totalRows = 0;
            var books = _bookRepository.Books.Where(x => x.Status == true);
            totalRows = books.Count();
            if (!string.IsNullOrEmpty(filter))
            {
                books = books.Where(p => EF.Functions.Like(p.Name, "%" + filter + "%") || p.LongDescription.Contains(filter.ToLower())).OrderBy(x => x.Name).ToList();
                totalRows = books.Count();
            }
            books = books.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Ok(new
            {
                filter = filter,
                data = books,
                total = totalRows,
                page=page,
                status = true
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
        [Authorize]
        public async Task<IActionResult> GetBookByUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return View(user);
        }
        [Authorize]
        public IActionResult BookByUser(string filter, string userId, int page, int pageSize = 10)
        {
            var totalRows = 0;
            var books = _bookRepository.Books.Where(x => x.UserId == userId);
            totalRows = books.Count();
            if (!string.IsNullOrEmpty(filter))
            {
                books = books.Where(p => EF.Functions.Like(p.Name, "%" + filter + "%") || p.LongDescription.Contains(filter.ToLower())).OrderBy(x => x.Name).ToList();
                totalRows = books.Count();
            }
            books = books.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Ok(new
            {
                filter = filter,
                data = books,
                total = totalRows,
                page=page,
                status = true
            });
        }
        [Authorize]
        public async Task<IActionResult> GetManagers(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return View(user);
        }
        [Authorize]
        public async Task<IActionResult> ManagersAsync(string query, string userId, int page, int pageSize = 10)
        {
            var totalRows = 0;
            var user = await _userManager.FindByIdAsync(userId);
            var books = _bookRepository.Books.Where(x => x.UserId == userId);
            totalRows = books.Count();
            if (!string.IsNullOrEmpty(query))
            {
                books = books.Where(p => p.Name.ToLower().Contains(query)).OrderBy(x => x.Name).ToList();
                totalRows = books.Count();
            }
            books = books.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Ok(new
            {
                data = books,
                total = totalRows,
                page=page,
                status = true
            });
        }
        public IActionResult Detail(string id)
        {
            var bookDetail = _bookRepository.GetBook(id);
            var relatedBook = _bookRepository.RelatedBooks(id);
            var booktype = _bookTypeRepository.GetById(bookDetail.BookTypeId);
            var linkDownloads = _downloadFormat.GetLinkDownloads(bookDetail.FormatDownload);
            var bookDetailViewModel = new BookDetailViewModel
            {
                BookDetail = bookDetail,
                BookType = booktype,
                RelatedBooks = relatedBook,
                DownloadLinks = linkDownloads
            };

            return View(bookDetailViewModel);
        }
        [Authorize]
        public async Task<IActionResult> PutAsync(string userId, string id)
        {
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.Id = id;
            return View(user);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Put(string book, string downloadLink)
        {
            var status = false;
            string message = string.Empty;
            var data = JsonConvert.DeserializeObject<Book>(book);

            var downloadUrls = JsonConvert.DeserializeObject<DownloadFormat>(downloadLink);

            var result = UpdateLink(downloadUrls);
            if (result!=null)
            {
                data.FormatDownload = result;
                _bookRepository.Put(data);
                _downloadFormat.SaveChange();
                _bookRepository.SaveChange();
                status = true;
                message = ResultState.Update_SUCCESS;
            }
            else
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
        [Authorize]
        public IActionResult GetEbookDetail(string id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var status = false;
            string message = string.Empty;
            var bookDetail = _bookRepository.Books.Where(x => x.Id == id && x.UserId == userId).FirstOrDefault();

            if (bookDetail != null)
            {
                status = true;
            }
            else
            {
                message = ResultState.NotFound;
            }
            var formatId = bookDetail.FormatDownload;
            var getlink = _downloadFormat.GetLinkDownloads(formatId);
            var getBookDetailVm = new GetBookDetailViewModels
            {
                Book = bookDetail,
                DownloadLinks = getlink
            };
            return Json(new
            {
                status = status,
                data = getBookDetailVm,
                message = message
            });
        }
        private string SaveLinkDownload(DownloadFormat downloadFormat)
        {
            var formatId = _downloadFormat.Post(downloadFormat);
            return formatId;
        }
        private string UpdateLink(DownloadFormat downloadLink)
        {
            var result = _downloadFormat.Put(downloadLink);
            return result;
        }
        [HttpPost]
        [Authorize]
        public IActionResult Post(string book, string downloadLink)
        {
            var status = false;
            string message = string.Empty;
            var data = JsonConvert.DeserializeObject<Book>(book);
            var downloadUrls = JsonConvert.DeserializeObject<DownloadFormat>(downloadLink);
            var formatId = SaveLinkDownload(downloadUrls);
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
                message = message
            });
        }
        public IActionResult LoadByCategory(string id)
        {
            var category = _categoryRepository.GetById(id);
            return View(category);
        }
        [HttpPost]
        public IActionResult GetByCategory(string id, string filter, int page, int pageSize = 10)
        {
            var totalRows = 0;
            var books = _bookRepository.GetBooksByCategory(id).Where(x=>x.Status==true).ToList();
            totalRows = books.Count();
            if (!string.IsNullOrEmpty(filter))
            {
                books = books.Where(p => EF.Functions.Like(p.Name, "%" + filter + "%") ||
                p.LongDescription.Contains(filter.ToLower())).
                    OrderBy(x => x.Name).ToList();
                totalRows = books.Count;
            }
            books = books.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Ok(new
            {
                filter = filter,
                data = books,
                total = totalRows,
                page=page,
                status = true
            });
        }
        public IActionResult LoadByType(string id)
        {
            var type = _bookTypeRepository.GetById(id);
            return View(type);
        }
        [HttpPost]
        public IActionResult GetByType(string id, string filter, int page, int pageSize = 10)
        {
            var books = _bookRepository.GetBooksByType(id).Where(x=>x.Status==true).ToList();
            var totalRows = books.Count();
          
            if (!string.IsNullOrEmpty(filter))
            {
                books = books.Where(p => EF.Functions.Like(p.Name, "%" + filter + "%")).OrderBy(x => x.Name).ToList();
                totalRows = books.Count;
            }
            books = books.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Ok(new
            {
                data = books,
                total = totalRows,
                page=page,
                status = true
            });
        }
        [HttpPost]
        public IActionResult IncreaseDownload(string bookid)
        {
            var status = false;
            var book = _bookRepository.GetById(bookid);
            if (book != null)
            {
                book.CountDownload += 1;
                _bookRepository.SaveChange();
                status = true;
            }
            return Json(new
            {
                status = status
            });
        }
        [Authorize]
        public async Task<IActionResult> Upload(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return View(user);
        }
        [Authorize]
        public IActionResult GetBookCategory()
        {
            var bookCategories = _categoryRepository.Categories().OrderBy(x => x.Name);
            return Json(new
            {
                status = true,
                data = bookCategories
            });
        }
        public IActionResult GetbyCategory(string categoryId)
        {

            var types = _bookTypeRepository.AllBookTypes().Where(x => x.CategoryId == categoryId).OrderBy(x => x.Name);
            return Json(new
            {
                status = true,
                data = types
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
        public IActionResult CheckExist(string name)
        {
            var result = _bookRepository.CheckName(name);
            return Json(new
            {
                result = !result
            });
        }

    }
}