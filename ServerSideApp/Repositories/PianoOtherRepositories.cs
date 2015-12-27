using System.Collections.Generic;
using System.Data.SqlClient;
using ServerSideApp.Models.Piano;

namespace ServerSideApp.Repositories
{
    public class PianoGenreRepository : NameIdRepositoryBase<Genre>
    {
        public static void Setup() {
            Table = "PianoGenres";
        }

    }
    public class PianoDifficultyRepository : NameIdRepositoryBase<Difficulty>
    {
        public static void Setup() {
            Table = "PianoDifficulties";
        }

    }


}