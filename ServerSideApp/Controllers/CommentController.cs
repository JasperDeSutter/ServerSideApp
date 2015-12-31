using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ServerSideApp.Models;
using ServerSideApp.Models.Comment;
using ServerSideApp.Repositories;

namespace ServerSideApp.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private ApplicationUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        private string UserId => UserManager.FindByName(User.Identity.Name).Id;
        private string UserName(string id) => UserManager.FindById(id).UserName;

        [HttpPost]
        [Authorize]
        public ActionResult AddComment(string text, Topics topic, int? postId) {
            if (!User.Identity.IsAuthenticated || text == null || topic == Topics.UNDEFINED) return Redirect(Request.UrlReferrer?.ToString());
            var comment = new Comment {
                Text = text,
                UserId = UserId,
                PostId = postId,
                Time = DateTime.Now,
                Hidden = false,
                TopicId = (int)topic
            };
            CommentRepository.Add(comment);
            return Redirect(Request.UrlReferrer?.ToString());
        }
        [HttpGet]
        [ChildActionOnly]
        [Authorize]
        public ActionResult GetComments(Topics topic, int? postId, string userId = null, float minimumRating = 0) {
            var all = CommentRepository.Get(topic, postId);
            var comments = new CommentListPM {
                CanCreate = User.Identity.IsAuthenticated,
                CanHide = User.IsInRole("Administrator"),
                Topic = topic,
                PostId = postId,
                Comments = all.Select(comment => new CommentPM {
                    Text = comment.Text,
                    UserName = UserManager.FindById(comment.UserId).UserName,
                    Time = comment.Time,
                    Id = comment.Id
                })
            };
            return View("Comments", comments);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Hide(int id, bool hide) {
            CommentRepository.Hide(id, hide);
            return Redirect(Request.UrlReferrer?.ToString());
        }
    }
}