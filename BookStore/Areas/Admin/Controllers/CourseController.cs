using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BookStore.Data.Interfaces;
using BookStore.Data.Models;
using BookStore.SharedComponents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Administrator")]
    [Route("admin/[controller]/[action]")]
    public class CourseController : Controller
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseCategoryRepository _courseCategoryRepository;
        
        private readonly UserManager<ApplicationUser> _userManager;

        public CourseController(ICourseRepository courseRepository,
            UserManager<ApplicationUser> userManager,
            ICourseCategoryRepository courseCategoryRepository
            )
        {
            _userManager = userManager;
            _courseCategoryRepository = courseCategoryRepository;
            _courseRepository = courseRepository;
        }

        public IActionResult Index()
        {
            var courseList = _courseRepository.Courses().ToList();

            return View(courseList);
        }

        public IActionResult GetAll(string filter, int page, int pageSize = 10)
        {
            var totalRows = 0;
            var courses = _courseRepository.Courses();
            totalRows = courses.Count();
            if (!string.IsNullOrEmpty(filter))
            {
                courses = courses.Where(p => p.Name.ToLower().Contains(filter)).OrderBy(x => x.Name).ToList();
                totalRows = courses.Count();
            }

          
            var model = courses.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Ok(new
            {
                data = model,
                total = totalRows,
                currentPage = page,
                status = true
            });
        }

        [HttpPost]
        public IActionResult Post(string Course)
        {
            var status = false;
            string message = string.Empty;
            var data = JsonConvert.DeserializeObject<Course>(Course);
            if (User.Identity.IsAuthenticated)
            {
                var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                data.UserId = userId;
                _courseRepository.Post(data);
                _courseRepository.SaveChange();
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

        [HttpPost]
        public IActionResult Put(string Course)
        {
            var message = string.Empty;
            bool status = false;
            var data = JsonConvert.DeserializeObject<Course>(Course);
            var result = _courseRepository.Put(data);
            try
            {
                if (result)
                    _courseRepository.SaveChange();
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
            var result = _courseRepository.Delete(id);
            _courseRepository.SaveChange();
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
                    _courseRepository.Delete(id);
                }
                status = true;
                message = ResultState.Delete_SUCCESS;
            }
            catch
            {
                status = false;
                message = ResultState.Delete_FALSE;
            }
            _courseRepository.SaveChange();
            return Json(new
            {
                status = status,
                message = message
            });
        }

        [HttpPost]
        public IActionResult GetDetail(string id)
        {
            var Course = _courseRepository.GetCourse(id);
          
            return Json(new
            {
                data = Course,
                status = true
            });
        }

        [HttpPost]
        public IActionResult EditImage(string id)
        {
            var Course = _courseRepository.GetCourse(id);
            return Json(new
            {
                data = Course,
                status = true
            });
        }

        [HttpPost]
        public IActionResult CheckExist(string name)
        {
            var result = _courseRepository.CheckName(name);
            return Json(new
            {
                result = !result
            });
        }

        public IActionResult ChangeStatus(string id)
        {
            bool status;
            string message = string.Empty;
            var Course = _courseRepository.GetCourse(id);
            if (Course == null)
            {
                status = false;
                message = ResultState.NotFound;
            }
            else
            {
                Course.Status = !Course.Status;
                _courseRepository.SaveChange();
                status = Course.Status;
            }
            return Json(new
            {
                status = status,
                message = message
            });
        }
    }
}