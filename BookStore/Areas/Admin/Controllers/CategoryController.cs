using BookStore.Data.Interfaces;
using BookStore.Data.Models;
using BookStore.SharedComponents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Administrator")]
    [Route("admin/[controller]/[action]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Category()
        {
            var categorys = _categoryRepository.Categories().ToList();
            return Json(new
            {
                data = categorys,
                status = true
            });
        }

        public IActionResult GetAll(string filter, int page, int pageSize = 10)
        {
            var totalRows = 0;
            var books = _categoryRepository.Categories();
            totalRows = books.Count();
            if (!string.IsNullOrEmpty(filter))
            {
                books = books.Where(p => p.Name.ToLower().Contains(filter)).OrderBy(x => x.Name).ToList();
                totalRows = books.Count();
            }
            var model = books.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Ok(new
            {
                data = model,
                total = totalRows,
                status = true
            });
        }

        [HttpPost]
        public IActionResult Post(string category)
        {
            var data = JsonConvert.DeserializeObject<Category>(category);
            _categoryRepository.Post(data);
            _categoryRepository.SaveChange();
            return Json(new
            {
                status = true,
                message = ResultState.Add_SUCCESS
            });
        }

        [HttpPost]
        public IActionResult Put(string category)
        {
            var data = JsonConvert.DeserializeObject<Category>(category);
            var result = _categoryRepository.Put(data);
            if (result)
                _categoryRepository.SaveChange();
            return Json(new
            {
                status = true,
                message = ResultState.Update_SUCCESS
            });
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            var result = _categoryRepository.Delete(id);
            _categoryRepository.SaveChange();
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
                    _categoryRepository.Delete(id);
                }
                status = true;
                message = ResultState.Delete_SUCCESS;
            }
            catch
            {
                status = false;
                message = ResultState.Delete_FALSE;
            }
            _categoryRepository.SaveChange();
            return Json(new
            {
                status = status,
                message = message
            });
        }

        [HttpPost]
        public IActionResult GetDetail(string id)
        {
            var book = _categoryRepository.GetById(id);
            return Json(new
            {
                data = book,
                status = true
            });
        }

        [HttpPost]
        public IActionResult CheckExist(string name)
        {
            var result = _categoryRepository.CheckName(name);
            return Json(new
            {
                result = !result
            });
        }

        public IActionResult ChangeStatus(string id)
        {
            bool status;
            string message = string.Empty;
            var category = _categoryRepository.GetById(id);
            if (category == null)
            {
                status = false;
                message = ResultState.NotFound;
            }
            else
            {
                category.Status = !category.Status;
                _categoryRepository.SaveChange();
                status = category.Status;
            }
            return Json(new
            {
                status = status,
                message = message
            });
        }
    }
}