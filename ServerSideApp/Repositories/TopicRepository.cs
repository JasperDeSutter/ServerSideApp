using System;
using ServerSideApp.Models;

namespace ServerSideApp.Repositories
{
    public class TopicRepository : NameIdRepositoryBase<Topic>
    {
        public static void Setup() {
            Table = "Topics";
        }
    }
}