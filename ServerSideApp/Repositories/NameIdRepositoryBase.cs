using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ServerSideApp.Models;
using ServerSideApp.Models.Piano;

namespace ServerSideApp.Repositories
{
    public abstract class NameIdRepositoryBase<T> : RepositoryBase where T : class, INameId, new()
    {
        public static string Table { get; set; }


        public static T Get(int id) {
            var command = Prepare(Connect(),
                $"SELECT * FROM {Table} WHERE Id = @Id",
                new SqlParameter("@Id", id));
            using (var reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    var result = new T {
                        Id = int.Parse(reader["Id"].ToString()),
                        Name = reader["Name"].ToString()
                    };
                    return result;
                }
            }
            return null;
        }
        public static List<T> GetAll() {
            var result = new List<T>();
            var command = Prepare(Connect(), $"SELECT * FROM {Table}");
            using (var reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    result.Add(new T {
                        Id = int.Parse(reader["Id"].ToString()),
                        Name = reader["Name"].ToString()
                    });
                }
            }
            return result;
        }
        public static int Add(string name) {
            var command = Prepare(Connect(),
                $"INSERT INTO {Table} VALUES (@Name);SELECT @@IDENTITY",
                new SqlParameter("@Name", name));
            return int.Parse(command.ExecuteScalar().ToString());
        }
        public static void Update(T T) {
            var command = Prepare(Connect(),
                $"UPDATE {Table} SET Name=@Name WHERE Id=@Id",
                new SqlParameter("@Name", T.Name),
                new SqlParameter("@Id", T.Id));
            command.ExecuteNonQuery();
        }
        public static void Delete(int id) {
            var command = Prepare(Connect(),
                $"DELETE FROM {Table} WHERE Id=@Id",
                new SqlParameter("@Id", id));
            command.ExecuteNonQuery();
        }
    }
}