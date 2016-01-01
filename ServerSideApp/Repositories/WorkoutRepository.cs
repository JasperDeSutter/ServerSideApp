using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Helpers;
using ServerSideApp.Models;
using ServerSideApp.Models.Workout;

namespace ServerSideApp.Repositories
{
    public class WorkoutRepository : RepositoryBase
    {
        public const string TABLE = "Workouts";

        private static List<Workout> GetList(string queryAfterSelect = "", params SqlParameter[] parameters) {
            var result = new List<Workout>();
            var command = Prepare(Connect(), $"SELECT * FROM {TABLE} " + queryAfterSelect, parameters);
            using (var reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    var reps = reader["Reps"].ToString();
                    result.Add(new Workout {
                        Id = int.Parse(reader["Id"].ToString()),
                        UserId = reader["UserId"].ToString(),
                        Reps = Json.Decode<List<Rep>>(reps),
                        TopicId = (int)Topics.Workout,
                        Text = reader["Text"].ToString(),
                        MuscleId = int.Parse(reader["MuscleId"].ToString()),
                        Time = DateTime.Parse(reader["Time"].ToString()),
                        Hidden = bool.Parse(reader["Hidden"].ToString())
                    });
                }
            }
            return result;
        }
        public static List<Workout> GetAll(int? muscleId, bool getHidden) {
            return muscleId.HasValue ?
                GetList("WHERE MuscleId=@muscleId" + (getHidden ? "" : " AND Hidden=0"),
                new SqlParameter("@muscleId", muscleId.Value))
                : GetList();
        }
        public static Workout Get(int id) {
            return GetList("WHERE Id=@id", new SqlParameter("@id", id)).First();
        }

        public static int Add(Workout workout) {
            var command = Prepare(Connect(), $"INSERT INTO {TABLE} VALUES(@MuscleId, @UserId, @Text, @Time, @Reps, 0); SELECT @@IDENTITY",
                new SqlParameter("@MuscleId", workout.MuscleId),
                new SqlParameter("@UserId", workout.UserId),
                new SqlParameter("@Text", workout.Text),
                new SqlParameter("@Time", workout.Time),
                new SqlParameter("@Reps", Json.Encode(workout.Reps)));
            workout.Id = int.Parse(command.ExecuteScalar().ToString());
            return workout.Id;
        }

        public static void Update(Workout workout) {
            var command = Prepare(Connect(), $"UPDATE {TABLE} SET MuscleId=@MuscleId, UserId=@UserId, Text=@Text," +
                " Time=@Time, Reps=@Reps, Hidden=@Hidden WHERE Id=@Id",
                new SqlParameter("@MuscleId", workout.MuscleId),
                new SqlParameter("@UserId", workout.UserId),
                new SqlParameter("@Text", workout.Text),
                new SqlParameter("@Time", workout.Time),
                new SqlParameter("@Reps", Json.Encode(workout.Reps)),
                new SqlParameter("@Id", workout.Id),
                new SqlParameter("@Hidden", workout.Hidden));
            command.ExecuteNonQuery();
        }

        public static void Delete(int id) {
            var command = Prepare(Connect(), $"DELETE FROM {TABLE} WHERE Id=@id",
                new SqlParameter("@id", id));
            command.ExecuteNonQuery();
        }

        public void Hide(int workoutId, bool hide) {
            var command = Prepare(Connect(), $"UPDATE {TABLE} SET Hidden=@hide WHERE Id=@id",
                new SqlParameter("@hide", hide),
                new SqlParameter("@id", workoutId));
            command.ExecuteNonQuery();
        }
    }

}