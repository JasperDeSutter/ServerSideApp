using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSideApp.Models
{
    public enum Topics
    {
        UNDEFINED = 0,
        PianoMusic = 1,
        Workout = 2,
    }

    public class Topic : INameId
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
}
