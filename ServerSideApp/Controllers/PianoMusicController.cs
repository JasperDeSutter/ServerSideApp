using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ServerSideApp.Models.Piano;
using ServerSideApp.Repositories;

namespace ServerSideApp.Controllers
{
    public class PianoMusicController : Controller
    {
        private ApplicationUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        private string UserId => UserManager.FindByName(User.Identity.Name).Id;
        private string UserName(string Id) => UserManager.FindById(Id).UserName;

        [HttpGet]
        public ActionResult Index() {
            return View();
        }
        [HttpGet]
        public ActionResult List() {
            var all = PianoPieceRepository.GetAll();
            var allpm = new List<ListItemPM>();
            foreach (var piece in all) {
                var add = new ListItemPM {
                    HasMidi = !string.IsNullOrEmpty(piece.MidiPath),
                    HasMp3 = !string.IsNullOrEmpty(piece.Mp3Path),
                    HasPdf = !string.IsNullOrEmpty(piece.PdfPath),
                    CanEdit = User.Identity.IsAuthenticated && UserId == piece.UserId,
                    UserName = UserName(piece.UserId),
                    CommentCount = 1,
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
        public ActionResult Details(int? id) {
            return View();
        }
        [HttpGet]
        //[Authorize(Roles = "SuperUser")]
        public ActionResult New() {
            return View(EditPM.Create());
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
        [Authorize(Roles = "SuperUser")]
        public ActionResult Edit(int? id) {
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "SuperUser")]
        public ActionResult Delete(int? id) {
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Admin() {
            return View();
        }




    }
}