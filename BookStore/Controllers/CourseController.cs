using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseRepository _courseRepository;
        public CourseController(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetAll(string filter, int page, int pageSize = 10)
        {
            var totalRows = 0;
            var course = _courseRepository.Courses().Where(x => x.Status == true);
            totalRows = course.Count();
            if (!string.IsNullOrEmpty(filter))
            {
                course = course.Where(p => EF.Functions.Like(p.Name, "%" + filter + "%") || p.Description.Contains(filter.ToLower())).OrderBy(x => x.Name).ToList();
                totalRows = course.Count();
            }
            course = course.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Ok(new
            {
                filter = filter,
                data = course,
                total = totalRows,
                page = page,
                status = true
            });
        }
        public IActionResult Detail(string id)
        {
            var courseDetail = _courseRepository.GetCourse(id);
         

            return View(courseDetail);
        }
    }
}