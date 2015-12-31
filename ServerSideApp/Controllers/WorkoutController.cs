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
using ServerSideApp.Models.Workout;
using ServerSideApp.Repositories;

namespace ServerSideApp.Controllers
{
    [Authorize]
    public class WorkoutController : Controller
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
        public ActionResult List(int? muscleArea) {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Index");
            var canHide = User.IsInRole("Administrator");
            var all = WorkoutRepository.GetAll(muscleArea, canHide);
            var allpm = new List<DetailsPM>();
            var muscles = WorkoutMuscleRepository.GetAll();
            var isSorted = muscleArea.HasValue;
            foreach (var workout in all) {
                var add = new DetailsPM {
                    CanEdit = UserId == workout.UserId,
                    UserName = UserName(workout.UserId),
                };
                add.CopyFrom(workout);
                allpm.Add(add);
            };
            var listPm = new ListPM {
                WorkoutList = allpm,
                CanCreate = User.IsInRole("SuperUser"),
                Muscles = muscles.Select(m=>new SelectListItem { Text = m.Name,Value = m.Id.ToString()}),
                CanHide = canHide,
                
            };
            return View(listPm);
        }
        [HttpGet]
        [Authorize]
        public ActionResult Details(int? id) {
            if (!id.HasValue || !User.Identity.IsAuthenticated) return RedirectToAction("Index");
            var workout = WorkoutRepository.Get(id.Value);
            var detailsPm = new DetailsPM {
                CanEdit = User.Identity.IsAuthenticated && UserId == workout.UserId,
                UserName = UserName(workout.UserId),
                Muscle = WorkoutMuscleRepository.Get(workout.MuscleId).Name,
            };
            detailsPm.CopyFrom(workout);
            return View(detailsPm);
        }

        [HttpGet]
        [Authorize(Roles = "SuperUser")]
        public ActionResult New() {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("SuperUser")) return RedirectToAction("list");
            var editPM = EditPM.Create();
            return View(editPM);
        }


        [HttpPost]
        [Authorize(Roles = "SuperUser")]
        public ActionResult New(Workout workout, HttpPostedFileBase pdf, HttpPostedFileBase mp3) {
            if (workout == null || !User.Identity.IsAuthenticated) return RedirectToAction("List");

            workout.UserId = UserId;
            WorkoutRepository.Add(workout);
            return RedirectToAction("List");
        }

        [HttpGet]
        [Authorize(Roles = "SuperUser")]
        public ActionResult Edit(int? id) {
            if (id.HasValue) {
                var piece = WorkoutRepository.Get(id.Value);
                if (User.Identity.IsAuthenticated && UserId == piece.UserId) {
                    var pm = EditPM.Create();
                    pm.CopyFrom(piece);
                    return View(pm);
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Authorize(Roles = "SuperUser")]
        public ActionResult Edit(Workout workout) {
            if (workout == null || !User.Identity.IsAuthenticated) return RedirectToAction("List");
            if (WorkoutRepository.Get(workout.Id).UserId != UserId) return RedirectToAction("List");
            
            WorkoutRepository.Update(workout);
            return RedirectToAction("List");
        }

        [HttpGet]
        [Authorize(Roles = "SuperUser")]
        public ActionResult Delete(int? id) {
            if (!id.HasValue || !User.Identity.IsAuthenticated) return RedirectToAction("Index");
            var workout = WorkoutRepository.Get(id.Value);
            if (UserId == workout.UserId)
                return View(workout);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Authorize(Roles = "SuperUser")]
        public ActionResult Delete(int id) {
            var workout = WorkoutRepository.Get(id);
            if (User.Identity.IsAuthenticated && UserId == workout.UserId)
                WorkoutRepository.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Admin() {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Rate(int? id, float? vote) {
            if (id.HasValue && vote.HasValue && User.Identity.IsAuthenticated) {
                WorkoutRatingRepository.SetRating(id.Value, UserId, vote.Value);
            }
            return Redirect(Request.UrlReferrer?.ToString());
        }
    }
}