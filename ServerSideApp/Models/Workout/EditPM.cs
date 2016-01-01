using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ServerSideApp.Repositories;
using ServerSideApp.Helpers;
namespace ServerSideApp.Models.Workout
{
    public class EditPM : Workout
    {
        public IEnumerable<SelectListItem> Muscles { get; set; }
        public Rep NewRep { get; set; }

        public void Init() {
            Muscles =
                WorkoutMuscleRepository.GetAll().CastToSelectList();
                    //.Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            Reps = Reps ?? new List<Rep>();
        }
    }
}