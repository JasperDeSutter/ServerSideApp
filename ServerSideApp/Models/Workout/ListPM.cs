using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ServerSideApp.Models.Workout
{
    public class ListPM
    {
        public IEnumerable<DetailsPM> WorkoutList { get; set; }
        public bool CanHide { get; set; }
        public bool CanCreate { get; set; }
        public IEnumerable<SelectListItem> MuscleSelects { get; set; }
        public bool IsSorted { get; set; }
    }
}