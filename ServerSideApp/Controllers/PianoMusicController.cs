using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ServerSideApp.Helpers;
using ServerSideApp.Models;
using ServerSideApp.Models.Piano;
using ServerSideApp.Repositories;

namespace ServerSideApp.Controllers
{
    [Authorize]
    public class PianoMusicController : Controller
    {
        private ApplicationUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        private string UserId => UserManager.FindByName(User.Identity.Name).Id;
        private string UserName(string id) => UserManager.FindById(id).UserName;

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index() {
            return View();
        }
        [HttpGet]
        [Authorize]
        public ActionResult List(int? difficulty, int? genre) {
            
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Index");
            var all = PianoPieceRepository.GetAll(difficulty, genre);
            var genres = PianoGenreRepository.GetAll();
            var difficulties = PianoDifficultyRepository.GetAll();
            var isSortedDiff = difficulty.HasValue;
            var isSortedGenre = genre.HasValue;

            var allpm = all.Select(piece => CreateDetailsPM(genres, difficulties, piece)).ToList();
            allpm.Sort((a, b) => b.Upvotes - a.Upvotes);
            var listPm = new ListPM {
                CanCreate = User.IsInRole(Roles.SuperUser),
                PianoPieces = allpm,
                IsSortedGenre = isSortedGenre,
                IsSortedDifficulty = isSortedDiff,
                GenreSelects = isSortedGenre ?
                    genres.CastToSelectListSelectedOn(genre.Value) : genres.CastToSelectList(),
                DifficulitySelects = isSortedDiff ?
                    difficulties.CastToSelectListSelectedOn(difficulty.Value) : difficulties.CastToSelectList(),
            };
            return View(listPm);
        }

        private DetailsPM CreateDetailsPM(IEnumerable<Genre> genres, IEnumerable<Difficulty> difficulties, Piece piece) {
            var add = new DetailsPM {
                HasMidi = !string.IsNullOrEmpty(piece.MidiPath),
                HasMp3 = !string.IsNullOrEmpty(piece.Mp3Path),
                HasPdf = !string.IsNullOrEmpty(piece.PdfPath),
                CanEdit = UserId == piece.UserId,
                UserName = UserName(piece.UserId),
                CommentCount = CommentRepository.GetCommentCount(Topics.PianoMusic, piece.Id),
                Genre = genres.GetNameFromId(piece.GenreId),
                Difficulty = difficulties.GetNameFromId(piece.DifficultyId),
                Upvotes = PianoUpvoteRepository.UpvoteCount(piece.Id),
                CanUpvote = !PianoUpvoteRepository.IsUpvoted(piece.Id, UserId),
            };
            add.CopyFrom(piece);
            return add;
        }

        [HttpGet]
        [Authorize]
        public ActionResult Details(int? id) {
            if (!id.HasValue || !User.Identity.IsAuthenticated) return RedirectToAction("Index");
            var piece = PianoPieceRepository.Get(id.Value);
            var genres = PianoGenreRepository.GetAll();
            var difficulties = PianoDifficultyRepository.GetAll();
            var detailsPm = CreateDetailsPM(genres, difficulties, piece);
            detailsPm.Comments = CommentRepository.Get(Topics.PianoMusic, id);
            return View(detailsPm);
        }

        //public ActionResult GetPdf(int? id) {
        //    if (!User.Identity.IsAuthenticated || !id.HasValue) return RedirectToAction("List");
        //    var filename = PianoPieceRepository.Get(id.Value).PdfPath;
        //    var path = Path.Combine(Server.MapPath("/PianoMusicFiles/"), filename);
        //    var client = new WebClient();

        //    var buffer = client.DownloadData(path);
        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("content-length",buffer.Length.ToString());
        //    Response.BinaryWrite(buffer);
        //    return RedirectToAction("List");
        //}

        [HttpGet]
        [Authorize(Roles = Roles.SuperUser)]
        public ActionResult New() {
            if (User.Identity.IsAuthenticated && User.IsInRole(Roles.SuperUser))
                return View(EditPM.Create());
            return RedirectToAction("list");
        }

        private string GetFullPath(string filename) =>
            Path.Combine(Server.MapPath("/PianoMusicFiles/"), filename);


        [HttpPost]
        [Authorize(Roles = Roles.SuperUser)]
        public ActionResult New(Piece piece, HttpPostedFileBase pdf, HttpPostedFileBase mp3) {
            if (piece == null || !User.Identity.IsAuthenticated) return RedirectToAction("List");

            if (pdf?.ContentLength > 0) {
                var filename = UserId + Path.GetFileName(pdf.FileName);
                pdf.SaveAs(GetFullPath(filename));
                piece.PdfPath = filename;
            }

            if (mp3?.ContentLength > 0) {
                var filename = UserId + Path.GetFileName(mp3.FileName);
                mp3.SaveAs(GetFullPath(filename));
                piece.Mp3Path = filename;
            }
            piece.UserId = UserId;
            PianoPieceRepository.Add(piece);
            return RedirectToAction("List");
        }



        [HttpGet]
        [Authorize(Roles = Roles.SuperUser)]
        public ActionResult Edit(int? id) {
            if (id.HasValue) {
                var piece = PianoPieceRepository.Get(id.Value);
                if (User.Identity.IsAuthenticated && UserId == piece.UserId) {
                    var pm = EditPM.Create();
                    pm.CopyFrom(piece);
                    return View(pm);
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Authorize(Roles = Roles.SuperUser)]
        public ActionResult Edit(Piece piece, HttpPostedFileBase pdf, HttpPostedFileBase mp3) {
            if (piece == null || !User.Identity.IsAuthenticated) return RedirectToAction("List");
            if (PianoPieceRepository.Get(piece.Id).UserId != UserId) return RedirectToAction("List");

            if (pdf?.ContentLength > 0) {
                var filename = UserId + Path.GetFileName(pdf.FileName);
                pdf.SaveAs(GetFullPath(filename));
                piece.PdfPath = filename;
            }

            if (mp3?.ContentLength > 0) {
                var filename = UserId + Path.GetFileName(mp3.FileName);
                mp3.SaveAs(GetFullPath(filename));
                piece.Mp3Path = filename;
            }

            PianoPieceRepository.Update(piece);
            return RedirectToAction("List");
        }

        [HttpGet]
        [Authorize(Roles = Roles.SuperUser)]
        public ActionResult Delete(int? id) {
            if (!id.HasValue || !User.Identity.IsAuthenticated) return RedirectToAction("Index");
            var piece = PianoPieceRepository.Get(id.Value);
            if (UserId == piece.UserId)
                return View(piece);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Authorize(Roles = Roles.SuperUser)]
        public ActionResult Delete(int id) {
            var piece = PianoPieceRepository.Get(id);
            if (User.Identity.IsAuthenticated && UserId == piece.UserId)
                PianoPieceRepository.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = Roles.Administrator)]
        public ActionResult Admin() {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Upvote(int? id, bool? vote) {
            if (id.HasValue && vote.HasValue && User.Identity.IsAuthenticated) {
                PianoUpvoteRepository.SetUpvote(id.Value, UserId, vote.Value);
            }
            return Redirect(Request.UrlReferrer?.ToString());
        }
    }
}