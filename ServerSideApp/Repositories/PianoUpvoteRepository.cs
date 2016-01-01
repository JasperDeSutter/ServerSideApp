using System.Data.SqlClient;

namespace ServerSideApp.Repositories
{
    public class PianoUpvoteRepository : RepositoryBase
    {
        public const string TABLE = "PianoUpvotes";

        public static bool IsUpvoted(int pieceId, string userId) {
            var command = Prepare(Connect(),
                $"SELECT Upvote FROM {TABLE} WHERE UserId = @UserId AND PieceId = @PieceId AND Upvote = 1",
                new SqlParameter("@PieceId", pieceId), new SqlParameter("@UserId", userId));
            using (var reader = command.ExecuteReader()) {
                return reader.Read();
            }
        }

        private static bool IsSet(int pieceId, string userId) {
            var command = Prepare(Connect(),
                $"SELECT Upvote FROM {TABLE} WHERE UserId = @UserId AND PieceId = @PieceId",
                new SqlParameter("@PieceId", pieceId), new SqlParameter("@UserId", userId));
            var a = command.ExecuteScalar();
            return a != null;
        }

        public static void SetUpvote(int pieceId, string userId, bool upvote) {
            var sql = IsSet(pieceId, userId) ?
                $"UPDATE {TABLE} SET Upvote=@Upvote WHERE UserId = @UserId AND PieceId = @PieceId"
                : $"INSERT INTO {TABLE} VALUES(@PieceId,@UserId,@Upvote)";
            var command = Prepare(Connect(), sql,
                new SqlParameter("@PieceId", pieceId),
                new SqlParameter("@UserId", userId),
                new SqlParameter("@Upvote", upvote));
            command.ExecuteNonQuery();
        }

        public static int UpvoteCount(int pieceId) {
            var command = Prepare(Connect(), $"SELECT COUNT(*) FROM {TABLE} WHERE PieceId=@PieceId AND Upvote=1",
                new SqlParameter("@PieceId", pieceId));
            return int.Parse(command.ExecuteScalar().ToString());
        }
    }
}