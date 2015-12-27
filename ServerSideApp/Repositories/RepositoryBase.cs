using System.Configuration;
using System.Data.SqlClient;

namespace ServerSideApp.Repositories
{
    public abstract class RepositoryBase
    {
        protected static readonly string CONNECTIONSTRING =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected static SqlConnection Connect() {
            var con = new SqlConnection(CONNECTIONSTRING);
            con.Open();
            return con;
        }

        protected static SqlCommand Prepare(SqlConnection con, string sql, params SqlParameter[] parameters) {
            var command = new SqlCommand(sql, con);
            foreach (var sqlParameter in parameters) {
                command.Parameters.Add(sqlParameter);
            }
            return command;
        }

    }
}