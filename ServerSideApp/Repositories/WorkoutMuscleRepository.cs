using ServerSideApp.Models.Workout;

namespace ServerSideApp.Repositories
{
    public class WorkoutMuscleRepository:NameIdRepositoryBase<Muscle>
    {
        public static void Setup() {
            Table = "WorkoutMuscles";
        }
    }
}