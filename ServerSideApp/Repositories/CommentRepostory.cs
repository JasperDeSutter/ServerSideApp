using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using ServerSideApp.Models;
using ServerSideApp.Models.Piano;

namespace ServerSideApp.Repositories
{
    public class CommentRepostory : RepositoryBase
    {
        public static readonly string TABLE = "Comments";

        public static int Add(Comment comment) {
            return 0;
        }
        public static int GetCommentCount(Topics topic, string userId, int? postId) {
            var command = Prepare(Connect(), $"SELECT COUNT(*) FROM {TABLE} WHERE TopicId=@topic AND UserId=@UserId AND PostId=@postId",
                new SqlParameter("@topic", (int)topic),
                new SqlParameter("@UserId", userId),
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
                        Title = reader["Title"].ToString(),
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
    }
}