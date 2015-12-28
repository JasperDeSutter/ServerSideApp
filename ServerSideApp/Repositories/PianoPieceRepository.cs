using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using ServerSideApp.Models.Piano;

namespace ServerSideApp.Repositories
{
    public class PianoPieceRepository : RepositoryBase
    {
        private const string TABLE = "PianoPieces";

        private static List<Piece> GetList(string query, params SqlParameter[] parameters) {
            var result = new List<Piece>();
            var command = Prepare(Connect(), query, parameters);
            using (var reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    result.Add(new Piece {
                        Id = int.Parse(reader["Id"].ToString()),
                        GenreId = int.Parse(reader["GenreId"].ToString()),
                        DifficultyId = int.Parse(reader["DifficultyId"].ToString()),
                        UserId = reader["UserId"].ToString(),
                        Title = reader["Title"].ToString(),
                        Composer = reader["Composer"].ToString(),

                        PdfPath = reader["PdfPath"].ToString(),
                        MidiPath = reader["MidiPath"].ToString(),
                        Mp3Path = reader["Mp3Path"].ToString(),

                        Upvotes = int.Parse(reader["Upvotes"].ToString()),
                        Description = reader["Description"].ToString(),
                    });
                }
            }
            return result;
        }
        public static Piece Get(int id) {
            return GetList($"SELECT * FROM {TABLE} WHERE Id = @Id",
                new SqlParameter("@Id", id)).First();
        }
        public static List<Piece> GetAll() {
            return GetList($"SELECT * FROM {TABLE}");
        }
        public static List<Piece> GetAllFromUser(string userId) {
            return GetList($"SELECT * FROM {TABLE} WHERE UserId=@UserId",
                new SqlParameter("@UserId", userId));
        }


        public static int Add(Piece piece) {
            var command = Prepare(Connect(),
                $"INSERT INTO {TABLE} VALUES (@GenreId,@DifficultyId,@UserId,@Title,@Composer,@PdfPath,@MidiPath,@Mp3Path,@Description,@Upvotes);SELECT @@IDENTITY",
                new SqlParameter("@GenreId", piece.GenreId),
                new SqlParameter("@DifficultyId", piece.DifficultyId),
                new SqlParameter("@UserId", piece.UserId),
                new SqlParameter("@Title", piece.Title),
                new SqlParameter("@Composer", piece.Composer),
                new SqlParameter("@PdfPath", string.IsNullOrEmpty(piece.PdfPath) ? (object)DBNull.Value : piece.PdfPath),
                new SqlParameter("@MidiPath", piece.MidiPath ?? (object)DBNull.Value),
                new SqlParameter("@Mp3Path", piece.Mp3Path ?? (object)DBNull.Value),
                new SqlParameter("@Description", piece.Description),
                new SqlParameter("@Upvotes", 0));
            piece.Id = int.Parse(command.ExecuteScalar().ToString());
            return piece.Id;
        }
        public static void Update(Piece piece) {
            var command = Prepare(Connect(),
                $"UPDATE {TABLE} SET VALUES GenreId=@GenreId,DifficultyId=@DifficultyId,UserId=@UserId,Title=@Title,Composer=@Composer," +
                "PdfPath=@PdfPath,MidiPath=@MidiPath,Mp3Path=@Mp3Path,Description=@Description,Upvotes=@Upvotes WHERE Id=@Id",
                new SqlParameter("@GenreId", piece.GenreId),
                new SqlParameter("@DifficultyId", piece.DifficultyId),
                new SqlParameter("@UserId", piece.UserId),
                new SqlParameter("@Title", piece.Title),
                new SqlParameter("@Composer", piece.Composer),
                new SqlParameter("@PdfPath", string.IsNullOrEmpty(piece.PdfPath) ? (object)DBNull.Value : piece.PdfPath),
                new SqlParameter("@MidiPath", piece.MidiPath ?? (object)DBNull.Value),
                new SqlParameter("@Mp3Path", piece.Mp3Path ?? (object)DBNull.Value),
                new SqlParameter("@Id", piece.Id),
                new SqlParameter("@Description", piece.Description),
                new SqlParameter("@Upvotes", piece.Upvotes));
            command.ExecuteNonQuery();
        }
        public static void Delete(int id) {
            var command = Prepare(Connect(),
                $"DELETE FROM {TABLE} WHERE Id=@Id",
                new SqlParameter("@Id", id));
            command.ExecuteNonQuery();
        }

    }
}