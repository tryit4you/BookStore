using BookStore.Areas.Admin.ViewModels;
using BookStore.Data.Interfaces;
using BookStore.Data.Models;
using BookStore.SharedComponents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Administrator")]
    [Route("admin/[controller]/[action]")]
    public class BookTypeController : Controller
    {
        private readonly IBookTypeRepository _bookTypeRepository;
        private readonly ICategoryRepository _categoryRepository;

        public BookTypeController(IBookTypeRepository bookTypeRepository, ICategoryRepository categoryRepository)
        {
            _bookTypeRepository = bookTypeRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            var categories = _categoryRepository.Categories().OrderBy(x=>x.Name).ToList();
            var bookType = _bookTypeRepository.AllBookTypes().OrderBy(x => x.Name).ToList();
            var loadByCateVm = new LoadByCategoryViewModel
            {
                Categories = categories,
                BookTypes = bookType
            };
            return View(loadByCateVm);
        }

        public IActionResult BookType()
        {
            var bookTypes = _bookTypeRepository.AllBookTypes().ToList();
            return Json(new
            {
                data = bookTypes,
                status = true
            });
        }

        public IActionResult GetAll(string filter, int page, int pageSize = 10)
        {
            var totalRows = 0;
            var booktypes = _bookTypeRepository.AllBookTypes();
            totalRows = booktypes.Count();
            if (!string.IsNullOrEmpty(filter))
            {
                booktypes = booktypes.Where(p => p.Name.ToLower().Contains(filter)).OrderBy(x => x.Name).ToList();
                totalRows = booktypes.Count();
            }
            var data = (from booktype in booktypes
                        join category in _categoryRepository.Categories()
                        on booktype.CategoryId equals category.Id
                        select new
                        {
                            Id = booktype.Id,
                            Name = booktype.Name,
                            Thumbnail= booktype.ThumbnailUrl,
                            Description = booktype.Description,
                            CreatedDate = booktype.CreatedDate,
                            Status = booktype.Status,
                            CategoryName = category.Name
                        }).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Ok(new
            {
                data = data,
                total = totalRows,
                status = true
            });
        }

        [HttpPost]
        public IActionResult Post(string bookType)
        {
            var data = JsonConvert.DeserializeObject<BookType>(bookType);

            _bookTypeRepository.Post(data);
            _bookTypeRepository.SaveChange();
            return Json(new
            {
                status = true,
                message = ResultState.Add_SUCCESS
            });
        }

        [HttpPost]
        public IActionResult Put(string bookType)
        {
            var data = JsonConvert.DeserializeObject<BookType>(bookType);
            var result = _bookTypeRepository.Put(data);
            if (result)
                _bookTypeRepository.SaveChange();
            return Json(new
            {
                status = true,
                message = ResultState.Update_SUCCESS
            });
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            var result = _bookTypeRepository.Delete(id);
            _bookTypeRepository.SaveChange();
            return Json(new
            {
                status = true,
                message = ResultState.Delete_SUCCESS
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
                    _bookTypeRepository.Delete(id);
                }
                status = true;
                message = ResultState.Delete_SUCCESS;
            }
            catch
            {
                status = false;
                message = ResultState.Delete_FALSE;
            }
            _bookTypeRepository.SaveChange();
            return Json(new
            {
                status = status,
                message = message
            });
        }

        [HttpPost]
        public IActionResult GetDetail(string id)
        {
            var book = _bookTypeRepository.GetById(id);
            return Json(new
            {
                data = book,
                status = true
            });
        }

        [HttpPost]
        public IActionResult CheckExist(string name)
        {
            var result = _bookTypeRepository.CheckName(name);
            return Json(new
            {
                result = !result
            });
        }

        public IActionResult ChangeStatus(string id)
        {
            bool status;
            string message = string.Empty;
            var category = _bookTypeRepository.GetById(id);
            if (category == null)
            {
                status = false;
                message = ResultState.NotFound;
            }
            else
            {
                category.Status = !category.Status;
                _bookTypeRepository.SaveChange();
                status = category.Status;
            }
            return Json(new
            {
                status = status,
                message = message
            });
        }

        public JsonResult GetByCategory(string categoryId)
        {
            var booktype = _bookTypeRepository.GetByCategory(categoryId);
            return Json(booktype);
        }
    }
}