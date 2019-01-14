using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BookStore.Data.Interfaces;
using BookStore.Data.Models;
using BookStore.SharedComponents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookStore.Areas.Admin.Controllers
{ 
        [Area("admin")]
        [Authorize(Roles = "Administrator")]
        [Route("admin/[controller]/[action]")]
        public class CourseCategoryController : Controller
        {
            private readonly ICourseCategoryRepository _categoryRepository;

            public CourseCategoryController(ICourseCategoryRepository categoryRepository)
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
                var categories = _categoryRepository.Categories();
                totalRows = categories.Count();
                if (!string.IsNullOrEmpty(filter))
                {
                categories = categories.Where(p => p.Name.ToLower().Contains(filter)).OrderBy(x => x.Name).ToList();
                    totalRows = categories.Count();
                }
                var model = categories.Skip((page - 1) * pageSize).Take(pageSize).ToList();
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
                var data = JsonConvert.DeserializeObject<CourseCategory>(category);
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            data.UserId = userId;
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
                var data = JsonConvert.DeserializeObject<CourseCategory>(category);
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
                var category = _categoryRepository.GetById(id);
                return Json(new
                {
                    data = category,
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