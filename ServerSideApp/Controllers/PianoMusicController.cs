using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ServerSideApp.Models;
using ServerSideApp.Models.Piano;
using ServerSideApp.Repositories;

namespace ServerSideApp.Controllers
{
    public class PianoMusicController : Controller
    {
        private ApplicationUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        private string UserId => UserManager.FindByName(User.Identity.Name).Id;
        private string UserName(string id) => UserManager.FindById(id).UserName;

        [HttpGet]
        public ActionResult Index() {
            return View();
        }
        [HttpGet]
        [Authorize]
        public ActionResult List() {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Index");
            var all = PianoPieceRepository.GetAll();
            var allpm = new List<DetailsPM>();
            foreach (var piece in all) {
                var add = new DetailsPM {
                    HasMidi = !string.IsNullOrEmpty(piece.MidiPath),
                    HasMp3 = !string.IsNullOrEmpty(piece.Mp3Path),
                    HasPdf = !string.IsNullOrEmpty(piece.PdfPath),
                    CanEdit = User.Identity.IsAuthenticated && UserId == piece.UserId,
                    UserName = UserName(piece.UserId),
                    CommentCount = CommentRepostory.GetCommentCount(Topics.PianoMusic, piece.UserId, piece.Id),
                    Genre = PianoGenreRepository.Get(piece.GenreId).Name,
                    Difficulty = PianoDifficultyRepository.Get(piece.DifficultyId).Name
                };
                add.CopyFrom(piece);
                allpm.Add(add);
            }

            var listPm = new ListPM {
                CanCreate = User.Identity.IsAuthenticated && User.IsInRole("SuperUser"),
                PianoPieces = allpm
            };

            return View(listPm);
        }
        [HttpGet]
        [Authorize]
        public ActionResult Details(int? id) {
            if (!id.HasValue || !User.Identity.IsAuthenticated) return RedirectToAction("Index");
            var piece = PianoPieceRepository.Get(id.Value);
            var detailsPm = new DetailsPM {
                HasMidi = !string.IsNullOrEmpty(piece.MidiPath),
                HasMp3 = !string.IsNullOrEmpty(piece.Mp3Path),
                HasPdf = !string.IsNullOrEmpty(piece.PdfPath),
                CanEdit = User.Identity.IsAuthenticated && UserId == piece.UserId,
                UserName = UserName(piece.UserId),
                Genre = PianoGenreRepository.Get(piece.GenreId).Name,
                Difficulty = PianoDifficultyRepository.Get(piece.DifficultyId).Name
            };
            detailsPm.CopyFrom(piece);
            detailsPm.Comments = CommentRepostory.Get(Topics.PianoMusic, id);
            return View(detailsPm);
        }

        public FileResult GetPdf(int? id) {
            if (!User.Identity.IsAuthenticated || !id.HasValue) return null;
            var filename = PianoPieceRepository.Get(id.Value).PdfPath;
            var path = Path.Combine(Server.MapPath("/RecipeImages/"), filename);
            var stream = new StreamReader(path);
            return File(stream.ReadToEnd(), "application/pdf");
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddComment(Comment comment, int? postId) {
            if (!User.Identity.IsAuthenticated || !postId.HasValue || comment == null) return RedirectToAction("Index");
            comment.UserId = UserId;
            comment.PostId = postId.Value;
            comment.Time = DateTime.Now;
            comment.Hidden = false;
            comment.TopicId = (int)Topics.PianoMusic;
            CommentRepostory.Add(comment);
            return RedirectToAction("Details", postId.Value);
        }


        [HttpGet]
        //[Authorize(Roles = "SuperUser")]
        public ActionResult New() {
            if (User.Identity.IsAuthenticated && User.IsInRole("SuperUser"))
                return View(EditPM.Create());
            return RedirectToAction("list");
        }
        [HttpPost]
        //[Authorize(Roles = "SuperUser")]
        public ActionResult New(Piece piece) {
            if (piece == null || !User.Identity.IsAuthenticated) return RedirectToAction("List");
            piece.UserId = UserId;
            PianoPieceRepository.Add(piece);
            return RedirectToAction("List");
        }

        [HttpGet]
        //[Authorize(Roles = "SuperUser")]
        public ActionResult Edit(int? id) {
            if (!id.HasValue) return RedirectToAction("Index");
            var piece = PianoPieceRepository.Get(id.Value);
            if (User.Identity.IsAuthenticated && UserId == piece.UserId)
                return View(piece);
            return RedirectToAction("Index");
        }
        [HttpPost]
        //[Authorize(Roles = "SuperUser")]
        public ActionResult Edit(Piece piece) {
            if (piece != null && User.Identity.IsAuthenticated && UserId == piece.UserId)
                PianoPieceRepository.Update(piece);
            return RedirectToAction("List");
        }

        [HttpGet]
        //[Authorize(Roles = "SuperUser")]
        public ActionResult Delete(int? id) {
            if (!id.HasValue) return RedirectToAction("Index");
            var piece = PianoPieceRepository.Get(id.Value);
            if (User.Identity.IsAuthenticated && UserId == piece.UserId)
                return View(piece);
            return RedirectToAction("Index");
        }
        [HttpPost]
        //[Authorize(Roles = "SuperUser")]
        public ActionResult Delete(int id) {
            var piece = PianoPieceRepository.Get(id);
            if (User.Identity.IsAuthenticated && UserId == piece.UserId)
                PianoPieceRepository.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Admin() {
            return View();
        }
    }
}