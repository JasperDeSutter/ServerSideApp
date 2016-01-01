using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ServerSideApp.Repositories;
using ServerSideApp.Helpers;
namespace ServerSideApp.Models.Piano
{
    public class EditPM : Piece
    {
        public IEnumerable<SelectListItem> GenreSelects { get; set; }
        public IEnumerable<SelectListItem> DifficulitySelects { get; set; }

        public static EditPM Create() {
            return new EditPM {
                GenreSelects = PianoGenreRepository.GetAll().CastToSelectList(),
                DifficulitySelects = PianoDifficultyRepository.GetAll().CastToSelectList()
            };
        }
    }
}