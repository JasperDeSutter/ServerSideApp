﻿using System.Web.Mvc;
using ServerSideApp.Repositories;

namespace ServerSideApp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        public ActionResult Index() {
            return View();
        }

        public ActionResult CreateGenre(string name) {
            if (!string.IsNullOrEmpty(name))
                PianoGenreRepository.Add(name);
            return RedirectToAction("Admin", "PianoMusic");
        }
        public ActionResult DeleteGenre(int? id) {
            if (id.HasValue)
                PianoGenreRepository.Delete(id.Value);
            return RedirectToAction("Admin", "PianoMusic");
        }
        public ActionResult EditGenre(Models.Piano.Genre genre) {
            if (!string.IsNullOrEmpty(genre?.Name))
                PianoGenreRepository.Update(genre);
            return RedirectToAction("Admin", "PianoMusic");
        }


        public ActionResult CreateDifficulty(string name) {
            if (!string.IsNullOrEmpty(name))
                PianoDifficultyRepository.Add(name);
            return RedirectToAction("Admin", "PianoMusic");
        }
        public ActionResult DeleteDifficulty(int? id) {
            if (id.HasValue)
                PianoDifficultyRepository.Delete(id.Value);
            return RedirectToAction("Admin", "PianoMusic");
        }
        public ActionResult EditDifficulty(Models.Piano.Difficulty difficulty) {
            if (!string.IsNullOrEmpty(difficulty?.Name))
                PianoDifficultyRepository.Update(difficulty);
            return RedirectToAction("Admin", "PianoMusic");
        }


        public ActionResult CreateMuscle(string name) {
            if (!string.IsNullOrEmpty(name))
                WorkoutMuscleRepository.Add(name);
            return RedirectToAction("Admin", "Workout");
        }
        public ActionResult DeleteMuscle(int? id) {
            if (id.HasValue)
                WorkoutMuscleRepository.Delete(id.Value);
            return RedirectToAction("Admin", "Workout");
        }
        public ActionResult EditMuscle(Models.Workout.Muscle muscle) {
            if (!string.IsNullOrEmpty(muscle?.Name))
                WorkoutMuscleRepository.Update(muscle);
            return RedirectToAction("Admin", "Workout");
        }
    }
}