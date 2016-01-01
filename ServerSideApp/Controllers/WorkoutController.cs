using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.ApplicationServices;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ServerSideApp.Helpers;
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
            var muscles = WorkoutMuscleRepository.GetAll();
            var isSorted = muscleArea.HasValue;

            var allpm = all.Select(workout => CreateDetailsPM(workout, muscles)).ToList();
            var listPm = new ListPM {
                WorkoutList = allpm,
                CanCreate = User.IsInRole("SuperUser"),
                MuscleSelects = isSorted ?
                    muscles.CastToSelectListSelectedOn(muscleArea.Value) : muscles.CastToSelectList(),
                CanHide = canHide,
                IsSorted = isSorted,
            };
            return View(listPm);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Details(int? id) {
            if (!id.HasValue || !User.Identity.IsAuthenticated) return RedirectToAction("Index");
            var workout = WorkoutRepository.Get(id.Value);
            var detailsPm = CreateDetailsPM(workout);
            return View(detailsPm);
        }

        private DetailsPM CreateDetailsPM(Workout workout, IEnumerable<Muscle> muscles = null) {
            var rating = WorkoutRatingRepository.GetRating(workout.Id);
            var isRated = !float.IsNaN(rating);
            var result = new DetailsPM {
                IsRated = isRated,
                CanEdit = UserId == workout.UserId,
                UserName = UserName(workout.UserId),
                RatingPercent = isRated ? (int)(rating * 100) : 0,
                RatingColor = isRated ? ColorTranslator.ToHtml(GenerateColor(rating)) : "",
                Muscle = muscles?.GetNameFromId(workout.Id) ?? WorkoutMuscleRepository.Get(workout.MuscleId).Name,
            };
            result.CopyFrom(workout);
            return result;
        }

        private static Color GenerateColor(float rating) {
            var red = rating < 0.5f ? 1f : (1 - rating) / 2;
            var green = rating > 0.5f ? 1f : rating * 2;
            return Color.FromArgb((int)(255 * red), (int)(255 * green), 0);
        }

        [HttpGet]
        [Authorize(Roles = Roles.SuperUser)]
        public ActionResult New() {
            if (!User.Identity.IsAuthenticated || !User.IsInRole(Roles.SuperUser)) return RedirectToAction("list");
            var editPM = new EditPM();
            editPM.Init();
            return View(editPM);
        }


        [HttpPost]
        [Authorize(Roles = Roles.SuperUser)]
        public ActionResult New(EditPM workout, string submit) {
            if (workout == null || !User.Identity.IsAuthenticated || string.IsNullOrEmpty(submit)) return RedirectToAction("List");
            if (submit == "Add") {
                workout.Init();
                var newRep = workout.NewRep;
                workout.Reps.Add(newRep);
                return View(workout);
            }
            workout.UserId = UserId;
            workout.Time = DateTime.Now;
            WorkoutRepository.Add(workout);
            return RedirectToAction("List");
        }

        [HttpGet]
        [Authorize(Roles = Roles.SuperUser)]
        public ActionResult Edit(int? id) {
            if (id.HasValue) {
                var piece = WorkoutRepository.Get(id.Value);
                if (User.Identity.IsAuthenticated && UserId == piece.UserId) {
                    var pm = new EditPM();
                    pm.Init();
                    pm.CopyFrom(piece);
                    return View(pm);
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Authorize(Roles = Roles.SuperUser)]
        public ActionResult Edit(Workout workout) {
            if (workout == null || !User.Identity.IsAuthenticated) return RedirectToAction("List");
            if (WorkoutRepository.Get(workout.Id).UserId != UserId) return RedirectToAction("List");

            WorkoutRepository.Update(workout);
            return RedirectToAction("List");
        }

        [HttpGet]
        [Authorize(Roles = Roles.SuperUser)]
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
        [Authorize(Roles = Roles.Administrator)]
        public ActionResult Admin() {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Rate(int? id, int? vote) {
            if (id.HasValue && vote.HasValue && User.Identity.IsAuthenticated) {
                float value = Math.Min(3, Math.Max(0, vote.Value / 3f));
                WorkoutRatingRepository.SetRating(id.Value, UserId, value);
            }
            var urlReferrer = Request.UrlReferrer;
            if (urlReferrer == null) return RedirectToAction("List");
            return Redirect(urlReferrer.ToString());
        }
    }
}