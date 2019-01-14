using BookStore.Areas.Admin.ViewModels;
using BookStore.Data.Interfaces;
using BookStore.Data.Models;
using BookStore.Areas.Admin.Services;
using BookStore.SharedComponents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using System;
using System.Web;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Administrator")]
    [Route("admin/[controller]/[action]")]
    public class FeedbackController : Controller
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailRegisterRepository _emailRegister;

        public FeedbackController(IFeedbackRepository feedbackRepository, UserManager<ApplicationUser> userManager,IEmailRegisterRepository emailRegister)
        {
            _feedbackRepository = feedbackRepository;
            _userManager = userManager;
            _emailRegister = emailRegister;
        }

        public IActionResult mailbox(string opt="compose")
        {
            ViewBag.opt = opt;
            return View();
        }
        //view gởi mail
        public IActionResult Compose(string id)
        {
            var feedback = _feedbackRepository.GetFeedback(id);
            var userInfo = _userManager.Users.Where(x => x.Id == feedback.UserId).FirstOrDefault();
            var feedbackData = new FeedbackData
            {
                ID = feedback.ID,
                Message = feedback.Message,
                Subject = feedback.Subject,
                UserName = userInfo.UserName,
                Email = userInfo.Email,
                urlAvatar = userInfo.UrlAvatar, 
                time = ConvertToTimeSpan.Convert(feedback.CreatedDate),
                IsWaiting=feedback.IsWaiting
            };
            return View("_reply", feedbackData);
        }
        [HttpPost]
        public IActionResult Compose(FeedbackData data)
        {
            var mess = string.Empty;
            var feedbackItem = _feedbackRepository.GetFeedback(data.ID);
            try
            {
                EmailSender.SendMail(data.Email, data.Message, data.Subject);
                feedbackItem.Status = true;
                feedbackItem.IsWaiting = true;
                _feedbackRepository.SaveChange();
                ViewBag.status = true;
                return Redirect("mailbox");
            }
            catch (Exception e)
            {
                ViewBag.status = false;
                ViewBag.mess = e.Message;
                return View("_reply",data);
            }
            
        }
        [HttpPost]
        public IActionResult GetAll(string filter,string opt, int page, int pageSize = 10 )
        {
            var totalRows = 0;
            var feedbacks = _feedbackRepository.Feedbacks();
            var countNewMess = feedbacks.Where(x => x.Status == false&&DateTime.Now.Subtract(x.CreatedDate).Days<=0).Count();
            var countIsWating = feedbacks.Where(x => x.IsWaiting== true).Count();
            var data = (from fb in feedbacks
                        join
                        user in _userManager.Users on fb.UserId equals user.Id
                        select new FeedbackData
                        {
                            ID = fb.ID,
                            Message = fb.Message,
                            Subject = fb.Subject,
                            time = ConvertToTimeSpan.Convert(fb.CreatedDate),
                            UserName = user.UserName,
                            urlAvatar = user.UrlAvatar,
                            Status = fb.Status,
                            IsWaiting=fb.IsWaiting,
                            InboxCount=countNewMess,
                            IsWaitingCount=countIsWating
                        }
                      ).ToList();
            totalRows = data.Count();
            if (!string.IsNullOrEmpty(filter))
            {
                data = data.Where(p => p.Subject.ToLower().Contains(filter)).OrderBy(x => x.Subject).ToList();
                totalRows = data.Count();
            }
            if (opt=="news-message")
            {
                data = data.Where(p => p.Status==false).OrderBy(x => x.Subject).ToList();
                totalRows = data.Count();
            }else if (opt == "sends-message")
            {
                data = data.Where(p => p.Status == true).OrderBy(x => x.Subject).ToList();
                totalRows = data.Count();
            }else if (opt == "waits-message")
            {
                data = data.Where(p => p.IsWaiting == true).OrderBy(x => x.Subject).ToList();
                totalRows = data.Count();
            }
            var model = data.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Ok(new
            {
                data = model,
                total = totalRows,
                status = true
            });
        }
        public IActionResult GetEmail()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetEmailRegister(int page,int pageSize=10)
        {
            var totalRows = 0;
            var data = _emailRegister.EmailRegisters;
            totalRows = data.Count();
            var model = data.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Ok(new
            {
                data = model,
                total = totalRows,
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
                    _feedbackRepository.Delete(id);
                }
                status = true;
                message = ResultState.Delete_SUCCESS;
            }
            catch
            {
                status = false;
                message = ResultState.Delete_FALSE;
            }
            _feedbackRepository.SaveChange();
            return Json(new
            {
                status = status,
                message = message
            });
        }
        [HttpGet]
        public IActionResult sendemails(string[] ids)
        {
            var sendMailViewModel = new SendMailViewModels
            {
                Ids = ids
            };
            return View("sendemails",sendMailViewModel);
        }
        [HttpPost]
        public IActionResult sendemails(SendMailViewModels model)
        {
            try
            {
                foreach(var id in model.Ids)
                {
                    var mailrecive = _emailRegister.GetEmail(id);
                    EmailSender.SendMail(mailrecive.Email, model.Subject, model.Content);
                }
            }
            catch (Exception)
            {

            }
            return RedirectToAction("getemail");
        }
         [HttpPost]
        public IActionResult DeleteEmail(string[] ids)
        {
            var status = false;
            string message = string.Empty;
            try
            {
                foreach (var id in ids)
                {
                    _emailRegister.Delete(id);
                }
                status = true;
                message = ResultState.Delete_SUCCESS;
            }
            catch
            {
                status = false;
                message = ResultState.Delete_FALSE;
            }
            _emailRegister.SaveChange();
            return Json(new
            {
                status = status,
                message = message
            });
        }

        [HttpPost]
        public IActionResult GetDetail(string id)
        {
            var book = _feedbackRepository.GetFeedback(id);
            return Json(new
            {
                data = book,
                status = true
            });
        }
    }
}