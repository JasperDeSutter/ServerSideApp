using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServerSideApp.Models.Workout
{
    public class Workout:Comment.Comment
    {
        [Required]
        public int MuscleId { get; set; }

        public List<Rep> Reps { get; set; }
        

        public void CopyFrom(Workout workout) {
            Text = workout.Text;
            MuscleId = workout.MuscleId;
            UserId = workout.UserId;
            Reps = workout.Reps;
            Time = workout.Time;
            Id = workout.Id;
            Hidden = workout.Hidden;
        }
    }
}