using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Data.Interfaces;
using BookStore.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookStore.Controllers
{
    [Authorize]
    public class FeedbackController : Controller
    {
        private readonly IFeedbackRepository _feedbackRepository;
        public FeedbackController(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        [HttpPost]
        public IActionResult Post(string feedback)
        {
            var status = false;
            var message = string.Empty;
            try
            {

                var data = JsonConvert.DeserializeObject<Feedback>(feedback);
                data.CreatedDate = DateTime.Now;
                _feedbackRepository.AddFeedback(data);
                _feedbackRepository.SaveChange();
                status = true;
            }
            catch (Exception)
            {
                message = "";
            }
            return Json(new
            {
                status = status
            });
        }
    }
}