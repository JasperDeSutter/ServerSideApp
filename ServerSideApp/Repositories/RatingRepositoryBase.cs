using System.Data.SqlClient;

namespace ServerSideApp.Repositories
{
    public class RatingRepositoryBase : RepositoryBase
    {
        private readonly string _table;

        public RatingRepositoryBase(string table) {
            _table = table;
        }

        public float GetRating(int commentId) {
            var command = Prepare(Connect(),
                $"SELECT SUM(Value) / COUNT(Value) FROM {_table} WHERE CommentId = @commentId",
                new SqlParameter("@commentId", commentId));
            var result = command.ExecuteScalar().ToString();
            if (string.IsNullOrEmpty(result)) return float.NaN;
            return float.Parse(result);
        }
        public float GetRating(int commentId, string userId) {
            var command = Prepare(Connect(),
                $"SELECT Value FROM {_table} WHERE CommentId = @commentId && UserId = @userId",
                new SqlParameter("@commentId", commentId),
                new SqlParameter("@userId", userId));
            var result = command.ExecuteScalar().ToString();
            if (string.IsNullOrEmpty(result)) return float.NaN;
            return float.Parse(result);
        }
        private bool IsSet(int commentId, string userId) {
            var command = Prepare(Connect(),
                $"SELECT Value FROM {_table} WHERE UserId = @UserId AND CommentId = @commentId",
                new SqlParameter("@commentId", commentId), new SqlParameter("@UserId", userId));
            var a = command.ExecuteScalar();
            return a != null;
        }
        public void SetRating(int commentId, string userId, float value) {
            var sql = IsSet(commentId,userId)? $"UPDATE {_table} SET Value=@value WHERE CommentId=@commentId AND UserId=@userId":
                $"INSERT INTO {_table} VALUES (@commentId,@userId,@value)";
            var command = Prepare(Connect(), sql,
                new SqlParameter("@commentId", commentId),
                new SqlParameter("@value", value),
                new SqlParameter("@userId", userId));
            command.ExecuteNonQuery();
        }
    }

    public static class WorkoutRatingRepository
    {
        private static readonly RatingRepositoryBase REP = new RatingRepositoryBase("WorkoutRatings");
        public static void SetRating(int commentId, string userId, float value) => REP.SetRating(commentId, userId, value);
        public static float GetRating(int commentId, string userId) => REP.GetRating(commentId, userId);
        public static float GetRating(int commentId) => REP.GetRating(commentId);
    }
}