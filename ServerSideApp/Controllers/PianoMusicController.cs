using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.ApplicationServices;
using System.Web.Mvc;
using System.Web.Security;
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
                    CanEdit = UserId == piece.UserId,
                    UserName = UserName(piece.UserId),
                    CommentCount = CommentRepository.GetCommentCount(Topics.PianoMusic, piece.Id),
                    Genre = PianoGenreRepository.Get(piece.GenreId).Name,
                    Difficulty = PianoDifficultyRepository.Get(piece.DifficultyId).Name
                };
                add.CopyFrom(piece);
                allpm.Add(add);
            }
            var a = User.IsInRole("SuperUser");
            var b = User.IsInRole("User");
            var c = User.IsInRole("Administrator");

            var listPm = new ListPM {
                CanCreate = User.IsInRole("SuperUser"),
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
        [Authorize(Roles = "SuperUser")]
        public ActionResult New() {
            if (User.Identity.IsAuthenticated && User.IsInRole("SuperUser"))
                return View(EditPM.Create());
            return RedirectToAction("list");
        }
        [HttpPost]
        [Authorize(Roles = "SuperUser")]
        public ActionResult New(Piece piece, HttpPostedFileBase pdf, HttpPostedFileBase mp3) {
            if (piece == null || !User.Identity.IsAuthenticated) return RedirectToAction("List");

            if (pdf?.ContentLength > 0) {
                var filename = UserId + Path.GetFileName(pdf.FileName);
                var path = Path.Combine(Server.MapPath("/PianoMusicFiles/"), filename);
                pdf.SaveAs(path);
                piece.PdfPath = filename;
            }

            if (mp3?.ContentLength > 0) {
                var filename = UserId + Path.GetFileName(mp3.FileName);
                var path = Path.Combine(Server.MapPath("/PianoMusicFiles/"), filename);
                mp3.SaveAs(path);
                piece.Mp3Path = filename;
            }
            piece.UserId = UserId;
            PianoPieceRepository.Add(piece);
            return RedirectToAction("List");
        }

        [HttpGet]
        [Authorize(Roles = "SuperUser")]
        public ActionResult Edit(int? id) {
            if (!id.HasValue) return RedirectToAction("Index");
            var piece = PianoPieceRepository.Get(id.Value);
            if (User.Identity.IsAuthenticated && UserId == piece.UserId) {
                var pm = EditPM.Create();
                pm.CopyFrom(piece);
                return View(pm);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Authorize(Roles = "SuperUser")]
        public ActionResult Edit(Piece piece, HttpPostedFileBase pdf, HttpPostedFileBase mp3) {
            if (piece == null || !User.Identity.IsAuthenticated) return RedirectToAction("List");
            if(PianoPieceRepository.Get(piece.Id).UserId != UserId) return RedirectToAction("List");


            if (pdf?.ContentLength > 0) {
                var filename = UserId + Path.GetFileName(pdf.FileName);
                var path = Path.Combine(Server.MapPath("/PianoMusicFiles/"), filename);
                pdf.SaveAs(path);
                piece.PdfPath = filename;
            }

            if (mp3?.ContentLength > 0) {
                var filename = UserId + Path.GetFileName(mp3.FileName);
                var path = Path.Combine(Server.MapPath("/PianoMusicFiles/"), filename);
                mp3.SaveAs(path);
                piece.Mp3Path = filename;
            }

            PianoPieceRepository.Update(piece);
            return RedirectToAction("List");
        }

        [HttpGet]
        [Authorize(Roles = "SuperUser")]
        public ActionResult Delete(int? id) {
            if (!id.HasValue) return RedirectToAction("Index");
            var piece = PianoPieceRepository.Get(id.Value);
            if (User.Identity.IsAuthenticated && UserId == piece.UserId)
                return View(piece);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Authorize(Roles = "SuperUser")]
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