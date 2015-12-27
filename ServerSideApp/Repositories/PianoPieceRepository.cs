using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ServerSideApp.Models.Piano;

namespace ServerSideApp.Repositories
{
    public class PianoPieceRepository : RepositoryBase
    {
        private const string TABEL = "PianoPieces";

        public static Piece Get(int id) {
            var command = Prepare(Connect(),
                $"SELECT * FROM {TABEL} WHERE Id = @Id",
                new SqlParameter("@Id", id));
            using (var reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    var result = new Piece {
                        Id = int.Parse(reader["Id"].ToString()),
                        GenreId = int.Parse(reader["GenreId"].ToString()),
                        DifficultyId = int.Parse(reader["DifficultyId"].ToString()),
                        UserId = reader["UserId"].ToString(),
                        Title = reader["Title"].ToString(),
                        Composer = reader["Composer"].ToString(),

                        PdfPath = reader["PdfPath"].ToString(),
                        MidiPath = reader["MidiPath"].ToString(),
                        Mp3Path = reader["Mp3Path"].ToString(),
                    };
                    return result;
                }
            }
            return null;
        }
        public static List<Piece> GetAll() {
            var result = new List<Piece>();
            var command = Prepare(Connect(), $"SELECT * FROM {TABEL}");
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
                    });
                }
            }
            return result;
        }
        public static List<Piece> GetAllFromUser(string userId) {
            var result = new List<Piece>();
            var command = Prepare(Connect(),
                $"SELECT * FROM {TABEL} WHERE UserId=@UserId",
                new SqlParameter("@UserId", userId));
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
                    });
                }
            }
            return result;
        }
        public static int Add(Piece piece) {
            var command = Prepare(Connect(),
                $"INSERT INTO {TABEL} VALUES (@GenreId,@DifficultyId,@UserId,@Title,@Composer,@PdfPath,@MidiPath,@Mp3Path);SELECT @@IDENTITY",
                new SqlParameter("@GenreId", piece.GenreId),
                new SqlParameter("@DifficultyId", piece.DifficultyId),
                new SqlParameter("@UserId", piece.UserId),
                new SqlParameter("@Title", piece.Title),
                new SqlParameter("@Composer", piece.Composer),
                new SqlParameter("@PdfPath", string.IsNullOrEmpty(piece.PdfPath) ? (object)DBNull.Value : piece.PdfPath),
                new SqlParameter("@MidiPath", piece.MidiPath ?? (object)DBNull.Value),
                new SqlParameter("@Mp3Path", piece.Mp3Path ?? (object)DBNull.Value));
            piece.Id = int.Parse(command.ExecuteScalar().ToString());
            return piece.Id;
        }
        public static void Update(Piece piece) {
            var command = Prepare(Connect(),
                $"UPDATE {TABEL} SET VALUES GenreId=@GenreId,DifficultyId=@DifficultyId,UserId=@UserId" +
                ",Title=@Title,Composer=@Composer,PdfPath=@PdfPath,MidiPath=@MidiPath,Mp3Path=@Mp3Path WHERE Id=@Id",
                new SqlParameter("@GenreId", piece.GenreId),
                new SqlParameter("@DifficultyId", piece.DifficultyId),
                new SqlParameter("@UserId", piece.UserId),
                new SqlParameter("@Title", piece.Title),
                new SqlParameter("@Composer", piece.Composer),
                new SqlParameter("@PdfPath", string.IsNullOrEmpty(piece.PdfPath) ? (object)DBNull.Value : piece.PdfPath),
                new SqlParameter("@MidiPath", piece.MidiPath ?? (object)DBNull.Value),
                new SqlParameter("@Mp3Path", piece.Mp3Path ?? (object)DBNull.Value),
                new SqlParameter("@Id", piece.Id));
            command.ExecuteNonQuery();
        }
        public static void Delete(int id) {
            var command = Prepare(Connect(),
                $"DELETE FROM {TABEL} WHERE Id=@Id",
                new SqlParameter("@Id", id));
            command.ExecuteNonQuery();
        }

    }
}