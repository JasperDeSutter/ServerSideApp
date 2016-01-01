using System.Drawing;
using ServerSideApp.Helpers;

namespace ServerSideApp.Models.Workout
{
    public class DetailsPM : Workout
    {
        public bool CanEdit { get; set; }
        public string UserName { get; set; }
        public string Muscle { get; set; }
        public int RatingPercent { get; set; }
        public string RelativeTimeString => Time.ToRelativeTimeString();
        public string RatingColor { get; set; }
        public bool IsRated { get; set; }
    }
}