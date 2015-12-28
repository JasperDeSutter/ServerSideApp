using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using ServerSideApp.Models;
using ServerSideApp.Models.Comment;

namespace ServerSideApp.Repositories
{
    public class CommentRepository : RepositoryBase
    {
        public static readonly string TABLE = "Comments";

        public static int Add(Comment comment) {
            var command = Prepare(Connect(),
                $"INSERT INTO {TABLE} VALUES (@TopicId,@PostId,@UserId,@Text,@Time,@Hidden,@RatingCount,@RatingTotal);SELECT @@IDENTITY",
                new SqlParameter("@TopicId", comment.TopicId),
                new SqlParameter("@PostId", comment.PostId),
                new SqlParameter("@UserId", comment.UserId),
                new SqlParameter("@Text", comment.Text),
                new SqlParameter("@Time", comment.Time),
                new SqlParameter("@Hidden", comment.Hidden),
                new SqlParameter("@RatingCount", comment.RatingCount),
                new SqlParameter("@RatingTotal", comment.RatingTotal));
            comment.Id = int.Parse(command.ExecuteScalar().ToString());
            return comment.Id;

        }
        public static int GetCommentCount(Topics topic, int? postId) {
            var command = Prepare(Connect(), $"SELECT COUNT(*) FROM {TABLE} WHERE TopicId=@topic AND PostId=@postId",
                new SqlParameter("@topic", (int)topic),
                new SqlParameter("@postId", postId ?? (object)DBNull.Value));
            return int.Parse(command.ExecuteScalar().ToString());
        }

        public static List<Comment> Get(Topics topic, int? postId) {
            var result = new List<Comment>();
            var command = Prepare(Connect(), $"SELECT * FROM {TABLE} WHERE TopicId=@topic AND PostId=@postId",
                new SqlParameter("@topic", (int)topic),
                new SqlParameter("@postId", postId ?? (object)DBNull.Value)
                );
            using (var reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    result.Add(new Comment {
                        Id = int.Parse(reader["Id"].ToString()),
                        TopicId = int.Parse(reader["TopicId"].ToString()),
                        PostId = int.Parse(reader["PostId"].ToString()),
                        UserId = reader["UserId"].ToString(),
                        Text = reader["Text"].ToString(),
                        Time = DateTime.Parse(reader["Time"].ToString()),
                        Hidden = bool.Parse(reader["Hidden"].ToString()),
                        RatingCount = int.Parse(reader["RatingCount"].ToString()),
                        RatingTotal = int.Parse(reader["RatingTotal"].ToString()),
                    });
                }
            }
            return result;

        }

        public static void Hide(int id, bool hide) {
            var command = Prepare(Connect(), $"UPDATE {TABLE} SET Hidden=@hide WHERE Id=@id",
                new SqlParameter("@hide", hide),
                new SqlParameter("@id", id));
            command.ExecuteNonQuery();
        }
    }
}