using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ServerSideApp.Repositories;

namespace ServerSideApp.Models.Workout
{
    public class EditPM:Workout
    {
        public IEnumerable<SelectListItem> Muscles { get; set; }
        public Rep LastRep { get; set; }
        public float Rating { get; set; }

        public static EditPM Create() {
            return new EditPM {
                Muscles = WorkoutMuscleRepository.GetAll().Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() })
            };
        }
    }
}