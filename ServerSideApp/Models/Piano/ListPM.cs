using System.Collections.Generic;
using System.Web.Mvc;

namespace ServerSideApp.Models.Piano
{
    public class ListPM 
    {
        public IEnumerable<DetailsPM> PianoPieces { get; set; }
        public bool CanCreate { get; set; }
        public bool IsSortedGenre { get; set; }
        public bool IsSortedDifficulty { get; set; }
        public IEnumerable<SelectListItem> GenreSelects { get; set; }
        public IEnumerable<SelectListItem> DifficulitySelects { get; set; }

    }
}