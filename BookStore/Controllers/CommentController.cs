using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Data.Interfaces;
using BookStore.Data.Models;
using BookStore.SharedComponents;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class CommentController : Controller
    {
        private readonly I_p_CommentsRepository _P_CommentsRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public CommentController(I_p_CommentsRepository P_CommentsRepository,
            UserManager<ApplicationUser> userManager)
        {
            _P_CommentsRepository = P_CommentsRepository;
            _userManager = userManager;
        }
        [HttpPost]
        public IActionResult CheckAuthorize()
        {
            
            string message = string.Empty;
            if (!User.Identity.IsAuthenticated)
            {
                message = "Bạn cần đăng nhập để thực hiện chức năng này!";
                return Json(new
                {
                    status = false,
                    message = message
                });
            }
            return Json(new
            {
                status = true
            });
        }
        [HttpPost]
        public async Task<IActionResult> SubmitComment(string message,string ebookId)
        {
            var status = false;
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var comment = new P_Comment();
            comment.p_id = Guid.NewGuid().ToString();
            comment.Message = message;
            comment.CreatedDate = DateTime.Now;
            comment.eBookId = ebookId;
            comment.UserId = currentUser.Id;
            comment.UserName= currentUser.UserName;
            var result= _P_CommentsRepository.CreatedComment(comment);
            if (result)
            {
                _P_CommentsRepository.SaveChange();
            }
            else
            {
                status = false;
            }
            
                status = true;
         
            return Json(new
            {
                status=status
            });
        }
        [HttpPost]
        public IActionResult GetComments(string bookId) {
            var comments =from c in _P_CommentsRepository.AllCommentsByBookId(bookId)
                          join u in _userManager.Users on c.UserId equals u.Id
                          select new
                          {
                              c.eBookId,
                              c.Message,
                              c.p_id,
                              u.UrlAvatar,
                              u.UserName,
                              createdTime =ConvertToTimeSpan.Convert(c.CreatedDate)
                          };
            return Json(new
            {
                data=comments,
                status=true
            });
        }
    }
}