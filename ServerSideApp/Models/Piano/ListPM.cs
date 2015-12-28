using System.Collections.Generic;

namespace ServerSideApp.Models.Piano
{
    public class ListPM
    {
        public IEnumerable<DetailsPM> PianoPieces { get; set; }
        public bool CanCreate { get; set; }
    }


}